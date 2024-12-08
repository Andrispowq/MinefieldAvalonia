using MinefieldGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Avalonia
{
    public class InputHandler : IInputHandler
    {
        private Dictionary<char, bool> keys = new Dictionary<char, bool>();

        public void Clear()
        {
            keys.Clear();
        }

        public void KeyPressed(char key)
        {
            keys[key] = true;
        }

        public void KeyReleased(char key)
        {
            keys[key] = false;
        }

        public bool IsPressed(char key)
        {
            try
            {
                return keys[key];
            }
            catch { return false; }
        }
    }
}
