using Godot;
using System;
using KaimiraGames;

public partial class PrizeSpawner : Node3D
{
    WeightedList<PrizeResource> prizes = new WeightedList<PrizeResource>();
    [Export]
    private PrizeResource[] prizeResources;

    [Export] private Timer spawnDelayTimer;
    [Export] private int minPrizes = 1;
    [Export] private int maxPrizes = 20;
    [Export] private Vector3 minPlayableArea = new Vector3(0.35f, 0.6f, -0.15f);
    [Export] private Vector3 maxPlayableArea = new Vector3(3.85f, 1.6f, -2.55f);
    [Export] private Vector3 minForbiddenArea = new Vector3(0.3f, 0.6f, -0.1f);
    [Export] private Vector3 maxForbiddenArea = new Vector3(1.2f, 1.6f, -1f);

    private int counter = 0;

    public override void _Ready()
    {
        foreach (var prizeRes in prizeResources)
        {
            prizes.Add(prizeRes, prizeRes.SpawnChanceWeight);
        }

        spawnDelayTimer.Timeout += OnSpawnDelayTimeout;
        spawnDelayTimer.Start();
    }
    
    private void SpawnPrize(Vector3 position)
    {
        var resource = prizes.Next();
        var prizeScene = resource.PrizeScene;
        var prize = prizeScene.Instantiate<Node3D>();
        prize.Position = position;
        AddChild(prize);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPos;
        
        do
        {
            float randomX = (float)GD.RandRange(minPlayableArea.X, maxPlayableArea.X);
            float randomY = (float)GD.RandRange(minPlayableArea.Y, maxPlayableArea.Y);
            float randomZ = (float)GD.RandRange(minPlayableArea.Z, maxPlayableArea.Z);

            randomPos = new Vector3(randomX, randomY, randomZ);
        }
        while (randomPos.X >= minForbiddenArea.X && randomPos.X <= maxForbiddenArea.X &&
               randomPos.Z <= minForbiddenArea.Z && randomPos.Z >= maxForbiddenArea.Z);

        return randomPos;
    }
    
    private void OnSpawnDelayTimeout()
    {
        counter++;
        if (counter > maxPrizes)
        {
            spawnDelayTimer.Stop();
            return;
        }
        var randomPosition = GetRandomPosition();
        SpawnPrize(randomPosition);
    }
}
