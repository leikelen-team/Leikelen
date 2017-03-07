namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class ModalTypeLogic
    {
        public static SubModalType getSubModalTypeByName(this ModalType mt, string subModalTypeName)
        {
            if (mt.SubModalTypes.Exists(smt => smt.Name == subModalTypeName))
            {
                return mt.SubModalTypes.Find(smt => smt.Name == subModalTypeName);
            }
            else
            {
                SubModalType smt = new SubModalType(subModalTypeName, mt);
                mt.SubModalTypes.Add(smt);
                return smt;
            }
        }
    }
}
