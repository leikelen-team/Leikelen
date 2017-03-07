using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class SubModalType
    {
        public int SubModalTypeId { get; set; }
        public string Name { get; set; }

        public int ModalTypeId { get; set; }
        public ModalType ModalType { get; set; }

        public List<EventData> EventDatas { get; set; }
        public IntervalGroup IntervalGroup { get; set; }
        public Graph Graph { get; set; }
        public NumberData NumberData { get; set; }
        public StringData StringData { get; set; }
        public JointArray JointArray { get; set; }

        public SubModalType()
        {
            this.EventDatas = new List<EventData>();
        }

        public SubModalType(string name, ModalType mt)
        {
            this.Name = name;
            this.ModalType = mt;
            this.EventDatas = new List<EventData>();
        }
    }
}
