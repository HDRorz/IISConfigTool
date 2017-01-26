using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IISConfigTool.Manager
{
	/// <summary>
	/// 算子网掩码组合
	/// </summary>
	public class SubnetMaskHelper
	{
		private bool UseSubNetMask = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSubNetMask"]);

		public static Regex IP = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled);

		private string orginInput;
		private List<int> ips = new List<int>();
		private string firstpart;
		private int start;
		private int end;

		private List<IPMask> IPMaskList;
		private List<int> Mask = new List<int>() { 128, 192, 224, 240, 248, 252 };

		private List<SubnetMaskHelperRet> Result = new List<SubnetMaskHelperRet>();

		public string StartIp { get; private set; }

		public SubnetMaskHelper(string ipstring)
		{
			orginInput = ipstring;

			GetIPs();

		}


		private void GetIPs()
		{
			var temp = orginInput.Split('-');
			if (temp.Length != 2)
			{
				throw new Exception("输入格式不正确");
			}

			StartIp = temp[0];
			end = Convert.ToInt32(temp[1]);
			if (end <= 0 || end > 255)
			{
				throw new Exception("输入格式不正确");
			}

			if (!IP.IsMatch(StartIp))
			{
				throw new Exception("输入格式不正确");
			}

			firstpart = StartIp.Substring(0, StartIp.LastIndexOf('.') + 1);

			start = Convert.ToInt32(StartIp.Split('.').Last());

			for (int i = start; i <= end; i++)
			{
				ips.Add(i);
			}
		}


		public List<SubnetMaskHelperRet> GetResult()
		{
			if (!UseSubNetMask)
			{
				foreach (var ip in ips)
				{
					Result.Add(
						new SubnetMaskHelperRet()
						{
							IP = firstpart + ip,
							Mask = "255.255.255.255"
						});
				}

				return Result;
			}

			foreach (var mask in Mask)
			{
				IPMaskList = new List<IPMask>();

				foreach (var ip in ips)
				{
					IPMaskList.Add(
						new IPMask()
						{
							IP = ip,
							Mask = mask,
							AndRet = ip & mask
						});
				}

				var groupby = IPMaskList.GroupBy(e => e.AndRet, (andRet, ipmasklist) => new { Key = andRet, Count = ipmasklist.Count() });

				var okmasks = groupby.Where(e => e.Count == (256 - mask));

				foreach (var okmask in okmasks)
				{
					Result.Add(
						new SubnetMaskHelperRet() {
							IP = firstpart + IPMaskList.Where(e => e.AndRet == okmask.Key).FirstOrDefault().IP.ToString(),
							Mask = "255.255.255." + mask
						});

					//删除已被子网掩码覆盖到的ip
					foreach (var ipmask in IPMaskList.Where(e => e.AndRet == okmask.Key))
					{
						ips.Remove(ipmask.IP);
					}
				}

			}

			foreach (var ip in ips)
			{
				Result.Add(
					new SubnetMaskHelperRet()
					{
						IP = firstpart + ip,
						Mask = "255.255.255.255"
					});
			}


			return Result;
		}


		/// <summary>
		/// 将子网表示为ip范围
		/// </summary>
		/// <param name="ipmask"></param>
		/// <returns></returns>
		public static string GetIPRange(string ipmask)
		{
			var temp = ipmask.Split(',');

			if (temp.Length != 2)
			{
				return "解析失败";
			}

			string ip = temp[0];
			string mask = temp[1];

			if (mask.Contains("255.255.255.255"))
			{
				return ip;
			}

			string ipfirstpart = ip.Substring(0, ip.LastIndexOf('.') + 1);
			int iplast = Convert.ToInt32(ip.Substring(ip.LastIndexOf('.') + 1));
			int masklast = Convert.ToInt32(mask.Substring(mask.LastIndexOf('.') + 1));

			int ipstart = iplast & masklast;
			int ipend = ipstart + 255 - masklast;

			return ipfirstpart + ipstart.ToString() + "-" + ipend.ToString();
		}

	}



	public class IPMask
	{
		public int IP { get; set; }
		public int Mask { get; set; }
		public int AndRet { get; set; }
	}

	public class SubnetMaskHelperRet
	{
		public string IP { get; set; }
		public string Mask { get; set; }
	}


}
