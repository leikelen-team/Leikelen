using cl.uv.leikelen.src.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.src.Data.Persistence.Interface
{
    public interface IBackupDataContext
    {
        bool isConnected();
        DbContext CreateConnection(string options);
        void CloseConnection();
        Scene LoadScene();
        void SaveScene(Scene instance);
    }
}
