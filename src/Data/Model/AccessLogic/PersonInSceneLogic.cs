namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class PersonInSceneLogic
    {
        public static ModalType getModalType(this PersonInScene pis, string modalTypeName)
        {
            if(pis.ModalTypes.Exists(mt => mt.Name == modalTypeName))
            {
                return pis.ModalTypes.Find(mt => mt.Name == modalTypeName);
            }
            else
            {
                throw new System.Exception("ModalType don't exists");
            }
        }

        public static bool hasModalType(this PersonInScene pis, string modalTypeName)
        {
            if (pis.ModalTypes.Exists(mt => mt.Name == modalTypeName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ModalType addModalType(this PersonInScene pis, string modalTypeName, string explanation)
        {
            if (pis.ModalTypes.Exists(mt => mt.Name == modalTypeName))
            {
                throw new System.Exception("ModalType already exists");
            }
            else
            {
                ModalType mt = new ModalType(modalTypeName, explanation, pis);
                pis.ModalTypes.Add(mt);
                return mt;
            }
        }
    }
}
