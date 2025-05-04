using Godot;
using System;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    private Dictionary<string, AudioStreamPlayer> sounds { get; } = new();

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is AudioStreamPlayer player)
            {
                var name = child.Name;
                sounds[name] = player;
            }
        }
    }
    
    public bool PlaySound(string name)
    {
        if (sounds.TryGetValue(name, out var player))
        {
            player.Play();
            return true;
        }
        GD.PrintErr($"Sound '{name}' not found.");
        return false;
    }
    
    public bool StopSound(string name)
    {
        if (sounds.TryGetValue(name, out var player))
        {
            player.Stop();
            return true;
        }
        GD.PrintErr($"Sound '{name}' not found.");
        return false;
    }
    
    public bool IsPlaying(string name)
    {
        if (sounds.TryGetValue(name, out var player))
        {
            return player.Playing;
        }
        GD.PrintErr($"Sound '{name}' not found.");
        return false;
    }
    
    public void StopAllSounds()
    {
        foreach (var player in sounds.Values)
        {
            player.Stop();
        }
    }

    public AudioStreamPlayer GetPlayer(string name)
    {
        return sounds[name];
    }
}
