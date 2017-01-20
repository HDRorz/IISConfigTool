using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IISConfigTool.Entity;
using Microsoft.Web.Administration;

namespace IISConfigTool.Manager
{
	public class IIS7Manager : IISConfigManager
	{
		private static string Appcmd = "C:\\Windows\\System32\\inetsrv\\appcmd.exe";
		private static string ParamFormat = "set config \"{0}\" -section:system.webServer/security/ipSecurity /+\"[ipAddress='{1}',allowed='{2}']\" /commit:apphost";
		private static string ParamFormat2 = "set config \"{0}\" -section:system.webServer/security/ipSecurity /+\"[ipAddress='{1}',subnetMask='{2}',allowed='{3}']\" /commit:apphost";
		private static string ParamFormat3 = "list config \"{0}\" -section:system.webServer/security/ipSecurity";
		private static string ParamFormat4 = "set config \"{0}\" -section:system.webServer/security/ipSecurity /allowUnlisted:{1} /commit:apphost";

		private static string ConfigDir = @"C:\Windows\System32\inetsrv\config\applicationHost.config";


		private Dictionary<string, string> IPSecurityConfig = new Dictionary<string, string>();

		public override bool LoadConfig()
		{
			try
			{
				LoadWebSitesByWebAdmin();

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("IIS读取配置失败" + Environment.NewLine + ex.Message);

				return false;
			}
		}



		#region xml


		/// <summary>
		/// 从IIS配置文件读取IIS所有网站
		/// </summary>
		public override void LoadWebSites()
		{
			WebSites = new List<WebSite>();

			var sites = doc.GetElementsByTagName("site");

			foreach (XmlNode site in sites)
			{
				var website = new WebSite();

				var id = site.Attributes["id"];
				if (id != null)
				{
					website.Id = Convert.ToString(id.Value);
				}

				var name = site.Attributes["name"];
				if (name != null)
				{
					website.Name = Convert.ToString(name.Value);
				}

				foreach (XmlNode node in site.ChildNodes)
				{
					if (node.Name.IndexOf("application") >= 0)
					{
						if (node.ChildNodes.Count > 0)
						{
							var dir = node.FirstChild.Attributes["physicalPath"];
							if (dir != null)
							{
								website.Dir = Convert.ToString(dir.Value);
							}
						}

						break;
					}
				}

				foreach (XmlNode node in site.ChildNodes)
				{
					if (node.Name.IndexOf("bindings") >= 0)
					{
						var ports = new List<string>();

						foreach (XmlNode bind in node.ChildNodes)
						{
							var protocol = bind.Attributes["protocol"];
							if (protocol != null)
							{
								if (protocol.Value.ToString() == "http")
								{
									var port = bind.Attributes["bindingInformation"];
									if (port != null)
									{
										ports.Add(Convert.ToString(port.Value));
									}
								}
							}
						}

						website.Port = string.Join(", ", ports);

						break;
					}
				}

				WebSites.Add(website);

			}


		}

		#endregion




		#region DirectoryEntry
		//private static DirectoryEntry IISServices = new DirectoryEntry("IIS://localhost/W3SVC");


		//public static void LoadWebSites()
		//{

		//	WebSites = new List<WebSite>();

		//	foreach (DirectoryEntry node in IISServices.Children)
		//	{
		//		if (node.SchemaClassName == "IIsWebServer" )
		//		{
		//			var site = new WebSite();

		//			site.Id = node.Name;
		//			site.Name = node.Properties["ServerComment"].Value.ToString();
		//			site.Port = node.Properties["ServerBindings"].Value.ToString();
		//			WebSites.Add(site);
		//		}
		//	}

		//}


		#endregion


		#region Web.Administration

		private ServerManager serverManager;
		private static Configuration appHostConfig;

		public void LoadWebSitesByWebAdmin()
		{
			serverManager = new ServerManager();
			appHostConfig = serverManager.GetApplicationHostConfiguration();

			WebSites = new List<WebSite>();

			foreach (var site in serverManager.Sites)
			{
				var website = new WebSite();
				website.Id = site.Id.ToString();
				website.Name = site.Name;
				website.Port = string.Join(", ", site.Bindings.Where(e => e.Protocol == "http").Select(e => e.BindingInformation));
				website.Dir = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;

				WebSites.Add(website);

				foreach (var virtualdir in site.Applications["/"].VirtualDirectories.Where(e => e.Path != "/"))
				{
					var subweb = new WebSite()
					{
						Id = website.Id + "虚拟目录",
						Name = website.Name + virtualdir.Path,
						Dir = virtualdir.PhysicalPath,
						Port = website.Port
					};

					WebSites.Add(subweb);
				}

			}


		}

		/// <summary>
		/// 保存webadmin更改
		/// </summary>
		private void SaveWebAdminChange()
		{
			lock (_lock)
			{
				serverManager.CommitChanges();
				serverManager.Dispose();
				serverManager = new ServerManager();
				appHostConfig = serverManager.GetApplicationHostConfiguration();
			}
		}

		/// <summary>
		/// 获取默认IP行为
		/// </summary>
		/// <param name="website"></param>
		/// <returns></returns>
		public override bool GetAllowByDefault(WebSite website)
		{
			var IPSecurity = appHostConfig.GetSection("system.webServer/security/ipSecurity", website.Name);

			return (bool)IPSecurity.Attributes["allowUnlisted"].Value;
		}

		/// <summary>
		/// 设置默认IP行为
		/// </summary>
		/// <param name="website"></param>
		/// <param name="allowByDefault"></param>
		public override void SetAllowByDefault(WebSite website, bool allowByDefault)
		{
			var IPSecurity = appHostConfig.GetSection("system.webServer/security/ipSecurity", website.Name);

			IPSecurity.SetAttributeValue("allowUnlisted", allowByDefault);

			SaveWebAdminChange();
		}

		/// <summary>
		/// 获取允许IP列表
		/// </summary>
		/// <param name="website"></param>
		public override List<string> GetAllowIP(WebSite website)
		{
			List<string> AllowIPList = new List<string>();

			var IPSecurity = appHostConfig.GetSection("system.webServer/security/ipSecurity", website.Name);

			var ipSecurityCollection = IPSecurity.GetCollection();

			foreach (var add in ipSecurityCollection)
			{
				var ip = add.Attributes["ipAddress"];
				var ipAddress = ip == null ? "" : Convert.ToString(ip.Value);

				var mask = add.Attributes["subnetMask"];
				var subnetMask = mask == null ? "" : Convert.ToString(mask.Value);

				var allow = add.Attributes["allowed"];
				var allowip = Convert.ToBoolean(allow == null ? "" : allow.Value);

				if (allowip)
				{
					AllowIPList.Add(SubnetMaskHelper.GetIPRange(ipAddress + "," + subnetMask));
				}
			}

			return AllowIPList;
		}


		/// <summary>
		/// 获取阻止IP列表
		/// </summary>
		/// <param name="website"></param>
		public override List<string> GetDenyIP(WebSite website)
		{
			List<string> DenyIPList = new List<string>();

			var IPSecurity = appHostConfig.GetSection("system.webServer/security/ipSecurity", website.Name);

			var ipSecurityCollection = IPSecurity.GetCollection();

			foreach (var add in ipSecurityCollection)
			{
				var ip = add.Attributes["ipAddress"];
				var ipAddress = ip == null ? "" : Convert.ToString(ip.Value);

				var mask = add.Attributes["subnetMask"];
				var subnetMask = mask == null ? "" : Convert.ToString(mask.Value);

				var allow = add.Attributes["allowed"];
				var allowip = Convert.ToBoolean(allow == null ? "" : allow.Value);

				if (!allowip)
				{
					DenyIPList.Add(SubnetMaskHelper.GetIPRange(ipAddress + "," + subnetMask));
				}
			}

			return DenyIPList;
		}

		/// <summary>
		/// 设置允许访问IP
		/// </summary>
		/// <param name="location"></param>
		/// <param name="ipaddr"></param>
		public override void AddAllowIP(WebSite website, List<string> ipadds)
		{

			var IPSecurity = appHostConfig.GetSection("system.webServer/security/ipSecurity", website.Name);

			var ipSecurityCollection = IPSecurity.GetCollection();

			var toremove = new List<ConfigurationElement>();

			foreach (var add in ipSecurityCollection)
			{
				var allow = add.Attributes["allowed"];
				var allowip = Convert.ToBoolean(allow == null ? "" : allow.Value);

				if (allowip)
				{
					toremove.Add(add);
				}
			}

			toremove.ForEach(e => ipSecurityCollection.Remove(e));

			foreach (var ip in ipadds)
			{
				var temp = ip.Split(',');

				ConfigurationElement addElement = ipSecurityCollection.CreateElement("add");

				if (temp.Length == 1)
				{
					addElement["ipAddress"] = ip;
					addElement["allowed"] = true;
				}
				else if (temp.Length == 2)
				{
					addElement["ipAddress"] = temp[0].Trim();
					addElement["subnetMask"] = temp[1].Trim();
					addElement["allowed"] = true;
				}

				ipSecurityCollection.Add(addElement);
			}

			SaveWebAdminChange();
		}

		/// <summary>
		/// 设置阻止访问IP
		/// </summary>
		/// <param name="location"></param>
		/// <param name="ipaddr"></param>
		public override void AddDenyIP(WebSite website, List<string> ipadds)
		{
			var IPSecurity = appHostConfig.GetSection("system.webServer/security/ipSecurity", website.Name);

			var ipSecurityCollection = IPSecurity.GetCollection();

			var toremove = new List<ConfigurationElement>();

			foreach (var add in ipSecurityCollection)
			{
				var allow = add.Attributes["allowed"];
				var allowip = Convert.ToBoolean(allow == null ? "" : allow.Value);

				if (!allowip)
				{
					toremove.Add(add);
				}
			}

			toremove.ForEach(e => ipSecurityCollection.Remove(e));

			foreach (var ip in ipadds)
			{
				var temp = ip.Split(',');

				ConfigurationElement addElement = ipSecurityCollection.CreateElement("add");

				if (temp.Length == 1)
				{
					addElement["ipAddress"] = ip;
					addElement["allowed"] = false;
				}
				else if (temp.Length == 2)
				{
					addElement["ipAddress"] = temp[0].Trim();
					addElement["subnetMask"] = temp[1].Trim();
					addElement["allowed"] = false;
				}

				ipSecurityCollection.Add(addElement);
			}

			SaveWebAdminChange();
		}

		#endregion


		///// <summary>
		///// 设置允许访问IP
		///// </summary>
		///// <param name="location"></param>
		///// <param name="ipaddr"></param>
		//public override void AddAllowIP(WebSite website, List<string> ipadds)
		//{
		//	foreach (var ip in ipadds)
		//	{
		//		var temp = ip.Split(',');

		//		if (temp.Length == 1)
		//		{
		//			AddAllowIP(website.Name, ip);
		//		}
		//		else if (temp.Length == 2)
		//		{
		//			AddAllowIPWithSubnetMask(website.Name, temp[0].Trim(), temp[1].Trim());
		//		}
		//	}

		//	IPSecurityConfig.Remove(website.Name);
		//}

		///// <summary>
		///// 设置阻止访问IP
		///// </summary>
		///// <param name="location"></param>
		///// <param name="ipaddr"></param>
		//public override void AddDenyIP(WebSite website, List<string> ipadds)
		//{
		//	foreach (var ip in ipadds)
		//	{
		//		var temp = ip.Split(',');

		//		if (temp.Length == 1)
		//		{
		//			AddDenyIP(website.Name, ip);
		//		}
		//		else if (temp.Length == 2)
		//		{
		//			AddDenyIPWithSubnetMask(website.Name, temp[0].Trim(), temp[1].Trim());
		//		}
		//	}

		//	IPSecurityConfig.Remove(website.Name);
		//}


		#region appcmd


		/// <summary>
		/// 获得IIS IPSecurity配置
		/// </summary>
		/// <param name="website"></param>
		public void GetIPSecurity(WebSite website)
		{
			ClearBuffer();

			string param = string.Format(ParamFormat3, website.Name);
			ExcuteCmd(param);

			var output = GetBuffer();

			if (output.Contains("ERROR"))
			{
				IPSecurityConfig[website.Name] = "";
			}
			else
			{
				IPSecurityConfig[website.Name] = output;
			}
		}

		///// <summary>
		///// 获取允许IP列表
		///// </summary>
		///// <param name="website"></param>
		//public override List<string> GetAllowIP(WebSite website)
		//{
		//	if (!IPSecurityConfig.ContainsKey(website.Name))
		//	{
		//		GetIPSecurity(website);
		//	}

		//	var config = IPSecurityConfig[website.Name];

		//	List<string> AllowIPList = new List<string>();
		//	var xml = new XmlDocument();
		//	xml.LoadXml(config);

		//	var adds = xml.GetElementsByTagName("add");
		//	foreach (XmlNode add in adds)
		//	{
		//		var ip = add.Attributes["ipAddress"];
		//		var ipAddress = ip == null ? "" : Convert.ToString(ip.Value);

		//		var mask = add.Attributes["subnetMask"];
		//		var subnetMask = mask == null ? "" : Convert.ToString(mask.Value);

		//		var allow = add.Attributes["allowed"];
		//		var allowip = Convert.ToBoolean(allow == null ? "" : allow.Value);

		//		if (allowip)
		//		{
		//			AllowIPList.Add(SubnetMaskHelper.GetIPRange(ipAddress + "," + subnetMask));
		//		}
		//	}

		//	return AllowIPList;
		//}

		///// <summary>
		///// 获取阻止IP列表
		///// </summary>
		///// <param name="website"></param>
		//public override List<string> GetDenyIP(WebSite website)
		//{
		//	if (!IPSecurityConfig.ContainsKey(website.Name))
		//	{
		//		GetIPSecurity(website);
		//	}

		//	var config = IPSecurityConfig[website.Name];

		//	List<string> DenyIPList = new List<string>();
		//	var xml = new XmlDocument();
		//	xml.LoadXml(config);

		//	var adds = xml.GetElementsByTagName("add");
		//	foreach (XmlNode add in adds)
		//	{
		//		var ip = add.Attributes["ipAddress"];
		//		var ipAddress = ip == null ? "" : Convert.ToString(ip.Value);

		//		var mask = add.Attributes["subnetMask"];
		//		var subnetMask = mask == null ? "" : Convert.ToString(mask.Value);

		//		var allow = add.Attributes["allowed"];
		//		var allowip = Convert.ToBoolean(allow == null ? "" : allow.Value);

		//		if (!allowip)
		//		{
		//			DenyIPList.Add(SubnetMaskHelper.GetIPRange(ipAddress + "," + subnetMask));
		//		}
		//	}

		//	return DenyIPList;
		//}

		///// <summary>
		///// 获取默认IP行为
		///// </summary>
		///// <param name="website"></param>
		///// <returns></returns>
		//public override bool GetAllowByDefault(WebSite website)
		//{
		//	if (!IPSecurityConfig.ContainsKey(website.Name))
		//	{
		//		GetIPSecurity(website);
		//	}

		//	var config = IPSecurityConfig[website.Name];

		//	List<string> DenyIPList = new List<string>();
		//	var xml = new XmlDocument();
		//	xml.LoadXml(config);

		//	var ipSecurity = xml.GetElementsByTagName("ipSecurity");

		//	if (ipSecurity.Count != 1)
		//	{
		//		SetAllowByDefault(website, true);
		//		return true;
		//	}

		//	var allowUnlisted = ipSecurity[0].Attributes["allowUnlisted"];

		//	var ret = Convert.ToBoolean(allowUnlisted == null ? "true" : allowUnlisted.Value);

		//	return ret;
		//}

		///// <summary>
		///// 设置默认IP行为
		///// </summary>
		///// <param name="website"></param>
		///// <param name="allowByDefault"></param>
		//public override void SetAllowByDefault(WebSite website, bool allowByDefault)
		//{
		//	ClearBuffer();

		//	string param = string.Format(ParamFormat4, website.Name, allowByDefault.ToString());
		//	ExcuteCmd(param);
		//}


		/// <summary>
		/// 设置允许访问IP
		/// </summary>
		/// <param name="webName"></param>
		/// <param name="ipaddr"></param>
		public void AddAllowIP(string webName, string ipaddr)
		{
			Buffer.Append(webName);
			Buffer.Append(" Allow ");
			Buffer.Append(ipaddr);

			string param = string.Format(ParamFormat, webName, ipaddr, "True");
			ExcuteCmd(param);
		}

		/// <summary>
		/// 设置允许访问IP网段
		/// </summary>
		/// <param name="webName"></param>
		/// <param name="ipaddr"></param>
		public void AddAllowIPWithSubnetMask(string webName, string ipaddr, string subnetMask)
		{
			Buffer.Append(webName);
			Buffer.Append(" Allow ");
			Buffer.Append(ipaddr);
			Buffer.Append("（");
			Buffer.Append(subnetMask);
			Buffer.Append("）");

			string param = string.Format(ParamFormat2, webName, ipaddr, subnetMask, "True");
			ExcuteCmd(param);
		}

		/// <summary>
		/// 设置阻止访问IP
		/// </summary>
		/// <param name="webName"></param>
		/// <param name="ipaddr"></param>
		public void AddDenyIP(string webName, string ipaddr)
		{
			Buffer.Append(webName);
			Buffer.Append(" Deny ");
			Buffer.Append(ipaddr);

			string param = string.Format(ParamFormat, webName, ipaddr, "False");
			ExcuteCmd(param);
		}

		/// <summary>
		/// 设置允许访问IP网段
		/// </summary>
		/// <param name="webName"></param>
		/// <param name="ipaddr"></param>
		public void AddDenyIPWithSubnetMask(string webName, string ipaddr, string subnetMask)
		{
			Buffer.Append(webName);
			Buffer.Append(" Allow ");
			Buffer.Append(ipaddr);
			Buffer.Append("（");
			Buffer.Append(subnetMask);
			Buffer.Append("）");

			string param = string.Format(ParamFormat2, webName, ipaddr, subnetMask, "False");
			ExcuteCmd(param);
		}

		/// <summary>
		/// 执行命令
		/// </summary>
		/// <param name="param"></param>
		private void ExcuteCmd(string param)
		{
			Process p = new Process();
			p.StartInfo.FileName = Appcmd;//需要启动的程序名  
			p.StartInfo.Arguments = param;//启动参数  
			p.StartInfo.UseShellExecute = false;//设置false可以重定向输出流
			p.StartInfo.RedirectStandardOutput = true;//重定向输出流
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//设定启动时不显示命令行窗体
			p.StartInfo.CreateNoWindow = true;//设定启动时不显示命令行窗体
			p.Start();//启动  
			p.WaitForExit();//等待运行结束
			if (p.HasExited)//判断是否运行结束  
			{
				Buffer.Append(p.StandardOutput.ReadToEnd());
			}
		}
		#endregion


	}
}
