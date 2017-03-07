using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class Person
    {
        public int PersonId { get; set; }
        public int ListIndex { get; set; }
        public ulong TrackingId { get; set; }
        public int Age { get; set; }
        public bool HasBeenTracked { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        
        public List<PersonInScene> PersonInScenes { get; set; }

        public Person()
        {
            this.PersonInScenes = new List<PersonInScene>();
        }

        public Person(ulong trackingId, int listIndex)
        {
            this.TrackingId = trackingId;
            this.ListIndex = listIndex;
            this.PersonInScenes = new List<PersonInScene>();
        }

        public Person(ulong trackingId, int listIndex, PersonInScene pis)
        {
            this.TrackingId = trackingId;
            this.ListIndex = listIndex;
            this.PersonInScenes = new List<PersonInScene>();
            this.PersonInScenes.Add(pis);
        }
    }
}
