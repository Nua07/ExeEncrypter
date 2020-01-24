namespace ExeEncrypter
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.FilePathTextbox = new MetroFramework.Controls.MetroTextBox();
            this.FileSelectButton = new MetroFramework.Controls.MetroButton();
            this.BuildButton = new MetroFramework.Controls.MetroButton();
            this.KeyTextbox = new MetroFramework.Controls.MetroTextBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // FilePathTextbox
            // 
            this.FilePathTextbox.AllowDrop = true;
            this.FilePathTextbox.Location = new System.Drawing.Point(23, 82);
            this.FilePathTextbox.Name = "FilePathTextbox";
            this.FilePathTextbox.ReadOnly = true;
            this.FilePathTextbox.Size = new System.Drawing.Size(354, 23);
            this.FilePathTextbox.TabIndex = 0;
            // 
            // FileSelectButton
            // 
            this.FileSelectButton.Location = new System.Drawing.Point(383, 82);
            this.FileSelectButton.Name = "FileSelectButton";
            this.FileSelectButton.Size = new System.Drawing.Size(76, 23);
            this.FileSelectButton.TabIndex = 1;
            this.FileSelectButton.Text = "파일 선택";
            this.FileSelectButton.Click += new System.EventHandler(this.FileSelectButton_Click);
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(24, 176);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(434, 36);
            this.BuildButton.TabIndex = 2;
            this.BuildButton.Text = "빌드";
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // KeyTextbox
            // 
            this.KeyTextbox.Location = new System.Drawing.Point(24, 111);
            this.KeyTextbox.Name = "KeyTextbox";
            this.KeyTextbox.Size = new System.Drawing.Size(435, 23);
            this.KeyTextbox.TabIndex = 3;
            this.KeyTextbox.Text = "This_Is_Key";
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "exe";
            this.openFile.FileName = "TargetFile";
            this.openFile.Filter = "실행파일|*.exe";
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(24, 140);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(215, 30);
            this.metroButton1.TabIndex = 5;
            this.metroButton1.Text = "ICON";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(481, 228);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.KeyTextbox);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.FileSelectButton);
            this.Controls.Add(this.FilePathTextbox);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Resizable = false;
            this.Text = "ExeEncrypter SAMPLE Nua07";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox FilePathTextbox;
        private MetroFramework.Controls.MetroButton FileSelectButton;
        private MetroFramework.Controls.MetroButton BuildButton;
        private MetroFramework.Controls.MetroTextBox KeyTextbox;
        private System.Windows.Forms.OpenFileDialog openFile;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}

