namespace SY_Dexinjiaoyu
{
    partial class ImportHotelCSV
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cancelButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.openFileBtton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.closeButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.progressMsgLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Text Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
            this.openFileDialog1.Title = "请选择酒店Excel数据文件";
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(275, 187);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(187, 187);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 25);
            this.importButton.TabIndex = 12;
            this.importButton.Text = "导入";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "文件路径";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(86, 70);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(302, 20);
            this.pathTextBox.TabIndex = 10;
            // 
            // openFileBtton
            // 
            this.openFileBtton.Location = new System.Drawing.Point(394, 68);
            this.openFileBtton.Name = "openFileBtton";
            this.openFileBtton.Size = new System.Drawing.Size(44, 25);
            this.openFileBtton.TabIndex = 9;
            this.openFileBtton.Text = "选择";
            this.openFileBtton.UseVisualStyleBackColor = true;
            this.openFileBtton.Click += new System.EventHandler(this.openFileBtton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(363, 187);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 25);
            this.closeButton.TabIndex = 17;
            this.closeButton.Text = "关闭";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(458, 25);
            this.label2.TabIndex = 16;
            this.label2.Text = "导入Excel数据";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressMsgLabel
            // 
            this.progressMsgLabel.AutoSize = true;
            this.progressMsgLabel.Location = new System.Drawing.Point(29, 108);
            this.progressMsgLabel.Name = "progressMsgLabel";
            this.progressMsgLabel.Size = new System.Drawing.Size(24, 13);
            this.progressMsgLabel.TabIndex = 15;
            this.progressMsgLabel.Text = "0/0";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(29, 127);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(409, 25);
            this.progressBar1.TabIndex = 14;
            // 
            // ImportHotelCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 249);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.openFileBtton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressMsgLabel);
            this.Controls.Add(this.progressBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportHotelCSV";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Excel数据导入";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button openFileBtton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label progressMsgLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}