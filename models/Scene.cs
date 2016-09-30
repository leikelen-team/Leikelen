using Microsoft.EntityFrameworkCore;
using cl.uv.multimodalvisualizer.db;
using cl.uv.multimodalvisualizer.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace cl.uv.multimodalvisualizer.models
{
    public class Scene
    {
        [NotMapped]
        private static Scene instance;

        // poner fecha como key

        //public int SceneId { get; set; }
        [Key]
        public DateTime StartDate { get; set; } // start date when begin to record
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        //public List<SceneFrame> Frames { get; set; }
        public int TickCount { get; set; }
        [NotMapped]
        public Statuses Status { get; private set; }

        public enum Statuses { Imported, Recorded }

        public List<Person> Persons { get; set; }
        //public DbSet<Person> Personss { get; set; }

        [NotMapped]
        public static Scene Instance
        {
            get
            {
                return instance;
            }
            //set
            //{
            //    instance = value;
            //}
        }
        public Scene() { }

        //private Scene(string name, DateTime startDate, TimeSpan duration)
        //{
        //    this.Name = name;
        //    this.StartDate = startDate;
        //    this.Duration = duration;
        //    this.Persons = new List<Person>();
        //    //this.Frames = new List<SceneFrame>();
        //    TickCount = 0;
        //}

        //public static Scene Create(string name, DateTime startDate, TimeSpan duration)
        //{
        //    instance = new Scene(name, startDate, duration);
        //    instance.Status = Statuses.Recorded;
        //    instance.InitTimeLine();

        //    return instance;
        //}

        public static Scene CreateFromRecord(string name)
        {
            if (instance != null) instance.Clear();
            instance = new Scene()
            {
                Name = name,
                StartDate = DateTime.Now,
                Persons = new List<Person>(),
                Status = Statuses.Recorded
            };
            //if (PgsqlContext.IsConnected)
            //{
            //    var db = PgsqlContext.db;
            //    instance.SceneId = db.Scene.Last().SceneId + 1;
            //}
            return instance;
        }

        public static void CreateFromDbContext(string path)
        {
            
            if(instance!=null) instance.Clear();
            instance = BackupDataContext.LoadScene(path);
            instance.Status = Statuses.Imported;
            instance.InitTimeLine();
            var microPostures = BackupDataContext.Load_MicroPostures(path);
            foreach (Person person in Scene.Instance.Persons)
            {
                if (!person.HasBeenTracked) continue;
                if (microPostures.Exists(mp => mp.PersonId == person.PersonId))
                {
                    person.MicroPostures = microPostures.FindAll(mp => mp.PersonId == person.PersonId);
                }
                person.generateView();
                person.View.repaintIntervalGroups();
                MainWindow.Instance().timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
            }
            MainWindow.Instance().enableButtons();

            //Console.WriteLine("oa");
            //instance = db.Scene.First();
        }

        public void InitTimeLine()
        {
            
            ColumnDefinition rulerCol, contentCol;
            TextBlock text;
            
            TimeSpan frameTime = TimeSpan.FromSeconds(0);
            //int currentSeg = 0;
            int colSpan = 10;
            for (int colCount = 0; true; colCount++)
            {
                if (frameTime < Duration)
                {
                    //Scene.Instance.Frames.Add(new SceneFrame(frameTime));
                    Instance.TickCount++;
                    rulerCol = new ColumnDefinition();
                    rulerCol.Width = new GridLength(5, GridUnitType.Pixel);
                    MainWindow.Instance().timeRulerGrid.ColumnDefinitions.Add(rulerCol);

                    contentCol = new ColumnDefinition();
                    contentCol.Width = new GridLength(5, GridUnitType.Pixel);
                    MainWindow.Instance().timeLineContentGrid.ColumnDefinitions.Add(contentCol);

                    if (colCount % colSpan == 0 && colCount != 0)
                    {
                        text = new TextBlock();
                        text.Text = "|";
                        text.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(text, 0);
                        Grid.SetColumn(text, colCount);
                        Grid.SetColumnSpan(text, colSpan);
                        MainWindow.Instance().timeRulerGrid.Children.Add(text);

                        text = new TextBlock();
                        //text.Text = frameTime.TotalSeconds.ToString("N0");
                        text.Text = frameTime.ToShortForm();
                        text.HorizontalAlignment = colCount == 0 ?
                            HorizontalAlignment.Left : HorizontalAlignment.Center;
                        Grid.SetRow(text, 1);
                        int offset = colCount == 0 ? 0 : (colSpan / 2);
                        Grid.SetColumn(text, colCount - offset);
                        Grid.SetColumnSpan(text, colSpan);
                        MainWindow.Instance().timeRulerGrid.Children.Add(text);
                    }
                    else
                    //if (colCount % (colSpan / 2) == 0)
                    {
                        text = new TextBlock();
                        text.Text = "·";
                        text.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(text, 0);
                        Grid.SetColumn(text, colCount);
                        //Grid.SetColumnSpan(text, colSpan/2);
                        MainWindow.Instance().timeRulerGrid.Children.Add(text);
                    }

                    frameTime = frameTime.Add(TimeSpan.FromMilliseconds(1000.00));
                }
                else
                {
                    break;
                }
            }
        }

        internal void Clear()
        {
            if (Scene.Instance != null)
            {
                UIElementCollection childs = MainWindow.Instance().timeRulerGrid.Children;
                for (int i = childs.Count - 1; i >= 0; i--)
                    if (childs[i] is TextBlock)
                        MainWindow.Instance().timeRulerGrid.Children.RemoveAt(i);

                MainWindow.Instance().timeLineContentGrid.Children.Clear();
                MainWindow.Instance().personLabelsGrid.Children.Clear();

                MainWindow.Instance().timeRulerGrid.ColumnDefinitions.Clear();
                MainWindow.Instance().timeLineContentGrid.ColumnDefinitions.Clear();
                MainWindow.Instance().timeLineContentGrid.RowDefinitions.Clear();
                MainWindow.Instance().personLabelsGrid.RowDefinitions.Clear();
                MainWindow.Instance().timeLineVerticalScrollViewSubGrid.RowDefinitions.Clear();
                foreach (var person in Scene.Instance.Persons)
                    if (person.View != null && person.View.postureGroupsGrid != null && person.View.ComboStackPanel != null)
                    {
                        person.View.postureGroupsGrid.Children.Clear();
                        person.View.postureGroupsGrid.ColumnDefinitions.Clear();
                        person.View.postureGroupsGrid.RowDefinitions.Clear();
                        person.View.ComboStackPanel.Children.Clear();
                    }
                Scene.Instance.Persons.Clear();
            }
        }
    }
}
