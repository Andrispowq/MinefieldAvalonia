using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Persistence
{
    public interface ISerializeable
    {
        ISerializer<T> GetSerializer<T>() where T: class, ISerializeable;
    }
}
