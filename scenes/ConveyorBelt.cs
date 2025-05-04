using Godot;
using System;
using System.Collections.Generic;

public partial class ConveyorBelt : Node3D
{
    [Export]
    private Area3D area3D;

    [Export] private Timer destroyTimer;
    
    [Export]
    public float BeltSpeed { get; set; } = 0.5f;
    private readonly HashSet<Node3D> bodiesOnBelt = new();

    public override void _Ready()
    {
        area3D.BodyEntered += OnBodyEntered;
        area3D.BodyExited += OnBodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var body in bodiesOnBelt)
        {
            body.GlobalTranslate(Vector3.Back * BeltSpeed * (float)delta);
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is PhysicsBody3D && !bodiesOnBelt.Contains(body))
        {
            GD.Print("Body entered conveyor belt: " + body.Name);
            bodiesOnBelt.Add(body);
            // Fire out the prize collected event
            GD.Print(body.Owner.Name);
            var prizeComponent = body.Owner.GetNodeOrNull<PrizeComponent>("PrizeComponent");
            GD.Print(prizeComponent.PrizeResource.Coins);
            EventManager.BroadcastPrizeCollectedEvent(prizeComponent.PrizeResource);
        }
    }
    
    private void OnBodyExited(Node3D body)
    {
        GD.Print("Body exited conveyor belt: " + body.Name);
        if (body.Owner != null && IsInstanceValid(body))
        {
            destroyTimer.Timeout += () =>
            {
                // Check if the body is still valid before trying to use it
                if (IsInstanceValid(body))
                {
                    bodiesOnBelt.Remove(body);
                    if (IsInstanceValid(body.Owner))
                    {
                        body.Owner.QueueFree();
                    }
                }

            };
            destroyTimer.Start();
        }
    }
}
