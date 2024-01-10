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
            components = new System.ComponentModel.Container();
            locLayout = new DevExpress.XtraLayout.LayoutControl();
            grdList = new GridControlEx();
            bdsList = new BindingSource(components);
            grvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)locLayout).BeginInit();
            locLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bdsList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grvList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            SuspendLayout();
            // 
            // locLayout
            // 
            locLayout.Controls.Add(grdList);
            locLayout.Dock = DockStyle.Fill;
            locLayout.Location = new Point(0, 0);
            locLayout.Name = "locLayout";
            locLayout.OptionsView.UseSkinIndents = false;
            locLayout.Root = Root;
            locLayout.Size = new Size(1200, 600);
            locLayout.TabIndex = 2;
            locLayout.Text = "layoutControl1";
            // 
            // grdList
            // 
            grdList.DataSource = bdsList;
            grdList.Location = new Point(5, 4);
            grdList.MainView = grvList;
            grdList.Name = "grdList";
            grdList.Size = new Size(1190, 592);
            grdList.TabIndex = 0;
            grdList.Tag = "";
            grdList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grvList });
            // 
            // grvList
            // 
            grvList.DetailHeight = 300;
            grvList.GridControl = grdList;
            grvList.Name = "grvList";
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1 });
            Root.Name = "Root";
            Root.Size = new Size(1200, 600);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = grdList;
            layoutControlItem1.Location = new Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(1200, 600);
            layoutControlItem1.TextSize = new Size(0, 0);
            layoutControlItem1.TextVisible = false;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(0, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(0, 0);
            btnClose.TabIndex = 3;
            btnClose.Text = " ";
            btnClose.Click += btnClose_Click;
            // 
            // GridForm
            // 
            AutoScaleDimensions = new SizeF(7F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 600);
            Controls.Add(btnClose);
            Controls.Add(locLayout);
            DoubleBuffered = true;
            Font = new Font("Gulim", 9F);
            Name = "GridForm";
            Text = "GridForm";
            ((System.ComponentModel.ISupportInitialize)locLayout).EndInit();
            locLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdList).EndInit();
            ((System.ComponentModel.ISupportInitialize)bdsList).EndInit();
            ((System.ComponentModel.ISupportInitialize)grvList).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ResumeLayout(false);
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