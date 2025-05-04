using Godot;
using System;

public partial class Hud : Control
{
    [Export]
    private CoinLabel coinLabel;
    [Export] 
    private Sprite2D coinSprite;
    
    public void SetCoinsToWin(int amount)
    {
        coinLabel.SetCoinsToWin(amount);
    }
    
    public void SetCoins(int amount)
    {
        coinLabel.SetCoins(amount);
    }
    
    public void SetCoinSprite(string path)
    {
        coinSprite.Texture = ResourceLoader.Load<Texture2D>(path);
    }
}
