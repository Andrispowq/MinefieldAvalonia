using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Persistence
{
    public interface ISerializer<T> where T : class, ISerializeable
    {
        string Serialize();
        T? Deserialize(string serialized);
    }
}
