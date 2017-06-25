using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data.Model;

namespace cl.uv.leikelen.src.Data.Persistence
{
    public interface IDBProvider
    {
        void CreateConnection(string options);
        void CloseConnection();

        List<Scene> LoadScenes();
        Scene LoadScene(int sceneId ,bool timeless, bool intervals, bool events);
        void SaveScene(Scene instance);
        void UpdateScene(int sceneId, Scene newScene);

        List<Person> LoadPersons();
        void SavePerson(Person person);
        void UpdatePerson(int personId, Person newPerson);

        List<ModalType> LoadModals();
        ModalType LoadModal(string name);
        void SaveModal(ModalType modalType);

        List<SubModalType> LoadSubModals(ModalType modalType);
        void SaveSubModal(string modalTypeName, SubModalType submodalType);
    }
}
