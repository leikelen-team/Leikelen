using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Access;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.View.Classes;
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

        //TODO: separar esto por tipo, hacerlo bien blabla, interfaz o algo que contenga todo esto.
        public static IntervalInsert IntervalInsert;
        public static EventInsert EventInsert;

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
