namespace DataVisualizer.WinformViewer.Forms
{
    partial class MainForm
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
            fbdDialog = new DevExpress.XtraEditors.XtraFolderBrowserDialog(components);
            fswWatcher = new System.IO.FileSystemWatcher();
            xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(components);
            ((System.ComponentModel.ISupportInitialize)fswWatcher).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabbedMdiManager1).BeginInit();
            SuspendLayout();
            // 
            // fbdDialog
            // 
            fbdDialog.SelectedPath = "xtraFolderBrowserDialog1";
            // 
            // fswWatcher
            // 
            fswWatcher.EnableRaisingEvents = true;
            fswWatcher.Filter = "*.json";
            fswWatcher.SynchronizingObject = this;
            fswWatcher.Created += fswWatcher_Created;
            // 
            // xtraTabbedMdiManager1
            // 
            xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            xtraTabbedMdiManager1.CloseTabOnMiddleClick = DevExpress.XtraTabbedMdi.CloseTabOnMiddleClick.OnMouseUp;
            xtraTabbedMdiManager1.MdiParent = this;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1339, 738);
            DoubleBuffered = true;
            IsMdiContainer = true;
            Name = "MainForm";
            Text = "DataVisualizer";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)fswWatcher).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabbedMdiManager1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.XtraFolderBrowserDialog fbdDialog;
        private System.IO.FileSystemWatcher fswWatcher;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
    }
}