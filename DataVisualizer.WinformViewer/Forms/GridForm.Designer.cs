using DataVisualizer.WinformViewer.Controls;

namespace DataVisualizer.WinformViewer.Forms
{
    partial class GridForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.locLayout = new DevExpress.XtraLayout.LayoutControl();
            this.grdList = new DataVisualizer.WinformViewer.Controls.GridControlEx();
            this.bdsList = new System.Windows.Forms.BindingSource(this.components);
            this.grvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.locLayout)).BeginInit();
            this.locLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // locLayout
            // 
            this.locLayout.Controls.Add(this.grdList);
            this.locLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locLayout.Location = new System.Drawing.Point(0, 0);
            this.locLayout.Name = "locLayout";
            this.locLayout.OptionsView.UseSkinIndents = false;
            this.locLayout.Root = this.Root;
            this.locLayout.Size = new System.Drawing.Size(1200, 700);
            this.locLayout.TabIndex = 2;
            this.locLayout.Text = "layoutControl1";
            // 
            // grdList
            // 
            this.grdList.DataSource = this.bdsList;
            this.grdList.Location = new System.Drawing.Point(5, 5);
            this.grdList.MainView = this.grvList;
            this.grdList.Name = "grdList";
            this.grdList.Size = new System.Drawing.Size(1190, 690);
            this.grdList.TabIndex = 0;
            this.grdList.Tag = "";
            this.grdList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvList});
            // 
            // grvList
            // 
            this.grvList.GridControl = this.grdList;
            this.grvList.Name = "grvList";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1200, 700);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.grdList;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1200, 700);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(0, 0);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = " ";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // GridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.locLayout);
            this.DoubleBuffered = true;
            this.Name = "GridForm";
            this.Text = "GridForm";
            ((System.ComponentModel.ISupportInitialize)(this.locLayout)).EndInit();
            this.locLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BindingSource bdsList;
        private DevExpress.XtraLayout.LayoutControl locLayout;
        private DataVisualizer.WinformViewer.Controls.GridControlEx grdList;
        private DevExpress.XtraGrid.Views.Grid.GridView grvList;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}