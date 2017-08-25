using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Persistence
{
    public interface IDbProvider
    {
        void CreateConnection(string options);
        void CloseConnection();

        List<Scene> LoadScenes();
        Scene LoadScene(int sceneId);
        Scene SaveScene(Scene instance);
        Scene UpdateScene(Scene newScene);

        List<Person> LoadPersons();
        Person SavePerson(Person person);
        Person UpdatePerson(Person newPerson);

        PersonInScene AddPersonToScene(Person person, Scene scene);
        bool ExistsPersonInScene(Person person, Scene scene);

        List<ModalType> LoadModals();
        ModalType LoadModal(string name);
        ModalType SaveModal(ModalType modalType);

        List<SubModalType> LoadSubModals(string ModalTypeName);
        SubModalType SaveSubModal(SubModalType submodalType);
    }
}
