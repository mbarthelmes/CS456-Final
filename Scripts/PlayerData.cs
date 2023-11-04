using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

public partial class PlayerData : Node
{
	public Dictionary<long, PlayerInfo> Players = new Dictionary<long, PlayerInfo>();

	public void OnPlayerConnected(Player player)
	{
		Players.TryAdd(player.Id, new PlayerInfo(player));
	}

	public void OnPlayerDisconnected(long id)
	{
		Players.Remove(id, out PlayerInfo playerInfo);
		playerInfo.Player.QueueFree();
	}

	public void OnPlayerCompletedLevel(long id, int level)
	{
		Players[id].Levels.Add(level);
	}

	public void OnPlayerDied(long id, int level)
	{
		//TODO telelportat the character at a position and reset velocities
		//Get the position they should teleport to using the level they died in
	}
}

public class PlayerInfo
{
	public long Score;
	public HashSet<int> Levels = new HashSet<int>();
	public Player Player;

	public PlayerInfo(Player player)
	{
		Player = player;
	}
}