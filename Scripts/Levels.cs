using Godot;
using Godot.Collections;
using System;

public partial class Levels : Node
{
	public Dictionary<int, Node3D> Spawns = new Dictionary<int, Node3D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var node in FindParent("Node3D").FindChildren("Spawn"))
		{
			if(node is Spawn spawn)
			{
				Spawns.Add(spawn.Level, spawn);
				GD.Print($"Found level spawn: {spawn.Level}");
			}
		}
	}
}
