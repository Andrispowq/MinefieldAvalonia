using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;

namespace MinefieldGame.Avalonia.Views;

public partial class MainView : UserControl
{
    private ImageBrush _submarineImage;

    public MainView()
    {
        InitializeComponent();

        _submarineImage = new ImageBrush();
        try
        {
            _submarineImage.Source = new Bitmap(System.IO.Path.Combine(Environment.CurrentDirectory, "res", "submarine.jpg"));
        }
        catch
        {
            throw new Exception("ERROR: you need to have the res/submarine.png file next to the executable!");
        }

        submarine.Fill = _submarineImage;
    }
}
