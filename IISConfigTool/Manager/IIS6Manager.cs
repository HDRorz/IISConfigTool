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

namespace IISConfigTool.Manager
{
	public class IIS6Manager : IISConfigManager
	{

		public override bool LoadConfig()
		{
			try
			{
				IISConfig = File.OpenRead(ConfigDir);

				doc.Load(IISConfig);

				LoadWebSites();

				Close();

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("IIS读取配置失败" + Environment.NewLine + ex.Message);

				return false;
			}
		}

		#region xml
		private static string ConfigDir = @"C:\Windows\System32\inetsrv\MetaBase.xml";


		/// <summary>
		/// 从IIS6配置文件MetaBase.xml读取IIS所有网站
		/// </summary>
		public override void LoadWebSites()
		{
			WebSites = new List<WebSite>();

			var sites = doc.GetElementsByTagName("IIsWebServer");
			var dirs = doc.GetElementsByTagName("IIsWebVirtualDir");

			Dictionary<string, XmlNode> dirdict = new Dictionary<string, XmlNode>();

			foreach (XmlNode dir in dirs)
			{
				var location = dir.Attributes["Location"];
				var local = location == null ? "" : Convert.ToString(location.Value);

				if (!string.IsNullOrWhiteSpace(local) && !dirdict.ContainsKey(local))
				{
					dirdict.Add(local, dir);
				}

			}

			//Loger.Debug((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(dirdict));

			foreach (XmlNode site in sites)
			{
				var website = new WebSite();

				var location = site.Attributes["Location"];
				string local = "";
				local = location == null ? "" : Convert.ToString(location.Value);
				website.Location = local + "/root";
				website.Id = local.Substring(local.LastIndexOf("/") + 1);

				var name = site.Attributes["ServerComment"];
				website.Name = name == null ? "" : Convert.ToString(name.Value);

				var port = site.Attributes["ServerBindings"];
				website.Port = port == null ? "" : Convert.ToString(port.Value);

				XmlNode dir = default(XmlNode);
				if (dirdict.TryGetValue(website.Location, out dir))
				{
					var path = dir.Attributes["Path"];

					website.Dir = path == null ? "" : Convert.ToString(path.Value);
				}

				WebSites.Add(website);

				foreach (var virtualdir in dirdict.Keys.Where(e => e != website.Location && e.IndexOf(website.Location) >= 0))
				{
					if (dirdict.TryGetValue(virtualdir, out dir))
					{
						var path = dir.Attributes["Path"];

						if (path == null)
						{
							continue;
						}

						var appName = dir.Attributes["AppFriendlyName"];

						var subweb = new WebSite()
						{
							Id = website.Id + "虚拟目录",
							Location = virtualdir,
							Name = appName == null ? "" : Convert.ToString(appName.Value),
							Port = website.Port,
							Dir = Convert.ToString(path.Value),
						};

						WebSites.Add(subweb);
					}
				}


			}

			//Loger.Debug((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(WebSites));

		}


		#endregion

		#region DirectoryEntry
		/// <summary>
		/// 获取允许IP列表
		/// </summary>
		/// <param name="website"></param>
		public override List<string> GetAllowIP(WebSite website)
		{
			var iisDir = new DirectoryEntry("IIS://localhost/W3SVC/" + website.Location.Substring(website.Location.IndexOf("W3SVC") + 6));

			var IPSecurity = iisDir.Properties["IPSecurity"].Value;

			//IPSecurity.GetType().InvokeMember("GrantByDefault", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { true });
			Array origIPAllowList = (Array)IPSecurity.GetType().InvokeMember("IPGrant", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);
			Loger.Debug("原始ip列表" + string.Join(",", origIPAllowList));
			List<string> IPAllowList = new List<string>();
			foreach (string ip in origIPAllowList)
			{
				if (ip.IndexOf(", 255.255.255.255") >= 0)
				{
					IPAllowList.Add(ip.Split(',')[0]);
				}
				else
				{
					IPAllowList.Add(SubnetMaskHelper.GetIPRange(ip));
				}
			}

			iisDir.Close();

			return IPAllowList;
		}

		/// <summary>
		/// 获取阻止IP列表
		/// </summary>
		/// <param name="website"></param>
		public override List<string> GetDenyIP(WebSite website)
		{
			var iisDir = new DirectoryEntry("IIS://localhost/W3SVC/" + website.Location.Substring(website.Location.IndexOf("W3SVC") + 6));

			var IPSecurity = iisDir.Properties["IPSecurity"].Value;

			//IPSecurity.GetType().InvokeMember("GrantByDefault", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { true });
			Array origIPDenyList = (Array)IPSecurity.GetType().InvokeMember("IPDeny", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);
			Loger.Debug("原始ip列表" + string.Join(",", origIPDenyList));
			List<string> IPDenyList = new List<string>();
			foreach (string ip in origIPDenyList)
			{
				if (ip.IndexOf(", 255.255.255.255") >= 0)
				{
					IPDenyList.Add(ip.Split(',')[0]);
				}
				else
				{
					IPDenyList.Add(SubnetMaskHelper.GetIPRange(ip));
				}
			}

			iisDir.Close();

			return IPDenyList;
		}

		/// <summary>
		/// 获取默认IP行为
		/// </summary>
		/// <param name="website"></param>
		/// <returns></returns>
		public override bool GetAllowByDefault(WebSite website)
		{
			var iisDir = new DirectoryEntry("IIS://localhost/W3SVC/" + website.Location.Substring(website.Location.IndexOf("W3SVC") + 6));

			var IPSecurity = iisDir.Properties["IPSecurity"].Value;

			var allowByDefault = IPSecurity.GetType().InvokeMember("GrantByDefault", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);

			iisDir.Close();

			return (bool)allowByDefault;
		}

		/// <summary>
		/// 设置默认IP行为
		/// </summary>
		/// <param name="website"></param>
		/// <param name="allowByDefault"></param>
		public override void SetAllowByDefault(WebSite website, bool allowByDefault)
		{
			var iisDir = new DirectoryEntry("IIS://localhost/W3SVC/" + website.Location.Substring(website.Location.IndexOf("W3SVC") + 6));

			var IPSecurity = iisDir.Properties["IPSecurity"].Value;

			IPSecurity.GetType().InvokeMember("GrantByDefault", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { allowByDefault });

			iisDir.Properties["IPSecurity"].Value = IPSecurity;

			iisDir.CommitChanges();

			iisDir.RefreshCache();

			iisDir.Close();
		}

		/// <summary>
		/// 设置允许访问IP
		/// </summary>
		/// <param name="location"></param>
		/// <param name="ipaddr"></param>
		public override void AddAllowIP(WebSite website, List<string> ipadds)
		{
			var iisDir = new DirectoryEntry("IIS://localhost/W3SVC/" + website.Location.Substring(website.Location.IndexOf("W3SVC") + 6));

			var IPSecurity = iisDir.Properties["IPSecurity"].Value;

			//IPSecurity.GetType().InvokeMember("GrantByDefault", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { true });
			Array origIPAllowList = (Array)IPSecurity.GetType().InvokeMember("IPGrant", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);
			Loger.Debug("原始ip列表" + string.Join(",", origIPAllowList));
			List<object> IPAllowList = new List<object>();

			foreach (string ip in origIPAllowList)
			{
				IPAllowList.Add(ip);
			}
			foreach (string ip in ipadds)
			{
				if (IPAllowList.Count(e => ((string)e) == ip) == 0)
				{
					IPAllowList.Add(ip);
				}
			}

			Loger.Debug("ip列表" + string.Join(",", IPAllowList));

			object[] ipList = IPAllowList.ToArray();

			IPSecurity.GetType().InvokeMember("IPGrant", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { ipList });

			iisDir.Properties["IPSecurity"].Value = IPSecurity;

			iisDir.CommitChanges();

			iisDir.RefreshCache();

			iisDir.Close();

		}

		/// <summary>
		/// 设置组织访问IP
		/// </summary>
		/// <param name="location"></param>
		/// <param name="ipaddr"></param>
		public override void AddDenyIP(WebSite website, List<string> ipadds)
		{
			var iisDir = new DirectoryEntry("IIS://localhost/W3SVC/" + website.Location.Substring(website.Location.IndexOf("W3SVC") + 6));

			//Loger.Debug((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(iisDir));
			Loger.Debug("原始列表" + string.Join(",", iisDir.Properties));
			//Loger.Debug("原始列表" + (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(iisDir.Properties.PropertyNames));
			var IPSecurity = iisDir.Properties["IPSecurity"].Value;

			//IPSecurity.GetType().InvokeMember("GrantByDefault", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { true });
			Array origIPDenyList = (Array)IPSecurity.GetType().InvokeMember("IPDeny", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);
			Loger.Debug("原始ip列表" + string.Join(",", origIPDenyList));
			List<object> IPDenyList = new List<object>();

			foreach (string ip in origIPDenyList)
			{
				IPDenyList.Add(ip);
			}
			foreach (string ip in ipadds)
			{
				if (IPDenyList.Count(e => ((string)e) == ip) == 0)
				{
					IPDenyList.Add(ip);
				}
			}

			Loger.Debug("ip列表" + string.Join(",", IPDenyList));

			object[] ipList = IPDenyList.ToArray();

			IPSecurity.GetType().InvokeMember("IPDeny", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { ipList });

			iisDir.Properties["IPSecurity"].Value = IPSecurity;

			iisDir.CommitChanges();

			iisDir.RefreshCache();

			iisDir.Close();

		}

		#endregion



	}
}
