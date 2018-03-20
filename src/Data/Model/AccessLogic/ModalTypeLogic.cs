namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class ModalTypeLogic
    {
        public static SubModalType getSubModalType(this ModalType mt, string subModalTypeName)
        {
            if (mt.SubModalTypes.Exists(smt => smt.Name == subModalTypeName))
            {
                return mt.SubModalTypes.Find(smt => smt.Name == subModalTypeName);
            }
            else
            {
                throw new System.Exception("SubModalType don't exists");
            }
        }

        public static bool hasSubModalType(this ModalType mt, string subModalTypeName)
        {
            if (mt.SubModalTypes.Exists(smt => smt.Name == subModalTypeName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static SubModalType addSubModalType(this ModalType mt, string subModalTypeName, string explanation, string path, DataType dataType)
        {
            if (mt.SubModalTypes.Exists(smt => smt.Name == subModalTypeName))
            {
                throw new System.Exception("SubModalType already exists");
            }
            else
            {
                SubModalType smt = new SubModalType(subModalTypeName, explanation, path, dataType, mt);
                mt.SubModalTypes.Add(smt);
                return smt;
            }
        }
    }
}
