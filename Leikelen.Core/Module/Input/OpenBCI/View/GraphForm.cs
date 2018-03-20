using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace cl.uv.leikelen.Module.Input.OpenBCI.View
{
    public partial class GraphForm : UserControl
    {
        public GraphForm()
        {
            InitializeComponent();

            radioButton4.Checked = true;
            radioButton5.Checked = true;
            radioButton8.Checked = true;

            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;
        }

        struct DaneSerialPort
        {
            public byte[] zmienna;
        };

        public Queue<double[]> driver = new Queue<double[]>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (driver.Count > 0)
            {
                double[] data = driver.Dequeue();
                data = ValueOrZero(data);
                drawPlot(data);
            }

        }

        private void drawPlot(double[] dane)
        {
            chart1.Series[0].Points.Add(dane[0]);
            chart2.Series[0].Points.Add(dane[1]);
            chart3.Series[0].Points.Add(dane[2]);
            chart4.Series[0].Points.Add(dane[3]);
            chart5.Series[0].Points.Add(dane[4]);
            chart6.Series[0].Points.Add(dane[5]);
            chart7.Series[0].Points.Add(dane[6]);
            chart8.Series[0].Points.Add(dane[7]);
            chart9.Series[0].Points.Add(dane[8]);

            while (chart1.Series[0].Points.Count > 1250)
            {
                chart1.Series[0].Points.SuspendUpdates();
                chart1.Series[0].Points.Remove(chart1.Series[0].Points.First());
                chart1.Series[0].Points.ResumeUpdates();

                chart2.Series[0].Points.SuspendUpdates();
                chart2.Series[0].Points.Remove(chart2.Series[0].Points.First());
                chart2.Series[0].Points.ResumeUpdates();

                chart3.Series[0].Points.SuspendUpdates();
                chart3.Series[0].Points.Remove(chart3.Series[0].Points.First());
                chart3.Series[0].Points.ResumeUpdates();

                chart4.Series[0].Points.SuspendUpdates();
                chart4.Series[0].Points.Remove(chart4.Series[0].Points.First());
                chart4.Series[0].Points.ResumeUpdates();

                chart5.Series[0].Points.SuspendUpdates();
                chart5.Series[0].Points.Remove(chart5.Series[0].Points.First());
                chart5.Series[0].Points.ResumeUpdates();

                chart6.Series[0].Points.SuspendUpdates();
                chart6.Series[0].Points.Remove(chart6.Series[0].Points.First());
                chart6.Series[0].Points.ResumeUpdates();

                chart7.Series[0].Points.SuspendUpdates();
                chart7.Series[0].Points.Remove(chart7.Series[0].Points.First());
                chart7.Series[0].Points.ResumeUpdates();

                chart8.Series[0].Points.SuspendUpdates();
                chart8.Series[0].Points.Remove(chart8.Series[0].Points.First());
                chart8.Series[0].Points.ResumeUpdates();

                chart9.Series[0].Points.SuspendUpdates();
                chart9.Series[0].Points.Remove(chart9.Series[0].Points.First());
                chart9.Series[0].Points.ResumeUpdates();
            }
        }

        private double[] ValueOrZero(double[] dane)
        {
            if (!checkBox1.Checked)
            {
                dane[1] = 0;
            }
            if (!checkBox2.Checked)
            {
                dane[2] = 0;
            }
            if (!checkBox3.Checked)
            {
                dane[3] = 0;
            }
            if (!checkBox4.Checked)
            {
                dane[4] = 0;
            }
            if (!checkBox5.Checked)
            {
                dane[5] = 0;
            }
            if (!checkBox6.Checked)
            {
                dane[6] = 0;
            }
            if (!checkBox7.Checked)
            {
                dane[7] = 0;
            }
            if (!checkBox8.Checked)
            {
                dane[8] = 0;
            }
            return dane;
        }

        private void turnOFF_transmision()
        {
            char[] buff = new char[1];
            buff[0] = 's';
            serialPort1.Write(buff, 0, 1);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //-1V 1V
            chart2.ChartAreas[0].AxisY.Maximum = 1000000;
            chart2.ChartAreas[0].AxisY.Minimum = -1000000;

            chart3.ChartAreas[0].AxisY.Maximum = 1000000;
            chart3.ChartAreas[0].AxisY.Minimum = -1000000;

            chart4.ChartAreas[0].AxisY.Maximum = 1000000;
            chart4.ChartAreas[0].AxisY.Minimum = -1000000;

            chart5.ChartAreas[0].AxisY.Maximum = 1000000;
            chart5.ChartAreas[0].AxisY.Minimum = -1000000;

            chart6.ChartAreas[0].AxisY.Maximum = 1000000;
            chart6.ChartAreas[0].AxisY.Minimum = -1000000;

            chart7.ChartAreas[0].AxisY.Maximum = 1000000;
            chart7.ChartAreas[0].AxisY.Minimum = -1000000;

            chart8.ChartAreas[0].AxisY.Maximum = 1000000;
            chart8.ChartAreas[0].AxisY.Minimum = -1000000;

            chart9.ChartAreas[0].AxisY.Maximum = 1000000;
            chart9.ChartAreas[0].AxisY.Minimum = -1000000;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //-100mV 100mV
            chart2.ChartAreas[0].AxisY.Maximum = 100000;
            chart2.ChartAreas[0].AxisY.Minimum = -100000;

            chart3.ChartAreas[0].AxisY.Maximum = 100000;
            chart3.ChartAreas[0].AxisY.Minimum = -100000;

            chart4.ChartAreas[0].AxisY.Maximum = 100000;
            chart4.ChartAreas[0].AxisY.Minimum = -100000;

            chart5.ChartAreas[0].AxisY.Maximum = 100000;
            chart5.ChartAreas[0].AxisY.Minimum = -100000;

            chart6.ChartAreas[0].AxisY.Maximum = 100000;
            chart6.ChartAreas[0].AxisY.Minimum = -100000;

            chart7.ChartAreas[0].AxisY.Maximum = 100000;
            chart7.ChartAreas[0].AxisY.Minimum = -100000;

            chart8.ChartAreas[0].AxisY.Maximum = 100000;
            chart8.ChartAreas[0].AxisY.Minimum = -100000;

            chart9.ChartAreas[0].AxisY.Maximum = 100000;
            chart9.ChartAreas[0].AxisY.Minimum = -100000;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //-10mv 10mv
            chart2.ChartAreas[0].AxisY.Maximum = 10000;
            chart2.ChartAreas[0].AxisY.Minimum = -10000;

            chart3.ChartAreas[0].AxisY.Maximum = 10000;
            chart3.ChartAreas[0].AxisY.Minimum = -10000;

            chart4.ChartAreas[0].AxisY.Maximum = 10000;
            chart4.ChartAreas[0].AxisY.Minimum = -10000;

            chart5.ChartAreas[0].AxisY.Maximum = 10000;
            chart5.ChartAreas[0].AxisY.Minimum = -10000;

            chart6.ChartAreas[0].AxisY.Maximum = 10000;
            chart6.ChartAreas[0].AxisY.Minimum = -10000;

            chart7.ChartAreas[0].AxisY.Maximum = 10000;
            chart7.ChartAreas[0].AxisY.Minimum = -10000;

            chart8.ChartAreas[0].AxisY.Maximum = 10000;
            chart8.ChartAreas[0].AxisY.Minimum = -10000;

            chart9.ChartAreas[0].AxisY.Maximum = 10000;
            chart9.ChartAreas[0].AxisY.Minimum = -10000;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //-1mV
            chart2.ChartAreas[0].AxisY.Maximum = 1000;
            chart2.ChartAreas[0].AxisY.Minimum = -1000;

            chart3.ChartAreas[0].AxisY.Maximum = 1000;
            chart3.ChartAreas[0].AxisY.Minimum = -1000;

            chart4.ChartAreas[0].AxisY.Maximum = 1000;
            chart4.ChartAreas[0].AxisY.Minimum = -1000;

            chart5.ChartAreas[0].AxisY.Maximum = 1000;
            chart5.ChartAreas[0].AxisY.Minimum = -1000;

            chart6.ChartAreas[0].AxisY.Maximum = 1000;
            chart6.ChartAreas[0].AxisY.Minimum = -1000;

            chart7.ChartAreas[0].AxisY.Maximum = 1000;
            chart7.ChartAreas[0].AxisY.Minimum = -1000;

            chart8.ChartAreas[0].AxisY.Maximum = 1000;
            chart8.ChartAreas[0].AxisY.Minimum = -1000;

            chart9.ChartAreas[0].AxisY.Maximum = 1000;
            chart9.ChartAreas[0].AxisY.Minimum = -1000;
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            //-100uV
            chart2.ChartAreas[0].AxisY.Maximum = 100;
            chart2.ChartAreas[0].AxisY.Minimum = -100;

            chart3.ChartAreas[0].AxisY.Maximum = 100;
            chart3.ChartAreas[0].AxisY.Minimum = -100;

            chart4.ChartAreas[0].AxisY.Maximum = 100;
            chart4.ChartAreas[0].AxisY.Minimum = -100;

            chart5.ChartAreas[0].AxisY.Maximum = 100;
            chart5.ChartAreas[0].AxisY.Minimum = -100;

            chart6.ChartAreas[0].AxisY.Maximum = 100;
            chart6.ChartAreas[0].AxisY.Minimum = -100;

            chart7.ChartAreas[0].AxisY.Maximum = 100;
            chart7.ChartAreas[0].AxisY.Minimum = -100;

            chart8.ChartAreas[0].AxisY.Maximum = 100;
            chart8.ChartAreas[0].AxisY.Minimum = -100;

            chart9.ChartAreas[0].AxisY.Maximum = 100;
            chart9.ChartAreas[0].AxisY.Minimum = -100;
        }

        private void chart9_Click(object sender, EventArgs e)
        {

        }
    }
}
