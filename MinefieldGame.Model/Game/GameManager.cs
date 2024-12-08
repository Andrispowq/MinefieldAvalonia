using MinefieldGame.Model.Math;
using MinefieldGame.Model.Mines;
using MinefieldGame.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Game
{
    public class GameManager
    {
        public GameState? GameState { get; private set; } = null;
        public IFileManager<GameState> FileManager { get; private set; }
        public IInputHandler InputHandler { get; private set; }

        public delegate void MineAddedHandler(Mine mine);

        public event EventHandler? OnGamePrepared;
        public event EventHandler? OnUpdate;
        public event MineAddedHandler? OnMineAdded;
        public event EventHandler? OnGameEnded;

        private ITimer _timer;
        private bool _gamePaused = false;
        private double _lastGameTime = 0;

        public GameManager(IFileManager<GameState> fileManager, IInputHandler inputHandler, ITimer timer)
        {
            FileManager = fileManager;
            InputHandler = inputHandler;

            _timer = timer;
            _timer.OnTick += OnTick;
        }

        private void OnTick(object? sender, EventArgs args)
        {
            double delta = _timer.ElapsedSeconds - _lastGameTime;
            _lastGameTime = _timer.ElapsedSeconds;

            if (_gamePaused || GameState == null) return;

            //shouldn't even be this big, but let's account for lag
            if (delta > 1.0) delta = 1.0;
            GameState.ElapsedSeconds += delta;

            Update((float)delta);
        }

        private void Update(float delta)
        {
            if (InputHandler.IsPressed('W'))
            {
                GameState!.Submarine.Move(MoveDirection.Up, delta);
            }
            else if (InputHandler.IsPressed('A'))
            {
                GameState!.Submarine.Move(MoveDirection.Left, delta);
            }
            else if (InputHandler.IsPressed('S'))
            {
                GameState!.Submarine.Move(MoveDirection.Down, delta);
            }
            else if (InputHandler.IsPressed('D'))
            {
                GameState!.Submarine.Move(MoveDirection.Right, delta);
            }

            if(GameState!.ShouldAddMine) 
            {
                var difficulty = RandomNumberGenerator.GetInt32(5) - 2 + GameState.DifficultyBias;
                var x = RandomNumberGenerator.GetInt32(1280);

                Mine? newMine = difficulty switch
                {
                    <= 0.0 => new EasyMine() { Position = new Point2D(x, 0) },
                    <= 2.0 => new MediumMine() { Position = new Point2D(x, 0) },
                    > 2.0 => new HardMine() { Position = new Point2D(x, 0) },
                    double.NaN => null
                };

                if (newMine != null)
                {
                    GameState.Mines.Add(newMine);
                    OnMineAdded?.Invoke(newMine);
                }
            }

            foreach (var mine in GameState.Mines)
            {
                mine.Move(delta);

                if(mine.HasCollision(GameState.Submarine))
                {
                    OnGameEnded?.Invoke(this, EventArgs.Empty);
                    GameState = null;
                    return;
                }
            }

            OnUpdate?.Invoke(this, EventArgs.Empty);
        }

        public void EndGame()
        {
            GameState = null;
            _timer.Stop();
        }

        public void PauseGame()
        {
            _gamePaused = true;
        }

        public void StartGame()
        {
            _gamePaused = false;
        }

        public void NewGame()
        {
            Submarine submarine = new Submarine()
            {
                Position = new Point2D(400, 400),
                Size = new Point2D(100, 40),
                MovementSpeed = 200.0f
            };

            GameState = new GameState() { Submarine = submarine };
            OnGamePrepared?.Invoke(this, EventArgs.Empty);
            _timer.Start();
        }

        public bool SaveGame(string path)
        {
            if (GameState == null)
            {
                return false;
            }

            return FileManager.CreateSave(GameState, path);
        }

        public bool LoadGame(string path)
        {
            GameState = FileManager.LoadSave(new GameStateSerializer(null), path);
            if (GameState == null)
            {
                return false;
            }

            OnGamePrepared?.Invoke(this, EventArgs.Empty);

            GameState?.Mines.ForEach(mine =>
            {
                OnMineAdded?.Invoke(mine);
            });

            OnUpdate?.Invoke(this, EventArgs.Empty);
            _timer.Start();
            return true;
        }
    }
}
