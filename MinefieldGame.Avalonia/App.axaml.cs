using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using MinefieldGame.Avalonia.ViewModels;
using MinefieldGame.Avalonia.Views;
using MinefieldGame.Model;
using MinefieldGame.Model.Math;
using System;

namespace MinefieldGame.Avalonia;

public partial class App : Application
{
    private InputHandler _inputHandler = null!;
    private GameTimer _gameTimer = null!;

    private MinefieldGameViewModel _viewModel = null!;

    private Point2D _gameBounds => new Point2D(1280, 720);

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        _inputHandler = new InputHandler();
        _gameTimer = new GameTimer();

        _viewModel = new MinefieldGameViewModel(_inputHandler, _gameTimer, _gameBounds);
        _viewModel.GameExited += GameExitedEvent;
        _viewModel.GamePrepared += GamePreparedEvent;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                CanResize = false,
                DataContext = _viewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void GamePreparedEvent(object? sender, EventArgs a)
    {
        var viewState = _viewModel.ViewState;
        Console.WriteLine(viewState);
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
