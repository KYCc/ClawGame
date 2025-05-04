using System;

public static class EventManager
{
    // Broadcasters: ConveyorBelt.cs
    // Listeners: CoinLabel.cs
    public static Action<PrizeResource> PrizeCollectedEvent; 
    public static void BroadcastPrizeCollectedEvent(PrizeResource prize)
    {
        PrizeCollectedEvent?.Invoke(prize);
    }

    // Broadcasters: Claw.cs
    // Listeners: CoinLabel.cs
    public static Action CoinInsertedEvent;
    public static void BroadcastCoinInsertedEvent()
    {
        CoinInsertedEvent?.Invoke();
    }
    
    public static Action LevelWonEvent;
    public static void BroadcastLevelWonEvent()
    {
        LevelWonEvent?.Invoke();
    }
}
