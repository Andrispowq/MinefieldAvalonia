using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Persistence
{
    public class GameFileManager<T> : IFileManager<T> where T: class, ISerializeable
    {
        public bool CreateSave(T game, string path)
        {
            try
            {
                File.WriteAllText(path, game.GetSerializer<T>().Serialize());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {e.Message} was thrown while trying to write to {path}.");
                return false;
            }
        }

        public T? LoadSave(ISerializer<T> gameSerializer, string path)
        {
            try
            {
                return gameSerializer.Deserialize(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {e.Message} was thrown while trying to read from {path}.");
                return null;
            }
        }
    }
}
