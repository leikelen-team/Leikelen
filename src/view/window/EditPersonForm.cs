using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cl.uv.leikelen.src.model;
using cl.uv.leikelen.src.interfaces;

namespace cl.uv.leikelen.src.view.window
{
    partial class EditPersonForm : Form
    {
        private Person person;

        public EditPersonForm(Person person)
        {
            InitializeComponent();
            this.person = person;
            this.nameTextBox.Text = person.Name;
            this.maleRadio.Checked = person.Gender == GenderEnum.Male ? true : false;
            this.femaleRadio.Checked = person.Gender == GenderEnum.Female ? true : false;
            this.ageTextBox.Text = person.Age.ToString();
        }

        //public EditPersonForm(int personIndex, ref System.Windows.Controls.Label label_sujeto)
        //{
        //    InitializeComponent();
        //    this.label_sujeto = label_sujeto;

        //    this.person = Scene.Instance.Persons[personIndex];
        //    this.nameTextBox.Text = person.Name;
        //    this.maleRadio.Checked = person.Gender == Person.GenderEnum.Male ? true : false;
        //    this.femaleRadio.Checked = person.Gender == Person.GenderEnum.Female ? true : false;
        //    this.ageTextBox.Text = person.Age.ToString();
        //}

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.person.Name = this.nameTextBox.Text;
            this.person.Gender = this.maleRadio.Checked ? GenderEnum.Male : GenderEnum.Female;
            this.person.Age = Int32.Parse(this.ageTextBox.Text);

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
