using Avalonia.Controls;
using Avalonia.Platform;
using MinefieldGame.Avalonia;
using MinefieldGame.Common.ViewModels;
using MinefieldGame.Model;
using MinefieldGame.Model.Math;
using System;
using System.Collections.Generic;

namespace MinefieldGame.Common.Views;

public partial class MainView : UserControl
{
    private InputHandler? _inputHandler = null;

    private bool isInMenu = false;

    public event EventHandler<bool>? MenuEvent;

    public MainView()
    {
        InitializeComponent();
    }

    public void InitAndroid(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;

        canvas.PointerReleased += (sender, e) =>
        {
            _inputHandler?.Clear();
        };

        canvas.PointerPressed += (sender, e) =>
        {
            var position = e.GetPosition(this);
            var bounds = canvas.Bounds;

            if (position.Y < bounds.Height / 3)
            {
                MoveUp(this);
            }
            else if (position.Y > 2 * bounds.Height / 3)
            {
                MoveDown(this);
            }
            else if (position.X < bounds.Width / 2)
            {
                MoveLeft(this);
            }
            else if (position.X > bounds.Width / 2)
            {
                MoveRight(this);
            }
        };

        canvas.DoubleTapped += (sender, e) =>
        {
            MenuAction(this);
        };
    }

    private void MoveUp(object? sender)
    {
        _inputHandler?.KeyPressed('W');
    }

    private void MoveDown(object? sender)
    {
        _inputHandler?.KeyPressed('S');
    }

    private void MoveLeft(object? sender)
    {
        _inputHandler?.KeyPressed('A');
    }

    private void MoveRight(object? sender)
    {
        _inputHandler?.KeyPressed('D');
    }

    private void MenuAction(object? sender)
    {
        isInMenu = !isInMenu;
        MenuEvent?.Invoke(this, isInMenu);
    }
}
