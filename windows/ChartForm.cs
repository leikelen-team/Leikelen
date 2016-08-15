using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.analytics;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.windows
{
    public partial class ChartForm : Form
    {
        

        public ChartForm(Person person)
        {
            InitializeComponent();
            personName_label1.Text = person.Name;
            personGender_label1.Text = "Genero " + person.Gender.ToString("g");
            personAge_label1.Text = person.Age.ToString() + " años";
            IReadOnlyDictionary<string, string> dict = person.calculateEmotionsAverageString();

            Series ser1 = new Series("Serie 1");
            ser1.ChartType = SeriesChartType.Pie;
            chart1.Series.Add(ser1);
            chart1.Series["Serie 1"].Points.DataBindXY(dict.Keys, dict.Values);
        }

        public ChartForm()
        {
            InitializeComponent();
            
        }


        public void updatePersonalPersonData(Person person)
        {
            //int i = person.BodyIndex + 1;
            //Label nameLabel = this.GetControl("personName_label" + i ) as Label;
            //Label genderLabel = this.GetControl("personGender_label" + i) as Label;
            //Label ageLabel = this.GetControl("personAge_label" + i) as Label;
            //nameLabel.Text = person.Name;
            //genderLabel.Text = "Genero " + person.Gender.ToString("g");
            //ageLabel.Text = person.Age.ToString() + " años";
        }

        public void updateAllPersonalPersonData()
        {
            for (int i = 1; i <= 6; i++)
            {
                Person person = Scene.Instance.Persons[i - 1];
                updatePersonalPersonData(person);
            }
        }

        public void updateCharts()
        {
            for (int i = 1; i <= 6; i++)
            {
                Person person = Scene.Instance.Persons[i-1];
                if ( !person.HasBeenTracked ) continue;
                updatePersonalPersonData(person);

                IReadOnlyDictionary<string, string> posturesAvg = person.calculateEmotionsAverageString();
                //if (posturesAvg == null) continue;

                Chart chart = this.GetControl("chart" + i) as Chart;
                Series serie = new Series("Serie 1");
                serie.ChartType = SeriesChartType.Pie;
                chart.Series.Add(serie);
                chart.Series["Serie 1"].Points.DataBindXY(posturesAvg.Keys, posturesAvg.Values);

                
                
            }
        }
        
    }
}
