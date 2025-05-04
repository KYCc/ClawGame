using Godot;
using System;

public partial class CoinLabel : Label
{
    private int coins;
    private int coinsToWin;
    
    public void SetCoinsToWin(int amount)
    {
        coinsToWin = amount;
    }
    
    public void SetCoins(int amount)
    {
        coins = amount;
        Text = coins + "/" + coinsToWin;
    }
}
