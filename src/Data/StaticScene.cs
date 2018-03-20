using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Access;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.View.Widget;
using System.Windows.Media;
using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data
{
    public static class StaticScene
    {
        private static Scene _instance;

        public static Brush[] boneColors = {
            Brushes.Red,
            Brushes.Green,
            Brushes.Orange,
            Brushes.Blue,
            Brushes.Indigo,
            Brushes.Coral };

        public static Brush[] jointColors = {
            Brushes.Blue,
            Brushes.Magenta,
            Brushes.DarkViolet,
            Brushes.Green,
            Brushes.Orange,
            Brushes.Red };

        public static Dictionary<Person, PersonView> personsView = new Dictionary<Person, PersonView>();

        //TODO: separar esto por tipo, hacer una interfaz o algo que contenga todos los accesores a datos.
        public static IntervalInsert IntervalInsert = new IntervalInsert();
        public static EventInsert EventInsert = new EventInsert();

        public static Scene Instance
        {
            get
            {
                return _instance;
            }
        }

        public static void CreateSceneFromRecord(string name)
        {
            _instance = new Scene();
            _instance.CreateFromRecord(name);
        }
    }
}
