using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class PgSqlProvider : IDbProvider
    {
        public PgSqlProvider()
        {

        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public void CreateConnection(string options)
        {
            throw new NotImplementedException();
        }

        public ModalType LoadModal(string name)
        {
            throw new NotImplementedException();
        }

        public List<ModalType> LoadModals()
        {
            throw new NotImplementedException();
        }

        public List<Person> LoadPersons()
        {
            throw new NotImplementedException();
        }

        public Scene LoadScene(int sceneId, bool timeless, bool intervals, bool events)
        {
            throw new NotImplementedException();
        }

        public List<Scene> LoadScenes()
        {
            throw new NotImplementedException();
        }

        public List<SubModalType> LoadSubModals(ModalType modalType)
        {
            throw new NotImplementedException();
        }

        public void SaveModal(ModalType modalType)
        {
            throw new NotImplementedException();
        }

        public void SavePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void SaveScene(Scene instance)
        {
            throw new NotImplementedException();
        }

        public void SaveSubModal(string modalTypeName, SubModalType submodalType)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(int personId, Person newPerson)
        {
            throw new NotImplementedException();
        }

        public void UpdateScene(int sceneId, Scene newScene)
        {
            throw new NotImplementedException();
        }
    }
}
