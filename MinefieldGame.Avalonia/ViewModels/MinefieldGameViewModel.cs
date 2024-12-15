using MinefieldGame.Model;
using MinefieldGame.Model.Game;
using MinefieldGame.Model.Math;
using MinefieldGame.Model.Mines;
using MinefieldGame.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Avalonia.ViewModels
{
    public class MinefieldGameViewModel : ViewModelBase
    {
        private IInputHandler _inputHandler;
        private IFileManager<GameState> _fileManager;
        private ViewState _viewState = ViewState.MainMenu;
        private Point2D _gameBounds;

        public bool ShowNewGameButton => _viewState == ViewState.MainMenu || _viewState == ViewState.Over;
        public bool ShowLoadGameButton => _viewState == ViewState.MainMenu || _viewState == ViewState.Over || _viewState == ViewState.Paused;
        public bool ShowExitGameButton => _viewState == ViewState.MainMenu || _viewState == ViewState.Over;
        public bool ShowSaveGameButton => _viewState == ViewState.Paused;
        public bool ShowQuitGameButton => _viewState == ViewState.Paused;

        public GameManager GameManager { get; init; }

        private SubmarineViewModel? _submarine;

        public bool HasSubmarine => _submarine != null;

        public ObservableCollection<MineViewModel> Mines { get; private set; }

        public SubmarineViewModel? Submarine 
        {
            get => _submarine;
            private set
            {
                if (value != _submarine)
                {
                    _submarine = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasSubmarine));
                }
            }
        }

        public ViewState ViewState
        {
            get => _viewState;
            private set
            {
                if (value != _viewState)
                {
                    _viewState = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShowNewGameButton));
                    OnPropertyChanged(nameof(ShowLoadGameButton));
                    OnPropertyChanged(nameof(ShowExitGameButton));
                    OnPropertyChanged(nameof(ShowSaveGameButton));
                    OnPropertyChanged(nameof(ShowQuitGameButton));
                    ViewStateUpdated?.Invoke(this, _viewState);
                }
            }
        }

        public DelegateCommand NewGameCommand { get; }
        public DelegateCommand LoadGameFDCommand { get; }
        public DelegateCommand SaveGameFDCommand { get; }
        public DelegateCommand SaveGameCommand { get; }
        public DelegateCommand LoadGameCommand { get; }
        public DelegateCommand QuitGameCommand { get; } //Quits the current game to the stopped menu
        public DelegateCommand ExitGameCommand { get; } //Exits the whole application
        public DelegateCommand StopGameCommand { get; }
        public DelegateCommand ContinueGameCommand { get; }

        public event EventHandler? GameExited;
        public event EventHandler? GameEnded;
        public event EventHandler? GamePrepared;
        public event EventHandler? GameUpdated;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        public event EventHandler<MineViewModel>? MineAdded;
        public event EventHandler<ViewState>? ViewStateUpdated;

        public MinefieldGameViewModel(IInputHandler inputHandler, ITimer timer, Point2D gameBounds)
        {
            _inputHandler = inputHandler;
            _gameBounds = gameBounds;

            _fileManager = new GameFileManager<GameState>();
            GameManager = new GameManager(_fileManager, inputHandler, timer);

            Mines = new ObservableCollection<MineViewModel>();

            LoadGameFDCommand = new DelegateCommand(param => LoadGame?.Invoke(this, EventArgs.Empty));
            SaveGameFDCommand = new DelegateCommand(param => SaveGame?.Invoke(this, EventArgs.Empty));

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param =>
            {
                if(param is string s)
                    OnLoadGame(s);
            });
            SaveGameCommand = new DelegateCommand(param =>
            {
                if (param is string s)
                    OnSaveGame(s);
            });
            QuitGameCommand = new DelegateCommand(param => OnQuitGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());
            StopGameCommand = new DelegateCommand(param => OnStopGame());
            ContinueGameCommand = new DelegateCommand(param => OnContinueGame());

            GameManager.OnGameEnded += OnGameEnded;
            GameManager.OnGamePrepared += OnGamePrepared;
            GameManager.OnMineAdded += OnMineAdded;
            GameManager.OnUpdate += OnGameUpdated;
        }

        private void OnGameEnded(object? sender, EventArgs e) 
        {
            GameEnded?.Invoke(this, e);
        }

        private void OnGamePrepared(object? sender, EventArgs e)
        {
            if (GameManager.GameState != null)
            {
                Submarine = new SubmarineViewModel(GameManager.GameState.Submarine, _gameBounds);

                var mines = GameManager.GameState.Mines;
                foreach(var m in mines)
                {
                    MineViewModel mineViewModel = new MineViewModel(m, _gameBounds);
                    Mines.Add(mineViewModel);
                }
            }

            ViewState = ViewState.Play;
            GamePrepared?.Invoke(this, e);
        }

        private void OnMineAdded(Mine mine)
        {
            MineViewModel mineViewModel = new MineViewModel(mine, _gameBounds);
            Mines.Add(mineViewModel);

            MineAdded?.Invoke(this, mineViewModel);
        }

        private void OnGameUpdated(object? sender, EventArgs e)
        {
            Submarine?.UpdatePosition();

            List<MineViewModel> removed = new List<MineViewModel>();
            foreach (var mine in Mines)
            {
                mine.UpdatePosition();

                if (mine.CheckIfMineEvaded())
                {
                    removed.Add(mine);
                }
            }

            foreach (var mine in removed)
            {
                Mines.Remove(mine);
            }

            GameUpdated?.Invoke(this, e);
        }

        private void OnNewGame()
        {
            GameManager?.EndGame();
            Mines.Clear();

            GameManager?.NewGame();
            GameManager?.StartGame();
            ViewState = ViewState.Play;
        }

        private void OnLoadGame(string fileName)
        {
            GameManager?.EndGame();
            Mines.Clear();

            GameManager?.LoadGame(fileName);
            GameManager?.StartGame();
            ViewState = ViewState.Play;
        }

        private void OnSaveGame(string fileName)
        {
            GameManager?.SaveGame(fileName);
        }

        private void OnQuitGame()
        {
            GameManager?.EndGame();
            Mines.Clear();

            Submarine = null;
            ViewState = ViewState.MainMenu;
        }

        private void OnExitGame()
        {
            GameManager?.EndGame();
            Submarine = null;
            ViewState = ViewState.MainMenu;

            GameExited?.Invoke(this, EventArgs.Empty);
        }

        private void OnStopGame()
        {
            GameManager?.PauseGame();
            ViewState = ViewState.Paused;
        }

        private void OnContinueGame()
        {
            GameManager?.StartGame();
            ViewState = ViewState.Play;
        }
    }
}
