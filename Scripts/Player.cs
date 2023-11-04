using Godot;
using Marbles.Scripts;
using System;

public partial class Player : RigidBody3D
{
    public long Id = 1;

    [Export]
    public float GroundSpeed = 5;

    [Export]
    public float AirSpeed = 2;

    [Export]
    public float JumpStrength = 500;

    private CollisionShape3D _collider;
    private Node3D _camera;
    private bool _jumping;
    private bool _grounded;

    private PlayerData _playerData;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _collider = (CollisionShape3D)FindChild("CollisionShape3D");
        _camera = (Node3D)FindParent("Node3D").FindChild("Camera3D");
        _playerData = (PlayerData)FindParent("Node3D").FindChild("Session").FindChild("PlayerData");

        _playerData.OnPlayerConnected(this);
    }

    public override void _Process(double delta)
    {
        if (Multiplayer.IsServer())
        {

        }
        else if(Multiplayer.MultiplayerPeer != null && Multiplayer.MultiplayerPeer.GetUniqueId() == Id)
        {
            RpcId(1, nameof(UpdateServer), Id, Position, Quaternion, LinearVelocity, AngularVelocity);
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
    public void UpdateServer(long id, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        var player = _playerData.Players[id].Player;
        player.Position = position;
        player.Quaternion = rotation;
        player.LinearVelocity = velocity;
        player.AngularVelocity = angularVelocity;
    }

    public void SetId(long id)
    {
        _playerData.Players.Remove(Id);
        Id = id;
        _playerData.OnPlayerConnected(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Multiplayer.MultiplayerPeer.GetUniqueId() != Id)
            return;

        var spaceState = GetWorld3D().DirectSpaceState;
        // use global coordinates, not local to node
        var query = PhysicsRayQueryParameters3D.Create(GlobalPosition, GlobalPosition - (Vector3.One * (1 + 0.1f)));
        var result = spaceState.IntersectRay(query);

        if (result.TryGetValue("collider", out Variant collider) && collider.As<CollisionObject3D>() != this)
        {
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }

        Quaternion rot = Quaternion.FromEuler(new Vector3(0, (Mathf.Pi * 2) - _camera.Rotation.Y, 0));
        Vector3 input = Vector3.Zero;

        input.X = Input.GetAxis("Left", "Right");
        input.Z = Input.GetAxis("Forward", "Backward");
        input = input.Normalized();

        LinearVelocity = LinearVelocity + (input * rot) * (float)delta * (_grounded ? GroundSpeed : AirSpeed);

        if (Input.IsActionPressed("Jump"))
        {
            if (!_jumping && _grounded)
            {
                _jumping = true;
                LinearVelocity = LinearVelocity + Vector3.Up * (float)delta * JumpStrength;
            }
        }
        else
        {
            _jumping = false;
        }
    }
}
