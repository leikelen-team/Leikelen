namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.windows
{
    partial class EditPersonForm
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ageTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.maleRadio = new System.Windows.Forms.RadioButton();
            this.femaleRadio = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre: ";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(68, 6);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(146, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Género:";
            // 
            // ageTextBox
            // 
            this.ageTextBox.Location = new System.Drawing.Point(68, 58);
            this.ageTextBox.Name = "ageTextBox";
            this.ageTextBox.Size = new System.Drawing.Size(133, 20);
            this.ageTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Edad:";
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.saveButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveButton.Location = new System.Drawing.Point(12, 99);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(108, 27);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Guardar cambios";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(126, 99);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 27);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancelar";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // maleRadio
            // 
            this.maleRadio.AutoSize = true;
            this.maleRadio.Location = new System.Drawing.Point(68, 35);
            this.maleRadio.Name = "maleRadio";
            this.maleRadio.Size = new System.Drawing.Size(73, 17);
            this.maleRadio.TabIndex = 8;
            this.maleRadio.TabStop = true;
            this.maleRadio.Text = "Masculino";
            this.maleRadio.UseVisualStyleBackColor = true;
            this.maleRadio.CheckedChanged += new System.EventHandler(this.maleRadio_CheckedChanged);
            // 
            // femaleRadio
            // 
            this.femaleRadio.AutoSize = true;
            this.femaleRadio.Location = new System.Drawing.Point(143, 35);
            this.femaleRadio.Name = "femaleRadio";
            this.femaleRadio.Size = new System.Drawing.Size(71, 17);
            this.femaleRadio.TabIndex = 9;
            this.femaleRadio.TabStop = true;
            this.femaleRadio.Text = "Femanino";
            this.femaleRadio.UseVisualStyleBackColor = true;
            this.femaleRadio.CheckedChanged += new System.EventHandler(this.femaleRadio_CheckedChanged);
            // 
            // EditPersonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 140);
            this.Controls.Add(this.femaleRadio);
            this.Controls.Add(this.maleRadio);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.ageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "EditPersonForm";
            this.Text = "Editar Sujeto";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ageTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton maleRadio;
        private System.Windows.Forms.RadioButton femaleRadio;
    }
}