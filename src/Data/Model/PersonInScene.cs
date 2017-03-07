using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class PersonInScene
    {
        public int PersonInSceneId { get; set; }

        public int SceneId { get; set; }
        public Scene Scene { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public List<ModalType> ModalTypes { get; set; }

        public PersonInScene()
        {
            this.ModalTypes = new List<ModalType>();
        }

        public PersonInScene(Scene scene, Person person)
        {
            this.Scene = scene;
            this.Person = person;
            this.ModalTypes = new List<ModalType>();
        }
    }
}
