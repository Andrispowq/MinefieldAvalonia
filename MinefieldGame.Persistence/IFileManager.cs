using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Persistence
{
    public interface IFileManager<T> where T: class, ISerializeable
    {
        bool CreateSave(T game, string path);
        T? LoadSave(ISerializer<T> gameSerializer, string path);
    }
}
