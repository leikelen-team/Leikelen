using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.view;
using System.Windows.Media;

namespace cl.uv.leikelen.src.true_model
{
    public class Person
    {
        public int ListIndex { get; set; }
        public int Id { get; set; }
        public long TrackingId { get; set; }
        public string Name { get; set; }
        public GenderEnum Gender { get; set; }
        public int Age { get; set; }
        public List<Scene> scenes { get; set; }
        public bool HasBeenTracked { get; set; }

        public Brush Color { get; set; }
        public PersonView View { get; set; }

        public PersonData data { get; set; }

        public Person() { }
    }
}
