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
	public abstract class IISConfigManager
	{

		protected object _lock = new object();

		

		protected StringBuilder Buffer = new StringBuilder();

		public List<WebSite> WebSites { get; protected set; }

		private static string W3wpDir= @"C:\Windows\System32\inetsrv\w3wp.exe";


		private static int _IISVersion = 0;
		public static int IISVersion
		{
			get {
				if (_IISVersion != 0)
				{
					return _IISVersion;
				}

				FileVersionInfo W3wpInfo = FileVersionInfo.GetVersionInfo(W3wpDir);

				//Loger.Debug(W3wpInfo.FileVersion);

				_IISVersion = W3wpInfo.FileMajorPart;

				return _IISVersion;
			}
		}

		public abstract bool LoadConfig();

		#region xml

		protected FileStream IISConfig;
		protected XmlDocument doc = new XmlDocument();



		/// <summary>
		/// 从IIS配置文件读取IIS所有网站
		/// </summary>
		public abstract void LoadWebSites();

		/// <summary>
		/// 关闭文件流
		/// </summary>
		protected void Close()
		{
			if (IISConfig != null)
			{
				IISConfig.Close();
			}
		}

		#endregion


		#region DirectoryEntry
		//private static DirectoryEntry IISServices = new DirectoryEntry("IIS://localhost/W3SVC");


		#endregion


		#region IIS操作方法

		/// <summary>
		/// 获取允许IP列表
		/// </summary>
		/// <param name="website"></param>
		public abstract List<string> GetAllowIP(WebSite website);

		/// <summary>
		/// 获取阻止IP列表
		/// </summary>
		/// <param name="website"></param>
		public abstract List<string> GetDenyIP(WebSite website);

		/// <summary>
		/// 获取默认IP行为
		/// </summary>
		/// <param name="website"></param>
		/// <returns></returns>
		public abstract bool GetAllowByDefault(WebSite website);

		/// <summary>
		/// 设置默认IP行为
		/// </summary>
		/// <param name="website"></param>
		/// <param name="allowByDefault"></param>
		public abstract void SetAllowByDefault(WebSite website, bool allowByDefault);

		/// <summary>
		/// 设置允许访问IP
		/// </summary>
		/// <param name="location"></param>
		/// <param name="ipaddr"></param>
		public abstract void AddAllowIP(WebSite website, List<string> ipadds);

		/// <summary>
		/// 设置组织访问IP
		/// </summary>
		/// <param name="location"></param>
		/// <param name="ipaddr"></param>
		public abstract void AddDenyIP(WebSite website, List<string> ipadds);

		#endregion



		public void ClearBuffer()
		{
			Buffer.Clear();
		}

		public string GetBuffer()
		{
			return Buffer.ToString();
		}

	}
}
