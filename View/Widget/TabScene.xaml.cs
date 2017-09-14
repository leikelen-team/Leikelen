using cl.uv.leikelen.API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.View.Widget
{
    /// <summary>
    /// Lógica de interacción para TabScene.xaml
    /// </summary>
    public partial class TabScene : TabItem, ITab
    {
        public TabScene()
        {
            InitializeComponent();
        }

        public void Fill()
        {
            List<SceneTabDataGrid> data = new List<SceneTabDataGrid>();
            var persons = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene;
            foreach(var person in persons)
            {
                foreach(var subModal in person.SubModalType_PersonInScenes)
                {
                    SceneTabDataGrid row = new SceneTabDataGrid();
                    row.PersonName = person.Person.Name;
                    row.ModalName = subModal.ModalTypeId;
                    row.SubModalName = subModal.SubModalTypeId;
                    row.TimeTotal = "";
                    row.TimeAverage = "";
                    row.TimeStandardDeviation = "";
                    row.ValueAverage = "";
                    row.ValueStandardDeviation = "";
                    var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);
                    var events = DataAccessFacade.Instance.GetEventAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);
                    var timelesses = DataAccessFacade.Instance.GetTimelessAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);

                    if(intervals != null)
                    {
                        TimeSpan totalTime = new TimeSpan(0);
                        double stdDev = 0;
                        foreach(var interval in intervals)
                        {
                            var time = interval.EndTime.Subtract(interval.StartTime);
                            totalTime = totalTime.Add(time);
                        }
                        row.TimeTotal = totalTime.ToString(@"hh\:mm\:ss");
                        double secsAverage = totalTime.TotalSeconds / intervals.Count;
                        row.TimeAverage = $"{secsAverage} {Properties.GUI.seconds}";
                        foreach (var interval in intervals)
                        {
                            var time = interval.EndTime.Subtract(interval.StartTime);
                            stdDev += Math.Pow(time.TotalSeconds - secsAverage, 2);
                        }
                        row.TimeStandardDeviation = $"{Math.Sqrt(stdDev/ (intervals.Count-1))}";
                    }

                    if(events != null)
                    {
                        double stdDev = 0;
                        TimeSpan totalTime = new TimeSpan(0);
                        double total = 0;
                        foreach (var event_data in events)
                        {
                            totalTime = totalTime.Add(event_data.EventTime);
                            if (event_data.Value.HasValue)
                                total += event_data.Value.Value;
                        }
                        double valueAverage = total / events.Count;
                        foreach (var event_data in events)
                        {
                            if (event_data.Value.HasValue)
                                stdDev += Math.Pow(event_data.Value.Value - valueAverage, 2);
                        }
                        if(total != 0)
                        {
                            row.ValueAverage = $"{valueAverage}";
                            row.ValueStandardDeviation = $"{Math.Sqrt(stdDev / (events.Count - 1))}";
                        }
                    }

                    data.Add(row);
                }
            }

            TabSceneDataGrid.ItemsSource = data;
            TabSceneDataGrid.Items.Refresh();
        }
    }

    public class SceneTabDataGrid
    {
        public string PersonName { get; set; }
        public string ModalName { get; set; }
        public string SubModalName { get; set; }
        public string TimeTotal { get; set; }
        public string TimeAverage { get; set; }
        public string ValueAverage { get; set; }
        public string ValueStandardDeviation { get; set; }
        public string TimeStandardDeviation { get; set; }
    }
}
