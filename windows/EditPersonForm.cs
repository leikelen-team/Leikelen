using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.windows
{
    partial class EditPersonForm : Form
    {
        private Person person;
        private System.Windows.Controls.Label label_sujeto;

        public EditPersonForm()
        {
            InitializeComponent();
        }

        //public EditPersonForm(Person person)
        //{
        //    this.person = person;
        //    this.nameTextBox.Text = person.name;
        //    this.maleRadio.Checked = person.gender == Person.Gender.Masculino ? true : false;
        //    this.femaleRadio.Checked = person.gender == Person.Gender.Femenino ? true : false;
        //    this.ageTextBox.Text = person.age.ToString();
        //}

        public EditPersonForm(int personIndex, ref System.Windows.Controls.Label label_sujeto)
        {
            InitializeComponent();
            this.label_sujeto = label_sujeto;

            this.person = Scene.Instance.Persons[personIndex];
            this.nameTextBox.Text = person.Name;
            this.maleRadio.Checked = person.Gender == Person.GenderEnum.Masculino ? true : false;
            this.femaleRadio.Checked = person.Gender == Person.GenderEnum.Femenino ? true : false;
            this.ageTextBox.Text = person.Age.ToString();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            label_sujeto.Content = this.person.Name = this.nameTextBox.Text;
            this.person.Gender = this.maleRadio.Checked ? Person.GenderEnum.Masculino : Person.GenderEnum.Femenino;
            this.person.Age = Int32.Parse(this.ageTextBox.Text);
            if(MainWindow.chartForm!=null && !MainWindow.chartForm.IsDisposed)
                MainWindow.chartForm.updatePersonalPersonData(this.person);

            Console.WriteLine("new name in Instance: " + Scene.Instance.Persons.FirstOrDefault(p => p.TrackingId == person.TrackingId).Name);

            this.Hide();
            this.Dispose();
        }

        private void maleRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (maleRadio.Checked)
            {
                femaleRadio.Checked = false;
            }
        }

        private void femaleRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (femaleRadio.Checked)
            {
                maleRadio.Checked = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
