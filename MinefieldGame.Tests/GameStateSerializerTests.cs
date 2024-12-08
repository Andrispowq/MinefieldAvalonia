using MinefieldGame.Model.Game;
using MinefieldGame.Model;
using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Tests
{
    [TestClass]
    public class GameStateSerializerTests
    {
        [TestMethod]
        public void TestGameSerializer()
        {
            GameState game = new GameState()
            {
                Submarine = new Submarine()
                {
                    MovementSpeed = 15.0f,
                    Position = new Point2D(400, 400),
                    Size = new Point2D(40, 40)
                }
            };

            GameStateSerializer gameStateSerializer = new GameStateSerializer(game);

            string serialized = gameStateSerializer.Serialize();
            GameState? deserialized = gameStateSerializer.Deserialize(serialized);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(game.Submarine, deserialized.Submarine);
            Assert.AreEqual(game.Mines.Count, deserialized.Mines.Count);
            for (int i = 0; i < game.Mines.Count; i++)
            {
                Assert.AreEqual(game.Mines[i], deserialized.Mines[i]);
            }

            Assert.AreEqual(game.ElapsedSeconds, deserialized.ElapsedSeconds);
        }
    }
}
