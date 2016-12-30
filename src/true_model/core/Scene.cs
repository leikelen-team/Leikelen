using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class Scene
    {
        public int Id { get; set; }
        public DateTime recordStartDate { get; set; }
        public string Name { get; set; }
        public DateTime dateTime { get; set; }
        public int numberOfParticipants { get; set; }
        public string type { get; set; }
        public string place { get; set; }
        public string description { get; set; }
        public List<Person> persons { get; set; }
        public SceneStatuses status { get; set; }

        public Scene() { }
    }
}
