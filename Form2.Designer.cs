namespace CTA
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxStationName = new System.Windows.Forms.TextBox();
            this.txtBoxStopName = new System.Windows.Forms.TextBox();
            this.radioADAUpdateYes = new System.Windows.Forms.RadioButton();
            this.radioADAUpdateNo = new System.Windows.Forms.RadioButton();
            this.buttonADAUpdateConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Station:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Stop:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(241, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Is the current selected stop handicap accessible?";
            // 
            // txtBoxStationName
            // 
            this.txtBoxStationName.Location = new System.Drawing.Point(87, 22);
            this.txtBoxStationName.Name = "txtBoxStationName";
            this.txtBoxStationName.ReadOnly = true;
            this.txtBoxStationName.Size = new System.Drawing.Size(454, 20);
            this.txtBoxStationName.TabIndex = 3;
            // 
            // txtBoxStopName
            // 
            this.txtBoxStopName.Location = new System.Drawing.Point(87, 57);
            this.txtBoxStopName.Name = "txtBoxStopName";
            this.txtBoxStopName.ReadOnly = true;
            this.txtBoxStopName.Size = new System.Drawing.Size(454, 20);
            this.txtBoxStopName.TabIndex = 4;
            // 
            // radioADAUpdateYes
            // 
            this.radioADAUpdateYes.AutoSize = true;
            this.radioADAUpdateYes.Location = new System.Drawing.Point(348, 98);
            this.radioADAUpdateYes.Name = "radioADAUpdateYes";
            this.radioADAUpdateYes.Size = new System.Drawing.Size(43, 17);
            this.radioADAUpdateYes.TabIndex = 5;
            this.radioADAUpdateYes.TabStop = true;
            this.radioADAUpdateYes.Text = "Yes";
            this.radioADAUpdateYes.UseVisualStyleBackColor = true;
            this.radioADAUpdateYes.CheckedChanged += new System.EventHandler(this.radioADAUpdateYes_CheckedChanged);
            // 
            // radioADAUpdateNo
            // 
            this.radioADAUpdateNo.AutoSize = true;
            this.radioADAUpdateNo.Location = new System.Drawing.Point(454, 98);
            this.radioADAUpdateNo.Name = "radioADAUpdateNo";
            this.radioADAUpdateNo.Size = new System.Drawing.Size(39, 17);
            this.radioADAUpdateNo.TabIndex = 6;
            this.radioADAUpdateNo.TabStop = true;
            this.radioADAUpdateNo.Text = "No";
            this.radioADAUpdateNo.UseVisualStyleBackColor = true;
            this.radioADAUpdateNo.CheckedChanged += new System.EventHandler(this.radioADAUpdateNo_CheckedChanged);
            // 
            // buttonADAUpdateConfirm
            // 
            this.buttonADAUpdateConfirm.Location = new System.Drawing.Point(466, 138);
            this.buttonADAUpdateConfirm.Name = "buttonADAUpdateConfirm";
            this.buttonADAUpdateConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonADAUpdateConfirm.TabIndex = 7;
            this.buttonADAUpdateConfirm.Text = "Update";
            this.buttonADAUpdateConfirm.UseVisualStyleBackColor = true;
            this.buttonADAUpdateConfirm.Click += new System.EventHandler(this.buttonADAUpdateConfirm_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 182);
            this.Controls.Add(this.buttonADAUpdateConfirm);
            this.Controls.Add(this.radioADAUpdateNo);
            this.Controls.Add(this.radioADAUpdateYes);
            this.Controls.Add(this.txtBoxStopName);
            this.Controls.Add(this.txtBoxStationName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CTA Ridership Analysis";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBoxStationName;
        private System.Windows.Forms.TextBox txtBoxStopName;
        private System.Windows.Forms.RadioButton radioADAUpdateYes;
        private System.Windows.Forms.RadioButton radioADAUpdateNo;
        private System.Windows.Forms.Button buttonADAUpdateConfirm;
    }
}