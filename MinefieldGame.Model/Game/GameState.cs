using MinefieldGame.Model.Mines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MinefieldGame.Persistence;
using System.Text.Json.Serialization;

namespace MinefieldGame.Model.Game
{
    public record GameState : ISerializeable
    {
        public required Submarine Submarine { get; set; }
        public List<Mine> Mines { get; set; } = new List<Mine>();
        public double ElapsedSeconds { get; set; } = 0.0;

        [JsonIgnore]
        public bool ShouldAddMine
        {
            get
            {
                return Mines.Count <= (int)GetMineCountFunction(ElapsedSeconds);
            }
        }

        public double DifficultyBias
        {
            get
            {
                return 5 / (1 + MathF.Pow(MathF.E, (float)(40.0 - ElapsedSeconds) / 15f)) - 2;
            }
        }

        public double GetMineCountFunction(double seconds)
        {
            return seconds * seconds / 10.0 - 3.0;
        }

        public ISerializer<T> GetSerializer<T>() where T : class, ISerializeable
        {
            return (ISerializer<T>)new GameStateSerializer(this);
        }
    }
}
