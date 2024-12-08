using MinefieldGame.Model.Mines;
using MinefieldGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Game
{
    public class GameStateSerializer(GameState? obj): ISerializer<GameState>
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new MineConverter() },
            WriteIndented = true
        };

        public string Serialize()
        {
            return JsonSerializer.Serialize(obj, options);
        }

        public GameState? Deserialize(string encoded)
        {
            return JsonSerializer.Deserialize<GameState>(encoded, options);
        }
    }
}
