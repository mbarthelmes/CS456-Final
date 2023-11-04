using Godot;
using System;
using Godot.NativeInterop;

public partial class Camera : Camera3D
{
	private Node3D _player;
    private Node2D _menu;
    private float _rotationX = Mathf.Pi / 2;
    private float _rotationY = 0f;
    private float LookAroundSpeed = 0.01f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = (Node3D)FindParent("Node3D").FindChild("Player");
        _menu = (Node2D)FindParent("Node3D").FindChild("MainMenu");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position = _player.Position + (Vector3.Forward * Quaternion.FromEuler(new Vector3(-_rotationY, 0, 0)) * Quaternion.FromEuler(new Vector3(0, _rotationX, 0))) * 5 + Vector3.Up;
        LookAt(_player.Position, Vector3.Up);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            if (_menu.IsVisibleInTree())
            {
                Input.MouseMode = Input.MouseModeEnum.Confined;
            }
            else
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
                _rotationX += mouseMotionEvent.Relative.X * LookAroundSpeed;
                _rotationY += mouseMotionEvent.Relative.Y * LookAroundSpeed;
            }
            
            _rotationY = Mathf.Clamp(_rotationY, -Mathf.Pi / 2 + 0.01f, Mathf.Pi / 2 - 0.01f);
        }
    }
}
