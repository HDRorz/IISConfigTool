using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using IISConfigTool.Entity;
using IISConfigTool.Manager;

namespace IISConfigTool
{
	public partial class IISConfigToolForm : Form
	{

		private IISConfigManager IISManager;

		public List<WebSite> WebSites { get; set; }

		public IISConfigToolForm()
		{
			InitializeComponent();

			WebSites = new List<WebSite>();
		}

		private void IISConfigToolForm_Load(object sender, EventArgs e)
		{
			if (IISConfigManager.IISVersion == 6)
			{
				IISManager = new IIS6Manager();
			}
			else if (IISConfigManager.IISVersion == 7)
			{
				IISManager = new IIS7Manager();
			}
			else
			{
				MessageBox.Show("不支持当前IIS版本：" + IISConfigManager.IISVersion.ToString());

				Application.Exit();
			}

			if (!IISManager.LoadConfig())
			{
				Application.Exit();
			}

			//IISConfigManager.LoadWebSites();
			WebSites = IISManager.WebSites;

			dataGridView_WebList.Visible = false;
			this.dataGridView_WebList.DataSource = WebSites;
			dataGridView_WebList.Visible = true;

		}

		private void Reset()
		{
			WebSites.ForEach((e) => e.IsSelected = false);
			textBox_IPAllowList.Clear();
		}

		private void button_AddIPAllow_Click(object sender, EventArgs e)
		{
			Loger.Debug("Allow" + textBox_IPAllowList.Text);

			try
			{
				IISManager.ClearBuffer();

				var ipaddrs = textBox_IPAllowList.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

				List<string> ipAllowList = new List<string>();

				if (WebSites.Count(item => item.IsSelected == true) == 0)
				{
					MessageBox.Show("请选择一个站点");
					return;
				}

				//if (ipaddrs.Count() == 0)
				//{
				//	MessageBox.Show("请输入ip地址");
				//	return;
				//}

				try
				{
					foreach (var ipaddr in ipaddrs)
					{
						if (ipaddr.Contains("-"))
						{
							var helper = new SubnetMaskHelper(ipaddr);
						}
						else
						{
							if (!SubnetMaskHelper.IP.IsMatch(ipaddr))
							{
								throw new Exception("输入格式不正确");
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("错误：" + Environment.NewLine + ex.Message);

					return;
				}

				foreach (var web in WebSites.Where(web => web.IsSelected == true))
				{
					ipAllowList = new List<string>();

					foreach (var ipaddr in ipaddrs)
					{
						if (ipaddr.Contains("-"))
						{
							var helper = new SubnetMaskHelper(ipaddr);

							var iplist = helper.GetResult();

							foreach (var ipset in iplist)
							{
								ipAllowList.Add(ipset.IP + ", " + ipset.Mask);
							}
						}
						else
						{
							ipAllowList.Add(ipaddr + ", 255.255.255.255");
						}

					}

					IISManager.AddAllowIP(web, ipAllowList);
				}

				MessageBox.Show("设置完毕" + Environment.NewLine + IISManager.GetBuffer());

				Loger.Info(IISManager.GetBuffer());

			}
			catch (Exception ex)
			{
				MessageBox.Show("设置异常" + Environment.NewLine + ex.Message);

				Loger.Error("Allow" + textBox_IPAllowList.Text, ex);
			}
		}

		private void button_AddIPDeny_Click(object sender, EventArgs e)
		{
			Loger.Debug("Deny" + textBox_IPAllowList.Text);

			try
			{
				IISManager.ClearBuffer();

				var ipaddrs = textBox_IPDenyList.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

				List<string> ipDenyList = new List<string>();

				if (WebSites.Count(item => item.IsSelected == true) == 0)
				{
					MessageBox.Show("请选择一个站点");
					return;
				}

				//if (ipaddrs.Count() == 0)
				//{
				//	MessageBox.Show("请输入ip地址");
				//	return;
				//}

				try
				{
					foreach (var ipaddr in ipaddrs)
					{
						if (ipaddr.Contains("-"))
						{
							var helper = new SubnetMaskHelper(ipaddr);
						}
						else
						{
							if (!SubnetMaskHelper.IP.IsMatch(ipaddr))
							{
								throw new Exception("输入格式不正确");
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("错误：" + Environment.NewLine + ex.Message);

					return;
				}

				foreach (var web in WebSites.Where(web => web.IsSelected == true))
				{
					ipDenyList = new List<string>();

					foreach (var ipaddr in ipaddrs)
					{
						if (ipaddr.Contains("-"))
						{
							var helper = new SubnetMaskHelper(ipaddr);

							var iplist = helper.GetResult();

							foreach (var ipset in iplist)
							{
								ipDenyList.Add(ipset.IP + ", " + ipset.Mask);
							}
						}
						else
						{
							ipDenyList.Add(ipaddr + ", 255.255.255.255");
						}
					}

					IISManager.AddDenyIP(web, ipDenyList);
				}

				MessageBox.Show("设置完毕" + Environment.NewLine + IISManager.GetBuffer());

				Loger.Info(IISManager.GetBuffer());
			}
			catch (Exception ex)
			{
				MessageBox.Show("设置异常" + Environment.NewLine + ex.Message);

				Loger.Error("Deny" + textBox_IPAllowList.Text, ex);
			}
		}

		private void dataGridView_WebList_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridView_WebList.Visible == false)
			{
				return;
			}

			if (dataGridView_WebList.SelectedRows.Count == 1)
			{
				WebSites.ForEach(item => item.IsSelected = false);

				var index = dataGridView_WebList.SelectedRows[0].Index;
				var website = WebSites[index];

				website.IsSelected = true;

				website.IPAllowList = IISManager.GetAllowIP(website);
				website.IPDenyList = IISManager.GetDenyIP(website);
				website.AllowByDefault = IISManager.GetAllowByDefault(website);

				textBox_IPAllowList.Text = string.Join(Environment.NewLine, website.IPAllowList);
				textBox_IPDenyList.Text = string.Join(Environment.NewLine, website.IPDenyList);
				checkBox_AllowByDefault.Checked = website.AllowByDefault;
			}

		}

		private void button_AllowByDefault_Click(object sender, EventArgs e)
		{
			try
			{
				if (WebSites.Count(item => item.IsSelected == true) == 0)
				{
					MessageBox.Show("请选择一个站点");
					return;
				}

				foreach (var web in WebSites.Where(web => web.IsSelected == true))
				{
					IISManager.SetAllowByDefault(web, checkBox_AllowByDefault.Checked);
				}

				MessageBox.Show("设置完毕");
			}
			catch (Exception ex)
			{
				MessageBox.Show("设置异常" + Environment.NewLine + ex.Message);

				Loger.Error("AllowByDefault:" + checkBox_AllowByDefault.Checked.ToString(), ex);
			}
		}


	}
}
