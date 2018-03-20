using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class SubModalType
    {
        public int SubModalTypeId { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public string Path { get; set; }
        public DataType DataType { get; set; }

        public int ModalTypeId { get; set; }
        public ModalType ModalType { get; set; }

        public List<EventData> EventGroup { get; set; }
        public List<IntervalData> IntervalGroup { get; set; }
        public List<ArrayData> ArrayGroup { get; set; }

        public SubModalType()
        {
            this.EventGroup = new List<EventData>();
            this.IntervalGroup = new List<IntervalData>();
            this.ArrayGroup = new List<ArrayData>();
        }

        public SubModalType(string name, string explanation, string path, DataType dataType, ModalType mt)
        {
            this.Name = name;
            this.Explanation = explanation;
            this.Path = path;
            this.DataType = dataType;
            this.ModalType = mt;

            this.EventGroup = new List<EventData>();
            this.IntervalGroup = new List<IntervalData>();
            this.ArrayGroup = new List<ArrayData>();
        }
    }
}
