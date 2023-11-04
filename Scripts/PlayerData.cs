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
}

public class PlayerInfo
{
	public long Score;
	public int Level;
}