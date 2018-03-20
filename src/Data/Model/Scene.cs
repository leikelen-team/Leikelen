using System;
using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class Scene
    {
        public int SceneId { get; set; }
        public int NumberOfParticipants { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Place { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime RecordStartDate { get; set; }
        public DateTime RealDateTime { get; set; }
        public SceneStatus Status { get; set; }

        public List<PersonInScene> PersonsInScene { get; set; }

        public Scene()
        {
            PersonsInScene = new List<PersonInScene>();
        }
    }
}
