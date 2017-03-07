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
                ModalType mt = new ModalType(modalTypeName, pis);
                pis.ModalTypes.Add(mt);
                return mt;
            }
        }
    }
}
