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
        private Dictionary<Person, List<String>> postures = new Dictionary<Person, List<String>>();
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
                MessageBox.Show("Error: No se detectaron personas");
            }
            
        }

        private void comboPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person selectedPerson = (Person)comboPersons.SelectedItem;
            string selectedType = comboPosture.GetItemText(comboPosture.SelectedItem);

            bool hasPosture = false;
            foreach (String postureName in postures[selectedPerson])
            {
                comboPosture.Items.Clear();
                comboPosture.Items.Add(postureName);

                if (!hasPosture)
                {
                    comboPosture.SelectedItem = postureName;
                }
                hasPosture = true;
            }

            Console.Write(selectedType);
            updateChart(selectedPerson, selectedType, Scene.Instance.Persons);
        }

        private void comboPosture_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person selectedPerson = (Person)comboPersons.SelectedItem;
            string selectedType = comboPosture.GetItemText(comboPosture.SelectedItem);
            Console.Write(selectedType);
            updateChart(selectedPerson, selectedType, Scene.Instance.Persons);
        }

        public void updateChart(Person person, string postureType, List<Person> persons)
        {
            if (persons.Exists(p => p == person))
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
                ProgressChart.Series[0].Points.Clear();
                List<MicroPosture> mpostures = persons.Find(p => p == person).MicroPostures;
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
            List<Person> personsInCombo = new List<Person>();
            bool hasPerson = false;
            foreach (Person person in persons)
            {
                if (person.MicroPostures != null)
                {
                    postures[person] = new List<string>();
                    foreach (MicroPosture mposture in person.MicroPostures)
                    {
                        if (mposture.GestureType == GestureType.Continuous && !postures[person].Contains(mposture.PostureType.Name) && mposture.PostureType.Name != "")
                        {
                            postures[person].Add(mposture.PostureType.Name);

                            if (!personsInCombo.Contains(person))
                            {
                                comboPersons.Items.Add(person);
                                if (!hasPerson) //First Person
                                {
                                    //comboPersons.SelectedItem = person;
                                }
                                hasPerson = true;
                            }
                        }
                    }
                }
            }
        }

    }
}
