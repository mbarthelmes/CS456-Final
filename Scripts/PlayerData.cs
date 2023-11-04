using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

public partial class PlayerData : Node
{
	public Dictionary<long, PlayerInfo> Players = new Dictionary<long, PlayerInfo>();
	private Levels _levels;

    public override void _Ready()
    {
        _levels = (Levels)GetParent().FindChild("Levels");
    }

    public void OnPlayerConnected(Player player)
	{
		Players.TryAdd(player.Id, new PlayerInfo(player));
	}

	public void OnPlayerDisconnected(long id)
	{
		Players.Remove(id, out PlayerInfo playerInfo);
		playerInfo.Player.QueueFree();
	}

	public void OnPlayerCompletedLevel(long id, int level, int nextLevel)
	{
        var player = Players[id].Player;
        Players[id].Levels.Add(level);
        player.Position = _levels.Spawns[nextLevel].GlobalPosition;
        player.LinearVelocity = Vector3.Zero;
        player.AngularVelocity = Vector3.Zero;
    }

	public void OnCollectableCollected(long id)
	{
		Players[id].Score++;
    }

	public void OnPlayerDied(long id, int level)
	{
		var player = Players[id].Player;
		player.Position = _levels.Spawns[level].Position;
		player.LinearVelocity = Vector3.Zero;
		player.AngularVelocity = Vector3.Zero;
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