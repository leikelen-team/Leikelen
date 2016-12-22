using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cl.uv.multimodalvisualizer.src.model;
using Microsoft.Kinect;

namespace cl.uv.multimodalvisualizer.src.view.window
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
                AxisComboBox.Items.Add("Total en 3D");
                AxisComboBox.Items.Add("Total en X");
                AxisComboBox.Items.Add("Total en Y");
                AxisComboBox.Items.Add("Total en Z");
                AxisComboBox.SelectedItem = "Total en 3D";

                TypeComboBox.Items.Add(DistanceInferred.WithInferred.ToString());
                TypeComboBox.Items.Add(DistanceInferred.WithoutInferred.ToString());
                TypeComboBox.Items.Add(DistanceInferred.OnlyInferred.ToString());
                TypeComboBox.SelectedItem = DistanceInferred.WithInferred.ToString();
            } else
            {
                string noPersons = "No se detectaron personas";
                AxisComboBox.Items.Add(noPersons);
                personComboBox.SelectedItem = noPersons;
                personComboBox.Items.Add(noPersons);
                AxisComboBox.SelectedItem = noPersons;
            }
        }

        private void updateLabels(string personName, string typeName, List<Person> persons, string type)
        {
            if (persons.Exists(p => p.Name == personName))
            {
                List<Distance> bodyDistance;
                DistanceInferred dISearch = new DistanceInferred();
                switch (type)
                {
                    case "WithInferred":
                        dISearch = DistanceInferred.WithInferred;
                        break;
                    case "WithoutInferred":
                        dISearch = DistanceInferred.WithoutInferred;
                        break;
                    case "OnlyInferred":
                        dISearch = DistanceInferred.OnlyInferred;
                        break;
                }
                bodyDistance = persons.Find(p => p.Name == personName).Distances.FindAll(d => d.inferredType == dISearch);
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
            string selectedType = AxisComboBox.GetItemText(AxisComboBox.SelectedItem);
            string selectedTypeReal = TypeComboBox.GetItemText(TypeComboBox.SelectedItem);
            Console.Write(selectedType);
            if (hasPersons())
            {
                updateLabels(selectedPerson, selectedType, Scene.Instance.Persons, selectedTypeReal);
            }
            else
            {
                MessageBox.Show("Error: No se ha reproducido nada aún");
            }
        }

        private void TypeComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            update();
        }

        private void DistanceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
