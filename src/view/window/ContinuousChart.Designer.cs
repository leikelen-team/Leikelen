namespace cl.uv.multimodalvisualizer.src.view.window
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ProgressChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboPersons = new System.Windows.Forms.ComboBox();
            this.comboPosture = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressChart)).BeginInit();
            this.SuspendLayout();
            // 
            // ProgressChart
            // 
            chartArea1.Name = "ChartArea1";
            this.ProgressChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ProgressChart.Legends.Add(legend1);
            this.ProgressChart.Location = new System.Drawing.Point(12, 70);
            this.ProgressChart.Name = "ProgressChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ProgressChart.Series.Add(series1);
            this.ProgressChart.Size = new System.Drawing.Size(719, 330);
            this.ProgressChart.TabIndex = 0;
            this.ProgressChart.Text = "chart1";
            // 
            // comboPersons
            // 
            this.comboPersons.FormattingEnabled = true;
            this.comboPersons.Location = new System.Drawing.Point(13, 13);
            this.comboPersons.Name = "comboPersons";
            this.comboPersons.Size = new System.Drawing.Size(198, 21);
            this.comboPersons.TabIndex = 1;
            this.comboPersons.SelectedIndexChanged += new System.EventHandler(this.comboPersons_SelectedIndexChanged);
            // 
            // comboPosture
            // 
            this.comboPosture.FormattingEnabled = true;
            this.comboPosture.Location = new System.Drawing.Point(244, 13);
            this.comboPosture.Name = "comboPosture";
            this.comboPosture.Size = new System.Drawing.Size(198, 21);
            this.comboPosture.TabIndex = 2;
            this.comboPosture.SelectedIndexChanged += new System.EventHandler(this.comboPosture_SelectedIndexChanged);
            // 
            // ContinuousChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 412);
            this.Controls.Add(this.comboPosture);
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
        private System.Windows.Forms.ComboBox comboPosture;
    }
}