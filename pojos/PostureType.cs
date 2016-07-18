using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    public class PostureType
    {
        //JointPosition jointPosition;
        //public List<Joint> joints { get; private set; }
        
        public static List<PostureType> availablesPostureTypes { get; private set; } = new List<PostureType>();
        public static PostureType none = new PostureType(Properties.Resources.NonePostureName);
        private static List<Tuple<string, Brush>> brushes = null;
        private static int brushIndex = 0;
        
        static PostureType(){
            availablesPostureTypes.Add(none);
        }



        public string name { get; private set; }
        public string colorName { get; private set; }
        public Brush color { get; private set; }
        


        public PostureType(string name)
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
                    brushes.Add( new Tuple<string, Brush>(pColor.Name, brush) );
                }
            }

            this.colorName = brushes[brushIndex].Item1;
            this.color = brushes[brushIndex].Item2; // brushes[indexBrush]; 
            
            if (++brushIndex >= brushes.Count) brushIndex = 0;
            this.name = name;
            //if ( name == Properties.Resources.NonePostureName ) postureTypesAvailables.Add(this);
        }

        public override string ToString()
        {
            return name;
        }

    }
}
