using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IISConfigTool.Entity
{
	public class WebSite
	{

		/// <summary>
		/// 是否选中
		/// </summary>
		public bool IsSelected { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 网站名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 网站路径
		/// </summary>
		public string Dir { get; set; }

		/// <summary>
		/// 端口号
		/// </summary>
		public string Port { get; set; }

		/// <summary>
		/// DirectoryEntry
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// 默认全局允许访问
		/// </summary>
		public bool AllowByDefault { get; set; }

		/// <summary>
		/// 允许ip列表
		/// </summary>
		public List<string> IPAllowList { get; set; }

		/// <summary>
		/// 阻止ip列表
		/// </summary>
		public List<string> IPDenyList { get; set; }
	}
}
