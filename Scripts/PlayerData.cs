using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

public partial class PlayerData : Node
{
	public Dictionary<int, PlayerInfo> Players = new Dictionary<int, PlayerInfo>();

	public void OnPlayerConnected(int id)
	{
		Players.TryAdd(id, new PlayerInfo());
	}

	public void OnPlayerDisconnected(int id)
	{
		Players.Remove(id);
	}

	public void OnPlayerCompletedLevel(int id, int level)
	{
		Players[id].Levels.Add(level);
	}

	public void OnPlayerDied(int id, int level)
	{
		//TODO telelportat the character at a position and reset velocities
		//Get the position they should teleport to using the level they died in
	}
}

public class PlayerInfo
{
	public long Score;
	public HashSet<int> Levels = new HashSet<int>();
}