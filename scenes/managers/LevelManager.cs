using Godot;
using System;

public partial class LevelManager : Node
{
    [Export] private int coinsToWin = 30;
    [Export] private int coins = 10;
    [Export] private string coinSpritePath;
    [Export] private string nextLevelPath;
    private Tween tween;
    
    [Export] private Hud hud;
    [Export] private Camera3D camera;
    [Export] private PlaqueObject plaqueObject;
    public override void _Ready()
    {
        hud.SetCoinSprite(coinSpritePath);
        hud.SetCoinsToWin(coinsToWin);
        hud.SetCoins(coins);
        EventManager.PrizeCollectedEvent += OnPrizeCollected;
        EventManager.CoinInsertedEvent += OnCoinInserted;
        EventManager.LevelWonEvent += OnLevelWon;
    }
    
    private void OnPrizeCollected(PrizeResource prizeResource)
    {
        plaqueObject.Shake();
        coins += prizeResource.Coins;
        hud.SetCoins(coins);
        GD.Print($"Coins: {coins}");
        if (coins >= coinsToWin)
        {
            GD.Print("You win!");
            EventManager.BroadcastLevelWonEvent();
            tween?.Kill();
            tween = GetTree().CreateTween();
            tween.SetTrans(Tween.TransitionType.Sine);
            tween.SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(camera, "rotation_degrees", new Vector3(90, 0, 0), 5f);
            tween.Finished += () =>
            {
                GetTree().ChangeSceneToFile(nextLevelPath);
            };
        }
    }
    
    private void OnCoinInserted()
    {
        coins -= 1;
        hud.SetCoins(coins);
        GD.Print($"Coins: {coins}");
        if (coins < 0)
        {
            GD.Print("Game Over");
            GetTree().ChangeSceneToFile("res://scenes/ui/main_menu.tscn");
        }
    }
    
    private void OnLevelWon()
    {
        camera.Current = true;
    }

    public override void _ExitTree()
    {
        EventManager.PrizeCollectedEvent -= OnPrizeCollected;
        EventManager.CoinInsertedEvent -= OnCoinInserted;
        EventManager.LevelWonEvent -= OnLevelWon;
    }
}