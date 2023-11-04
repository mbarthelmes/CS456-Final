using Godot;
using System;

public partial class Player : RigidBody3D
{
    [Export]
    public float Speed = 5;

    [Export]
    public float JumpStrength = 500;

    private CollisionShape3D _collider;
    private Node3D _camera;
    private bool _jumping;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _collider = (CollisionShape3D)FindChild("CollisionShape3D");
        _camera = (Node3D)FindParent("Node3D").FindChild("Camera3D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Quaternion rot = Quaternion.FromEuler(new Vector3(0, (Mathf.Pi * 2) - _camera.Rotation.Y, 0));
        Vector3 forward = Vector3.Forward * rot;
        Vector3 right = Vector3.Right * rot;

        if (Input.IsActionPressed("Forward"))
        {
            LinearVelocity = LinearVelocity + forward * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Backward"))
        {
            LinearVelocity = LinearVelocity - forward * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Left"))
        {
            LinearVelocity = LinearVelocity - right * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Right"))
        {
            LinearVelocity = LinearVelocity + right * (float)delta * Speed;
        }

        if (Input.IsActionPressed("Jump"))
        {
            if (!_jumping)
            {
                _jumping = true;

                var spaceState = GetWorld3D().DirectSpaceState;
                // use global coordinates, not local to node
                var query = PhysicsRayQueryParameters3D.Create(GlobalPosition, GlobalPosition - (Vector3.One * (1 + 0.1f)));
                var result = spaceState.IntersectRay(query);

                if (result.TryGetValue("collider", out Variant collider) && collider.As<CollisionObject3D>() != this)
                {
                    LinearVelocity = LinearVelocity + Vector3.Up * (float)delta * JumpStrength;
                }
            }
        }
        else
        {
            _jumping = false;
        }
    }
}
