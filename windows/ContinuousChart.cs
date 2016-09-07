using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cl.uv.multimodalvisualizer.models;
using Microsoft.Kinect.VisualGestureBuilder;

namespace cl.uv.multimodalvisualizer.windows
{
    public partial class ContinuousChart : Form
    {
        private bool hasPersons()
        {
            if (Scene.Instance != null && Scene.Instance.Persons != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public ContinuousChart()
        {
            InitializeComponent();
            if (hasPersons())
            {
                AddPersons(Scene.Instance.Persons);
            }
            else
            {
                MessageBox.Show("Error: No se ha reproducido nada aún");
            }
        }

        private void comboPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPerson = comboPersons.GetItemText(comboPersons.SelectedItem);
            string selectedType = PostureCombo.GetItemText(PostureCombo.SelectedItem);
            Console.Write(selectedType);
            if (hasPersons())
            {
                updateChart(selectedPerson, selectedType, Scene.Instance.Persons);
            }
            else
            {
                MessageBox.Show("Error: No se ha reproducido nada aún");
            }
        }

        private void PostureCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPerson = comboPersons.GetItemText(comboPersons.SelectedItem);
            string selectedType = PostureCombo.GetItemText(PostureCombo.SelectedItem);
            Console.Write(selectedType);
            if (hasPersons())
            {
                updateChart(selectedPerson, selectedType, Scene.Instance.Persons);
            }
            else
            {
                MessageBox.Show("Error: No se ha reproducido nada aún");
            }
        }

        public void updateChart(string personName, string postureType, List<Person> persons)
        {
            if (persons.Exists(p => p.Name == personName))
            {
                ProgressChart.Series.Clear();
                ProgressChart.Series.Add(postureType);
                //ProgressChart.ChartAreas.Add(postureType);
                //ProgressChart.ChartAreas[postureType].AxisX.MajorGrid.LineColor = Color.Blue;
                //ProgressChart.ChartAreas[postureType].AxisX.Name = "Tiempo";
                //ProgressChart.ChartAreas[postureType].AxisY.Name = "Progreso";
                //ProgressChart.ChartAreas[postureType].AxisY.MajorGrid.LineColor = Color.Blue;
                //ProgressChart.ChartAreas[postureType].BackColor = Color.White;
                ProgressChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                List<MicroPosture> mpostures = persons.Find(p => p.Name == personName).MicroPostures;
                foreach(var mc in mpostures)
                {
                    if (mc.GestureType == GestureType.Continuous)
                    {
                        if (mc.PostureType.Name == postureType)
                        {
                            
                            
                            ProgressChart.Series[0].Points.AddXY(mc.SceneLocationTime.TotalMilliseconds, mc.Progress);
                        }
                    }
                }
            }
        }

        public void AddPersons(List<Person> persons)
        {
            bool hasPerson = false;
            List<String> postures = new List<string>();
            foreach (Person person in persons)
            {
                if (person.MicroPostures != null)
                {
                    comboPersons.Items.Add(person.Name);
                    if (!hasPerson) //First Person
                    {
                        comboPersons.SelectedItem = person.Name;
                    }
                    hasPerson = true;

                    foreach(MicroPosture mposture in person.MicroPostures)
                    {
                        if (mposture.GestureType ==GestureType.Continuous && postures.Contains(mposture.PostureType.Name) == false)
                        {
                            postures.Add(mposture.PostureType.Name);
                            PostureCombo.Items.Add(mposture.PostureType.Name);
                        }
                    }
                }
            }
        }

    }
}
