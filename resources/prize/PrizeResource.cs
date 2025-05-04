using Godot;
using System;
using Godot.Collections;
using Theme = TRENDYGAME.scripts.Theme;

[GlobalClass]
public partial class PrizeResource : Resource
{
    [Export] public int Coins { get; private set; }
    [Export] public float Mass { get; private set; }
    [Export] public bool CanSpawnOnMovingPlatform { get; private set; }
    [Export] public int SpawnChanceWeight { get; private set; }
    [Export] public Array<Theme> Themes { get; private set; }
    [Export] public PackedScene PrizeScene { get; private set; }
}
