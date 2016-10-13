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
using Microsoft.Kinect;

namespace cl.uv.multimodalvisualizer.windows
{
    public partial class DistanceForm : Form
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

        public DistanceForm()
        {
            InitializeComponent();
            if(hasPersons())
            {
                AddPersons(Scene.Instance.Persons);
                update();
            } else
            {
                MessageBox.Show("Error: No se ha reproducido nada aún");
            }
        }

        private void personComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            update();
        }

        private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            update();
        }

        private void checkBoxInfered_CheckedChanged(object sender, EventArgs e)
        {
            update();
        }

        public void AddPersons(List<Person> persons)
        {
            bool hasPerson = false;
            foreach(Person person in persons)
            {
                if(person.Distances.Count != 0)
                {
                    personComboBox.Items.Add(person.Name);
                    if (!hasPerson) //First Person
                    {
                        personComboBox.SelectedItem = person.Name;
                    }
                    hasPerson = true;
                }
            }
            if(hasPerson)
            {
                TypeComboBox.Items.Add("Total en 3D");
                TypeComboBox.Items.Add("Total en X");
                TypeComboBox.Items.Add("Total en Y");
                TypeComboBox.Items.Add("Total en Z");
                TypeComboBox.SelectedItem = "Total en 3D";
            } else
            {
                string noPersons = "No se detectaron personas";
                TypeComboBox.Items.Add(noPersons);
                personComboBox.SelectedItem = noPersons;
                personComboBox.Items.Add(noPersons);
                TypeComboBox.SelectedItem = noPersons;
            }
        }

        private void updateLabels(string personName, string typeName, List<Person> persons, bool inferred)
        {
            if (persons.Exists(p => p.Name == personName))
            {
                List<Distance> bodyDistance;
                if (inferred)
                {
                    bodyDistance = persons.Find(p => p.Name == personName).Distances.FindAll(d => d.inferredType == DistanceInferred.WithInferred);
                }
                else
                {
                    bodyDistance = persons.Find(p => p.Name == personName).Distances.FindAll(d => d.inferredType == DistanceInferred.WithoutInferred);
                }
                //BodyDistance bodyDistance = persons.Find(p => p.Name == personName).distances;
                DistanceTypes showType;
                switch (typeName)
                {
                    case "Total en 3D":
                        showType = DistanceTypes.Total3D;
                        break;
                    case "Total en X":
                        showType = DistanceTypes.XTotal;
                        break;
                    case "Total en Y":
                        showType = DistanceTypes.YTotal;
                        break;
                    case "Total en Z":
                        showType = DistanceTypes.ZTotal;
                        break;
                    default:
                        showType = DistanceTypes.Total3D;
                        break;
                }
                foreach (Distance distanceObj in bodyDistance.FindAll(bd => bd.DistanceType == showType))
                {
                    int i = (int)distanceObj.jointType;
                    Label jointDistance = this.Controls.Find("label" + i, true)[0] as Label;
                    String distanceString = distanceObj.distance.ToString();
                    distanceString = distanceString + "      ";
                    jointDistance.Text = distanceString.Substring(0, 4);
                }
                
            }
        }

        private void update()
        {
            string selectedPerson = personComboBox.GetItemText(personComboBox.SelectedItem);
            string selectedType = TypeComboBox.GetItemText(TypeComboBox.SelectedItem);
            Console.Write(selectedType);
            if (hasPersons())
            {
                updateLabels(selectedPerson, selectedType, Scene.Instance.Persons, checkBoxInferred.Checked);
            }
            else
            {
                MessageBox.Show("Error: No se ha reproducido nada aún");
            }
        }
    }
}
