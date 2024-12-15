using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MinefieldGame.Avalonia.ViewModels;
using MinefieldGame.Avalonia.Views;
using MinefieldGame.Model;
using MinefieldGame.Model.Math;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ObjectiveC;

namespace MinefieldGame.Avalonia;

public partial class App : Application
{
    private ImageBrush _submarineImage = null!;
    private ImageBrush _mineImage = null!;

    private InputHandler _inputHandler = null!;
    private GameTimer _gameTimer = null!;

    private MinefieldGameViewModel _viewModel = null!;

    private Point2D _gameBounds => new Point2D(1280, 720);

    private TopLevel? TopLevel
    {
        get
        {
            return ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                _ => null
            };
        }
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        _submarineImage = new ImageBrush();
        try
        {
            _submarineImage.Source = new Bitmap(System.IO.Path.Combine(Environment.CurrentDirectory, "res", "submarine.jpg"));
        }
        catch
        {
            throw new Exception("ERROR: you need to have the res/submarine.png file next to the executable!");
        }

        _mineImage = new ImageBrush();
        try
        {
            _mineImage.Source = new Bitmap(System.IO.Path.Combine(Environment.CurrentDirectory, "res", "mine.jpg"));
        }
        catch
        {
            throw new Exception("ERROR: you need to have the res/submarine.png file next to the executable!");
        }

        _inputHandler = new InputHandler();
        _gameTimer = new GameTimer();

        _viewModel = new MinefieldGameViewModel(_inputHandler, _gameTimer, _gameBounds);
        _viewModel.GameExited += GameExitedEvent;
        _viewModel.GamePrepared += GamePreparedEvent;
        _viewModel.GameEnded += GameEndedEvent;
        _viewModel.MineAdded += MineAddedEvent;
        _viewModel.SaveGame += SaveGame;
        _viewModel.LoadGame += LoadGame;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new MainWindow
            {
                Width = _gameBounds.X,
                Height = _gameBounds.Y,
                CanResize = false,
                DataContext = _viewModel
            };
            window.KeyDown += OnKeyDown;
            window.KeyUp += OnKeyUp;

            desktop.MainWindow = window;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var view = new MainView
            {
                Width = _gameBounds.X,
                Height = _gameBounds.Y,
                DataContext = _viewModel
            };

            singleViewPlatform.MainView = view;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void LoadGame(object? sender, EventArgs a)
    {
        if (TopLevel == null)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Minefield Game",
                    "File saving and loading not supported!",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
            return;
        }

        try
        {
            var files = await TopLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Minefield Game",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Minefield Game")
                    {
                        Patterns = new[] { "*.mfg" }
                    }
                }
            });

            if (files.Count > 0)
            {
                _viewModel.LoadGameCommand.Execute(files[0].TryGetLocalPath());
            }
        }
        catch (Exception)
        {
            await MessageBoxManager.GetMessageBoxStandard("Minefield Game", "Loading file is unsuccessful",
                    ButtonEnum.Ok, Icon.Error).ShowAsync();
        }
    }

    private async void SaveGame(object? sender, EventArgs a)
    {
        if (TopLevel == null)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Minefield Game",
                    "File saving and loading not supported!",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
            return;
        }

        try
        {
            var file = await TopLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Minefield Game",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Minefield Game")
                    {
                        Patterns = new[] { "*.mfg" }
                    }
                }
            });

            if (file != null)
            {
                _viewModel.SaveGameCommand.Execute(file.TryGetLocalPath());
            }
        }
        catch (Exception)
        {
            await MessageBoxManager.GetMessageBoxStandard("Minefield Game", "Saving game is unsuccessful",
                    ButtonEnum.Ok, Icon.Error).ShowAsync();
        }
    }

    private void GameEndedEvent(object? sender, EventArgs a)
    {
        MessageBoxManager.GetMessageBoxStandard("Game over!", "The game is over!").ShowAsync();
        _viewModel.QuitGameCommand.Execute(this);
        _inputHandler.Clear();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            if (_viewModel.ViewState == ViewState.Play)
            {
                _viewModel.StopGameCommand.Execute(this);
                _inputHandler.Clear();
            }
            else if (_viewModel.ViewState == ViewState.Paused)
            {
                _viewModel?.ContinueGameCommand.Execute(this);
                _inputHandler.Clear();
            }
        }

        if (_viewModel?.ViewState == ViewState.Play)
        {
            switch (e.Key)
            {
                case Key.W: _inputHandler.KeyPressed('W'); break;
                case Key.S: _inputHandler.KeyPressed('S'); break;
                case Key.A: _inputHandler.KeyPressed('A'); break;
                case Key.D: _inputHandler.KeyPressed('D'); break;
            }
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (_viewModel?.ViewState == ViewState.Play)
        {
            switch (e.Key)
            {
                case Key.W: _inputHandler.KeyReleased('W'); break;
                case Key.S: _inputHandler.KeyReleased('S'); break;
                case Key.A: _inputHandler.KeyReleased('A'); break;
                case Key.D: _inputHandler.KeyReleased('D'); break;
            }
        }
    }

    private void GamePreparedEvent(object? sender, EventArgs a)
    {
        _viewModel.Submarine!.Brush = _submarineImage;
    }

    private void MineAddedEvent(object? sender, MineViewModel mine)
    {
        mine.Brush = _mineImage;
    }

    private void GameExitedEvent(object? sender, EventArgs a)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow?.Close();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            //singleViewPlatform.MainView?.
        }
    }
}
