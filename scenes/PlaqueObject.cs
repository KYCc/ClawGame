using Godot;
using System;

public partial class PlaqueObject : Node3D
{
    private AudioStreamPlayer audioStreamPlayer;
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Shake()
    {
        audioStreamPlayer.Play();
        animationPlayer.Play("shake");
    }
}
