using Godot;
using System;

public partial class MainMenu : CenterContainer
{
    private Button startButton;
    private Button quitButton;

    public override void _Ready()
    {
        startButton = GetNode<Button>("%StartButton");
        quitButton = GetNode<Button>("%QuitButton");
        
        startButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/levels/level_beach.tscn");
        quitButton.Pressed += () => GetTree().Quit();
    }
}
