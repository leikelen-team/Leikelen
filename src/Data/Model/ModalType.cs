using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class ModalType
    {
        public int ModalTypeId { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }

        public int PersonInSceneId { get; set; }
        public PersonInScene PersonInScene { get; set; }

        public List<SubModalType> SubModalTypes { get; set; }

        public ModalType()
        {
            this.SubModalTypes = new List<SubModalType>();
        }

        public ModalType(string name, string explanation, PersonInScene pis)
        {
            this.Name = name;
            this.Explanation = explanation;
            this.PersonInScene = pis;
            this.SubModalTypes = new List<SubModalType>();
        }
    }
}
