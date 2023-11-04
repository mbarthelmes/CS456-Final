using Godot;
using System;

public partial class Camera : Camera3D
{
	private Node3D _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = (Node3D)FindParent("Node3D").FindChild("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = _player.Position + (Vector3.Back * 5) + (Vector3.Up * 5);
		LookAt(_player.Position, Vector3.Up);
		//todo movement
	}
}
