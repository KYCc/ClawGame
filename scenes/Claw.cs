using Godot;

public partial class Claw : Node3D
{
    private readonly StringName MOVE_CLAW = "move_claw";
    private const float BoundX = 3.65f;
    private const float BoundZ = -2.45f;
    private const float DropX = 0.6f;
    private const float DropZ = -0.4f;

    private enum ClawState
    {
        Idle,
        MovingX,
        XStopped,
        Xoob,
        MovingZ,
        ZStopped,
        Zoob,
        ReturningX,
        ReturningZ,
        NoInput
    }
    
    private ClawState clawState = ClawState.Idle;
    [Export] public float ClawSpeed { get; set; } = 1f;
    [Export] public HingeJoint3D[] HingeJoints { get; set; } = new HingeJoint3D[4];
    [Export] public AnimationPlayer AnimPlayer;
    [Export] public SoundManager ClawSounds;
    [Export] public Camera3D Camera;
    private Tween tween;
    
    public override void _Ready()
    {
        AnimPlayer.AnimationFinished += OnAnimationFinished;
        EventManager.LevelWonEvent += OnLevelWon;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (clawState != ClawState.ZStopped && clawState != ClawState.ReturningX && clawState != ClawState.ReturningZ && clawState != ClawState.NoInput)
        {
            if (Input.IsActionPressed(MOVE_CLAW))
            {
                HandleMovement(delta);
            }

            if (Input.IsActionJustReleased(MOVE_CLAW))
            {
                HandleRelease();
            }
        }
    }

    private void OnLevelWon()
    {
        clawState = ClawState.NoInput;
    }

    private void HandleMovement(double delta)
    {
        switch (clawState)
        {
            case ClawState.Idle: 
            case ClawState.MovingX:

                if (!ClawSounds.IsPlaying("Moving"))
                {
                    ClawSounds.PlaySound("Moving");
                }
                
                if (Position.X < BoundX)
                {
                    if (clawState == ClawState.Idle)
                    {
                        EventManager.BroadcastCoinInsertedEvent();
                    }
                    
                    clawState = ClawState.MovingX;
                    Translate(Vector3.Right * ClawSpeed * (float)delta);
                }
                else
                {
                    clawState = ClawState.Xoob;
                    ClawSounds.StopSound("Moving");
                }
                break;
            
            case ClawState.XStopped: 
            case ClawState.MovingZ:
                
                if (!ClawSounds.IsPlaying("Moving"))
                {
                    ClawSounds.PlaySound("Moving");
                }
                
                if (Position.Z > BoundZ)
                {
                    clawState = ClawState.MovingZ;
                    Translate(Vector3.Forward * ClawSpeed * (float)delta);
                }
                else
                {
                    clawState = ClawState.Zoob;
                    ClawSounds.StopSound("Moving");
                }
                break;
        }
    }

    private void HandleRelease()
    {
        switch (clawState)
        {
            case ClawState.MovingX: 
            case ClawState.Xoob:
                
                
                ClawSounds.StopSound("Moving");
                ClawSounds.PlaySound("Stop");
                
                clawState = ClawState.XStopped;
                break;
            
            case ClawState.MovingZ:
            case ClawState.Zoob:
                
                ClawSounds.StopSound("Moving");
                ClawSounds.PlaySound("Stop");
                Camera.Current = true;
                
                clawState = ClawState.ZStopped;
                AnimPlayer.Play("open_motor");
                AnimPlayer.Queue("down");
                AnimPlayer.Queue("close_motor");
                AnimPlayer.Queue("up");
                break;
        }
    }

    private void OnAnimationFinished(StringName animName)
    {
        if (clawState == ClawState.ZStopped)
        {
            if (animName == "up")
            {
                Camera.Current = false;
                // Return to start position (Z axis)
                Vector3 targetZAxis = new Vector3(Position.X, Position.Y, DropZ);
                float distanceZ = Position.DistanceTo(targetZAxis);
                
                tween = GetTree().CreateTween();
                tween.TweenProperty(this, "position", targetZAxis, distanceZ / (ClawSpeed/2));
                tween.SetEase(Tween.EaseType.Out);
                tween.Finished += OnReturnZTweenFinished;
                clawState = ClawState.ReturningZ;
            }
        }
    }
    
    private void OnReturnZTweenFinished()
    {
        // Return to start position (X axis)
        Vector3 targetXAxis = new Vector3(DropX, Position.Y, DropZ);
        float distanceX = Position.DistanceTo(targetXAxis);
        tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", targetXAxis, distanceX / (ClawSpeed/2));
        tween.SetEase(Tween.EaseType.Out);
        tween.Finished += OnReturnXTweenFinished;
        clawState = ClawState.ReturningX;
    }
    
    private void OnReturnXTweenFinished()
    {
        ClawSounds.StopSound("Moving");
        ClawSounds.PlaySound("Stop");
        
        AnimPlayer.Queue("open_motor");
        AnimPlayer.Queue("RESET");
        AnimPlayer.AnimationFinished += OnFinalAnimationsFinished;
    }
    
    private void OnFinalAnimationsFinished(StringName animName)
    {
        if (animName == "RESET")
        {
            AnimPlayer.AnimationFinished -= OnFinalAnimationsFinished;
            clawState = ClawState.Idle;
            tween = null;
        }
    }

    private static float GetDistance(Vector3 from, Vector3 to)
    {
        return (from - to).Length();
    }

    public override void _ExitTree()
    {
        AnimPlayer.AnimationFinished -= OnAnimationFinished;
        EventManager.LevelWonEvent -= OnLevelWon;
    }
}
