using Godot;
using System;

public partial class PrizeComponent : Node
{
    [Export(PropertyHint.File, "*.tres")] 
    private string prizeResourcePath;
    [Export]
    private NodePath rigidBodyPath;
    public PrizeResource PrizeResource { get; private set; }
    private RigidBody3D rigidBody;

    public override void _Ready()
    {
        PrizeResource = (PrizeResource)ResourceLoader.Load(prizeResourcePath);
        rigidBody = GetNode<RigidBody3D>(rigidBodyPath);
        rigidBody.Mass = PrizeResource.Mass;
        rigidBody.SetCollisionMaskValue(2, true);
    }
    
}
