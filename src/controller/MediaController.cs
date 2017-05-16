using cl.uv.leikelen.src.Input;

namespace cl.uv.leikelen.src.Controller
{
    public class MediaController
    {
        private static MediaController _instance;

        public static MediaController Instance
        {
            get
            {
                if (_instance == null) _instance = new MediaController();
                return _instance;
            }
        }

        public void SetFromSensor()
        {
            foreach(var input in InputLoader.Instance.Inputs)
            {
                input.Monitor.Open();
                //TODO: deshabilitar player, hacer player bien
            }
        }
        public void SetFromScene()
        {
            foreach (var input in InputLoader.Instance.Inputs)
            {
                input.Monitor.Close();
                //TODO: habilitar player, hacer player bien
            }
        }
    }

    
}
