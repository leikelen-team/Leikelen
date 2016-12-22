namespace cl.uv.leikelen.src.view.window
{
    partial class ManageScene
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
            this.inputName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.inputDateTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.inputType = new System.Windows.Forms.TextBox();
            this.inputDescription = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.inputNumberParticipants = new System.Windows.Forms.NumericUpDown();
            this.inputPlace = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.inputNumberParticipants)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(156, 51);
            this.inputName.Name = "inputName";
            this.inputName.Size = new System.Drawing.Size(200, 20);
            this.inputName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha";
            // 
            // inputDateTime
            // 
            this.inputDateTime.Location = new System.Drawing.Point(156, 89);
            this.inputDateTime.Name = "inputDateTime";
            this.inputDateTime.Size = new System.Drawing.Size(200, 20);
            this.inputDateTime.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Descripcion";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Numero de Participantes";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Tipo de Actividad";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Lugar";
            // 
            // inputType
            // 
            this.inputType.Location = new System.Drawing.Point(156, 207);
            this.inputType.Name = "inputType";
            this.inputType.Size = new System.Drawing.Size(200, 20);
            this.inputType.TabIndex = 8;
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(156, 170);
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.Size = new System.Drawing.Size(200, 20);
            this.inputDescription.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputNumberParticipants
            // 
            this.inputNumberParticipants.Location = new System.Drawing.Point(156, 133);
            this.inputNumberParticipants.Name = "inputNumberParticipants";
            this.inputNumberParticipants.Size = new System.Drawing.Size(59, 20);
            this.inputNumberParticipants.TabIndex = 11;
            // 
            // inputPlace
            // 
            this.inputPlace.Location = new System.Drawing.Point(156, 241);
            this.inputPlace.Name = "inputPlace";
            this.inputPlace.Size = new System.Drawing.Size(200, 20);
            this.inputPlace.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(107, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 25);
            this.label7.TabIndex = 13;
            this.label7.Text = "Manage Scene";
            // 
            // ManageScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 342);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.inputPlace);
            this.Controls.Add(this.inputNumberParticipants);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.inputDescription);
            this.Controls.Add(this.inputType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inputDateTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputName);
            this.Controls.Add(this.label1);
            this.Name = "ManageScene";
            this.Text = "ManageScene";
            this.Load += new System.EventHandler(this.ManageScene_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inputNumberParticipants)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker inputDateTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox inputType;
        private System.Windows.Forms.TextBox inputDescription;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown inputNumberParticipants;
        private System.Windows.Forms.TextBox inputPlace;
        private System.Windows.Forms.Label label7;
    }
}