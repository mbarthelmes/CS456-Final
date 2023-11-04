using Godot;
using System;

public partial class Player : RigidBody3D
{
	[Export]
	public float Speed = 5;

    [Export]
    public float JumpStrength = 20;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("Forward"))
        {
            LinearVelocity = LinearVelocity + Vector3.Forward * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Backward"))
        {
            LinearVelocity = LinearVelocity + Vector3.Back * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Left"))
        {
            LinearVelocity = LinearVelocity + Vector3.Left * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Right"))
        {
            LinearVelocity = LinearVelocity + Vector3.Right * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Jump"))
        {
            LinearVelocity = LinearVelocity + Vector3.Up * (float)delta * JumpStrength;
        }
    }
}
