using Godot;
using System;
using System.Collections.Generic;

public partial class Levels : Node
{
	public Dictionary<int, Node3D> Spawns = new Dictionary<int, Node3D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        IterateChildren(FindParent("Node3D"));

        void IterateChildren(Node node)
        {
            foreach (var child in node.GetChildren())
            {
                IterateChildren(child);
            }

            if (node is Spawn spawn)
            {
                Spawns[spawn.Level] = spawn;
                GD.Print($"Found level spawn: {spawn.Level}");
            }
        }
    }
}
