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

namespace cl.uv.multimodalvisualizer.windows
{
    public partial class ManageScene : Form
    {
        public ManageScene()
        {
            InitializeComponent();
        }

        private void ManageScene_Load(object sender, EventArgs e)
        {
            inputDescription.Text = Scene.Instance.Description;
            inputName.Text = Scene.Instance.Name;
            inputType.Text = Scene.Instance.Type;
            inputPlace.Text = Scene.Instance.Place;
            inputNumberParticipants.Value = Scene.Instance.NumberOfParticipants;
            inputDateTime.Value = Scene.Instance.DateTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string desc = inputDescription.Text;
            string name = inputName.Text;
            string type = inputType.Text;
            string place = inputPlace.Text;
            int participants = (int)inputNumberParticipants.Value;
            DateTime dateTime = inputDateTime.Value;

            var result = MessageBox.Show("Confirm Scene Data", "Are You sure you want to confirm?", MessageBoxButtons.YesNoCancel);

            switch(result) {
                case DialogResult.Yes:
                    Scene.Instance.Name = name;
                    Scene.Instance.Description = desc;
                    Scene.Instance.Type = type;
                    Scene.Instance.Place = place;
                    Scene.Instance.NumberOfParticipants = participants;
                    Scene.Instance.DateTime = dateTime;
                    this.Close();
                    break;
                case DialogResult.No:
                    this.Close();
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }

        }
    }
}
