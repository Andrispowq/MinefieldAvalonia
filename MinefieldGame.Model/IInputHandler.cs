using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model
{
    public interface IInputHandler
    {
        bool IsPressed(char key);
    }
}
