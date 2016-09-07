namespace cl.uv.multimodalvisualizer.windows
{
    partial class ContinuousChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ProgressChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboPersons = new System.Windows.Forms.ComboBox();
            this.PostureCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressChart)).BeginInit();
            this.SuspendLayout();
            // 
            // ProgressChart
            // 
            chartArea2.Name = "ChartArea1";
            this.ProgressChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ProgressChart.Legends.Add(legend2);
            this.ProgressChart.Location = new System.Drawing.Point(12, 70);
            this.ProgressChart.Name = "ProgressChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ProgressChart.Series.Add(series2);
            this.ProgressChart.Size = new System.Drawing.Size(719, 330);
            this.ProgressChart.TabIndex = 0;
            this.ProgressChart.Text = "chart1";
            // 
            // comboPersons
            // 
            this.comboPersons.FormattingEnabled = true;
            this.comboPersons.Location = new System.Drawing.Point(13, 13);
            this.comboPersons.Name = "comboPersons";
            this.comboPersons.Size = new System.Drawing.Size(181, 21);
            this.comboPersons.TabIndex = 1;
            this.comboPersons.SelectedIndexChanged += new System.EventHandler(this.comboPersons_SelectedIndexChanged);
            // 
            // PostureCombo
            // 
            this.PostureCombo.FormattingEnabled = true;
            this.PostureCombo.Location = new System.Drawing.Point(244, 13);
            this.PostureCombo.Name = "PostureCombo";
            this.PostureCombo.Size = new System.Drawing.Size(224, 21);
            this.PostureCombo.TabIndex = 2;
            this.PostureCombo.SelectedIndexChanged += new System.EventHandler(this.PostureCombo_SelectedIndexChanged);
            // 
            // ContinuousChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 412);
            this.Controls.Add(this.PostureCombo);
            this.Controls.Add(this.comboPersons);
            this.Controls.Add(this.ProgressChart);
            this.Name = "ContinuousChart";
            this.Text = "ContinuousChart";
            ((System.ComponentModel.ISupportInitialize)(this.ProgressChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ProgressChart;
        private System.Windows.Forms.ComboBox comboPersons;
        private System.Windows.Forms.ComboBox PostureCombo;
    }
}