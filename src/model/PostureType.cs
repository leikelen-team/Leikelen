using Microsoft.Kinect;
using System;
using System.Collections.Generic;

//using System.Linq;

//using System.Data.Linq.Mapping;
//using System.Data.SQLite.Linq;
//using System.Data.SQLite;

using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using cl.uv.multimodalvisualizer.src.helpers;
using cl.uv.multimodalvisualizer.src.dbcontext;

namespace cl.uv.multimodalvisualizer.src.model
{
    public class PostureType
    {
        [NotMapped]
        public static PostureType none;
        [NotMapped]
        private static List<Tuple<string, Brush>> brushes = null;
        [NotMapped]
        private static int brushIndex = 0;

        [Key]
        public int PostureTypeId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        [NotMapped]
        public string colorName { get; private set; }
        [NotMapped]
        public Brush color { get; private set; }
        
        public PostureType()
        {

        }

        public PostureType(string name, string path)
        {
            if (brushes == null)
            {
                brushes = new List<Tuple<string, Brush>>();
                Type type = typeof(System.Windows.Media.Brushes);
                PropertyInfo[] colors = type.GetProperties();
                foreach (PropertyInfo pColor in colors)
                {
                    BrushConverter converter = new BrushConverter();
                    Brush brush = converter.ConvertFromString(pColor.Name) as Brush;
                    brushes.Add(new Tuple<string, Brush>(pColor.Name, brush));
                }
            }

            this.colorName = brushes[brushIndex].Item1;
            this.color = brushes[brushIndex].Item2; 
            this.Name = name;
            this.Path = path;
            if (++brushIndex >= brushes.Count) brushIndex = 0;
        }
        
        public override string ToString()
        {
            return Name;
        }

    }
}
