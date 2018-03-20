namespace cl.uv.leikelen.src.Data.Model
{
    public class ArrayData
    {
        public int ArrayDataId { get; set; }
        public int DataIndex { get; set; }
        public double NumberData { get; set; }

        public int SubModalTypeId { get; set; }
        public SubModalType SubModalType { get; set; }

        public ArrayData() { }

        public ArrayData(SubModalType smt, int dataIndex, double numberData)
        {
            this.SubModalType = smt;
            this.DataIndex = dataIndex;
            this.NumberData = numberData;
        }
    }
}
