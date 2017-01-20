namespace IISConfigTool
{
	partial class IISConfigToolForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.dataGridView_WebList = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel_Buttom = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox_IPDenyList = new System.Windows.Forms.GroupBox();
			this.textBox_IPDenyList = new System.Windows.Forms.TextBox();
			this.groupBox_IPAllowList = new System.Windows.Forms.GroupBox();
			this.textBox_IPAllowList = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel_Button = new System.Windows.Forms.TableLayoutPanel();
			this.button_AddIPDeny = new System.Windows.Forms.Button();
			this.button_AddIPAllow = new System.Windows.Forms.Button();
			this.label_Note = new System.Windows.Forms.Label();
			this.panel_AllowByDefault = new System.Windows.Forms.Panel();
			this.button_AllowByDefault = new System.Windows.Forms.Button();
			this.checkBox_AllowByDefault = new System.Windows.Forms.CheckBox();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.portDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dirDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.webSiteBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView_WebList)).BeginInit();
			this.tableLayoutPanel_Main.SuspendLayout();
			this.tableLayoutPanel_Buttom.SuspendLayout();
			this.groupBox_IPDenyList.SuspendLayout();
			this.groupBox_IPAllowList.SuspendLayout();
			this.tableLayoutPanel_Button.SuspendLayout();
			this.panel_AllowByDefault.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.webSiteBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView_WebList
			// 
			this.dataGridView_WebList.AllowUserToAddRows = false;
			this.dataGridView_WebList.AllowUserToDeleteRows = false;
			this.dataGridView_WebList.AllowUserToResizeRows = false;
			this.dataGridView_WebList.AutoGenerateColumns = false;
			this.dataGridView_WebList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView_WebList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.portDataGridViewTextBoxColumn,
            this.dirDataGridViewTextBoxColumn});
			this.dataGridView_WebList.DataSource = this.webSiteBindingSource;
			this.dataGridView_WebList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView_WebList.Location = new System.Drawing.Point(4, 4);
			this.dataGridView_WebList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.dataGridView_WebList.MultiSelect = false;
			this.dataGridView_WebList.Name = "dataGridView_WebList";
			this.dataGridView_WebList.RowHeadersVisible = false;
			this.dataGridView_WebList.RowTemplate.Height = 23;
			this.dataGridView_WebList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView_WebList.Size = new System.Drawing.Size(1037, 366);
			this.dataGridView_WebList.TabIndex = 0;
			this.dataGridView_WebList.SelectionChanged += new System.EventHandler(this.dataGridView_WebList_SelectionChanged);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "编号";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// tableLayoutPanel_Main
			// 
			this.tableLayoutPanel_Main.ColumnCount = 1;
			this.tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Main.Controls.Add(this.dataGridView_WebList, 0, 0);
			this.tableLayoutPanel_Main.Controls.Add(this.tableLayoutPanel_Buttom, 0, 1);
			this.tableLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel_Main.Name = "tableLayoutPanel_Main";
			this.tableLayoutPanel_Main.RowCount = 2;
			this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Main.Size = new System.Drawing.Size(1045, 749);
			this.tableLayoutPanel_Main.TabIndex = 0;
			// 
			// tableLayoutPanel_Buttom
			// 
			this.tableLayoutPanel_Buttom.ColumnCount = 3;
			this.tableLayoutPanel_Buttom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.64394F));
			this.tableLayoutPanel_Buttom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.35606F));
			this.tableLayoutPanel_Buttom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 387F));
			this.tableLayoutPanel_Buttom.Controls.Add(this.groupBox_IPDenyList, 1, 0);
			this.tableLayoutPanel_Buttom.Controls.Add(this.groupBox_IPAllowList, 0, 0);
			this.tableLayoutPanel_Buttom.Controls.Add(this.tableLayoutPanel_Button, 2, 0);
			this.tableLayoutPanel_Buttom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel_Buttom.Location = new System.Drawing.Point(4, 378);
			this.tableLayoutPanel_Buttom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel_Buttom.Name = "tableLayoutPanel_Buttom";
			this.tableLayoutPanel_Buttom.RowCount = 1;
			this.tableLayoutPanel_Buttom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Buttom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 367F));
			this.tableLayoutPanel_Buttom.Size = new System.Drawing.Size(1037, 367);
			this.tableLayoutPanel_Buttom.TabIndex = 1;
			// 
			// groupBox_IPDenyList
			// 
			this.groupBox_IPDenyList.Controls.Add(this.textBox_IPDenyList);
			this.groupBox_IPDenyList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox_IPDenyList.Location = new System.Drawing.Point(300, 4);
			this.groupBox_IPDenyList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox_IPDenyList.Name = "groupBox_IPDenyList";
			this.groupBox_IPDenyList.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox_IPDenyList.Size = new System.Drawing.Size(345, 359);
			this.groupBox_IPDenyList.TabIndex = 3;
			this.groupBox_IPDenyList.TabStop = false;
			this.groupBox_IPDenyList.Text = "阻止IP列表";
			// 
			// textBox_IPDenyList
			// 
			this.textBox_IPDenyList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox_IPDenyList.Location = new System.Drawing.Point(4, 23);
			this.textBox_IPDenyList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox_IPDenyList.Multiline = true;
			this.textBox_IPDenyList.Name = "textBox_IPDenyList";
			this.textBox_IPDenyList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox_IPDenyList.Size = new System.Drawing.Size(337, 332);
			this.textBox_IPDenyList.TabIndex = 0;
			// 
			// groupBox_IPAllowList
			// 
			this.groupBox_IPAllowList.Controls.Add(this.textBox_IPAllowList);
			this.groupBox_IPAllowList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox_IPAllowList.Location = new System.Drawing.Point(4, 4);
			this.groupBox_IPAllowList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox_IPAllowList.Name = "groupBox_IPAllowList";
			this.groupBox_IPAllowList.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox_IPAllowList.Size = new System.Drawing.Size(288, 359);
			this.groupBox_IPAllowList.TabIndex = 0;
			this.groupBox_IPAllowList.TabStop = false;
			this.groupBox_IPAllowList.Text = "允许IP列表";
			// 
			// textBox_IPAllowList
			// 
			this.textBox_IPAllowList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox_IPAllowList.Location = new System.Drawing.Point(4, 23);
			this.textBox_IPAllowList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox_IPAllowList.Multiline = true;
			this.textBox_IPAllowList.Name = "textBox_IPAllowList";
			this.textBox_IPAllowList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox_IPAllowList.Size = new System.Drawing.Size(280, 332);
			this.textBox_IPAllowList.TabIndex = 0;
			// 
			// tableLayoutPanel_Button
			// 
			this.tableLayoutPanel_Button.ColumnCount = 1;
			this.tableLayoutPanel_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Button.Controls.Add(this.label_Note, 0, 3);
			this.tableLayoutPanel_Button.Controls.Add(this.panel_AllowByDefault, 0, 0);
			this.tableLayoutPanel_Button.Controls.Add(this.button_AddIPDeny, 0, 2);
			this.tableLayoutPanel_Button.Controls.Add(this.button_AddIPAllow, 0, 1);
			this.tableLayoutPanel_Button.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel_Button.Location = new System.Drawing.Point(653, 4);
			this.tableLayoutPanel_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel_Button.Name = "tableLayoutPanel_Button";
			this.tableLayoutPanel_Button.RowCount = 4;
			this.tableLayoutPanel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
			this.tableLayoutPanel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 116F));
			this.tableLayoutPanel_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 168F));
			this.tableLayoutPanel_Button.Size = new System.Drawing.Size(380, 359);
			this.tableLayoutPanel_Button.TabIndex = 2;
			// 
			// button_AddIPDeny
			// 
			this.button_AddIPDeny.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.button_AddIPDeny.ForeColor = System.Drawing.Color.Red;
			this.button_AddIPDeny.Location = new System.Drawing.Point(106, 154);
			this.button_AddIPDeny.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_AddIPDeny.Name = "button_AddIPDeny";
			this.button_AddIPDeny.Size = new System.Drawing.Size(168, 67);
			this.button_AddIPDeny.TabIndex = 4;
			this.button_AddIPDeny.Text = "保存阻止访问IP";
			this.button_AddIPDeny.UseVisualStyleBackColor = true;
			this.button_AddIPDeny.Click += new System.EventHandler(this.button_AddIPDeny_Click);
			// 
			// button_AddIPAllow
			// 
			this.button_AddIPAllow.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.button_AddIPAllow.ForeColor = System.Drawing.Color.Green;
			this.button_AddIPAllow.Location = new System.Drawing.Point(106, 79);
			this.button_AddIPAllow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_AddIPAllow.Name = "button_AddIPAllow";
			this.button_AddIPAllow.Size = new System.Drawing.Size(168, 67);
			this.button_AddIPAllow.TabIndex = 3;
			this.button_AddIPAllow.Text = "保存允许访问IP";
			this.button_AddIPAllow.UseVisualStyleBackColor = true;
			this.button_AddIPAllow.Click += new System.EventHandler(this.button_AddIPAllow_Click);
			// 
			// label_Note
			// 
			this.label_Note.AutoSize = true;
			this.label_Note.Location = new System.Drawing.Point(4, 243);
			this.label_Note.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label_Note.Name = "label_Note";
			this.label_Note.Size = new System.Drawing.Size(304, 32);
			this.label_Note.TabIndex = 5;
			this.label_Note.Text = "注：多个IP用换行隔开，IP段用“-”例：192.168.1.1-255";
			// 
			// panel_AllowByDefault
			// 
			this.panel_AllowByDefault.Controls.Add(this.button_AllowByDefault);
			this.panel_AllowByDefault.Controls.Add(this.checkBox_AllowByDefault);
			this.panel_AllowByDefault.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_AllowByDefault.Location = new System.Drawing.Point(4, 4);
			this.panel_AllowByDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.panel_AllowByDefault.Name = "panel_AllowByDefault";
			this.panel_AllowByDefault.Size = new System.Drawing.Size(372, 67);
			this.panel_AllowByDefault.TabIndex = 6;
			// 
			// button_AllowByDefault
			// 
			this.button_AllowByDefault.Location = new System.Drawing.Point(188, 8);
			this.button_AllowByDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_AllowByDefault.Name = "button_AllowByDefault";
			this.button_AllowByDefault.Size = new System.Drawing.Size(127, 47);
			this.button_AllowByDefault.TabIndex = 1;
			this.button_AllowByDefault.Text = "保存全局设置";
			this.button_AllowByDefault.UseVisualStyleBackColor = true;
			this.button_AllowByDefault.Click += new System.EventHandler(this.button_AllowByDefault_Click);
			// 
			// checkBox_AllowByDefault
			// 
			this.checkBox_AllowByDefault.AutoSize = true;
			this.checkBox_AllowByDefault.Location = new System.Drawing.Point(4, 19);
			this.checkBox_AllowByDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.checkBox_AllowByDefault.Name = "checkBox_AllowByDefault";
			this.checkBox_AllowByDefault.Size = new System.Drawing.Size(155, 20);
			this.checkBox_AllowByDefault.TabIndex = 0;
			this.checkBox_AllowByDefault.Text = "默认全局允许访问";
			this.checkBox_AllowByDefault.UseVisualStyleBackColor = true;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "网站名称";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// portDataGridViewTextBoxColumn
			// 
			this.portDataGridViewTextBoxColumn.DataPropertyName = "Port";
			this.portDataGridViewTextBoxColumn.FillWeight = 120F;
			this.portDataGridViewTextBoxColumn.HeaderText = "端口号";
			this.portDataGridViewTextBoxColumn.Name = "portDataGridViewTextBoxColumn";
			this.portDataGridViewTextBoxColumn.ReadOnly = true;
			this.portDataGridViewTextBoxColumn.Width = 120;
			// 
			// dirDataGridViewTextBoxColumn
			// 
			this.dirDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dirDataGridViewTextBoxColumn.DataPropertyName = "Dir";
			this.dirDataGridViewTextBoxColumn.HeaderText = "物理路径";
			this.dirDataGridViewTextBoxColumn.Name = "dirDataGridViewTextBoxColumn";
			this.dirDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// webSiteBindingSource
			// 
			this.webSiteBindingSource.DataSource = typeof(IISConfigTool.Entity.WebSite);
			// 
			// IISConfigToolForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1045, 749);
			this.Controls.Add(this.tableLayoutPanel_Main);
			this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MinimumSize = new System.Drawing.Size(1061, 787);
			this.Name = "IISConfigToolForm";
			this.Text = "IIS配置工具";
			this.Load += new System.EventHandler(this.IISConfigToolForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView_WebList)).EndInit();
			this.tableLayoutPanel_Main.ResumeLayout(false);
			this.tableLayoutPanel_Buttom.ResumeLayout(false);
			this.groupBox_IPDenyList.ResumeLayout(false);
			this.groupBox_IPDenyList.PerformLayout();
			this.groupBox_IPAllowList.ResumeLayout(false);
			this.groupBox_IPAllowList.PerformLayout();
			this.tableLayoutPanel_Button.ResumeLayout(false);
			this.tableLayoutPanel_Button.PerformLayout();
			this.panel_AllowByDefault.ResumeLayout(false);
			this.panel_AllowByDefault.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.webSiteBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView_WebList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Main;
		private System.Windows.Forms.BindingSource webSiteBindingSource;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Buttom;
		private System.Windows.Forms.GroupBox groupBox_IPAllowList;
		private System.Windows.Forms.TextBox textBox_IPAllowList;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn portDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dirDataGridViewTextBoxColumn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Button;
		private System.Windows.Forms.Button button_AddIPDeny;
		private System.Windows.Forms.Button button_AddIPAllow;
		private System.Windows.Forms.Label label_Note;
		private System.Windows.Forms.GroupBox groupBox_IPDenyList;
		private System.Windows.Forms.TextBox textBox_IPDenyList;
		private System.Windows.Forms.Panel panel_AllowByDefault;
		private System.Windows.Forms.Button button_AllowByDefault;
		private System.Windows.Forms.CheckBox checkBox_AllowByDefault;
	}
}

