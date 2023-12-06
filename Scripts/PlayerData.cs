using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;

public partial class PlayerData : Node
{
	public Dictionary<long, PlayerInfo> Players = new Dictionary<long, PlayerInfo>();
	private Levels _levels;
    private Node3D _player;
    private Node2D _GameOver;

    [Export] 
    public SceneTreeTimer timer;
    public bool startTime = false;
    public SignalAwaiter signal;

    public override void _Ready()
    {
        _levels = (Levels)GetParent().FindChild("Levels");
        _player = (Node3D)FindParent("Node3D").FindChild("Player");
        _GameOver = (Node2D)_player.FindChild("GameOver");

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
		((Label)FindParent("Node3D").FindChild("Score")).Text = ": " + Players[id].Score.ToString();
    }

    public void SomeFunction(SceneTreeTimer timer)
    {
        ((Label)FindParent("Node3D").FindChild("TimeLbl")).Text = "Timer: " + timer.TimeLeft;

    }
    public void OnTriggerZone(long id)
	{
        ((Label)FindParent("Node3D").FindChild("AccLbl")).Visible = false;
        ((Label)FindParent("Node3D").FindChild("SecLbl")).Visible = false;
        ((Label)FindParent("Node3D").FindChild("PrvLbl")).Visible = false;
        ((Label)FindParent("Node3D").FindChild("DatLbl")).Visible = false;
        if (id == 0)		// Accessibility
		{
            ((Label)FindParent("Node3D").FindChild("AccLbl")).Visible = true;

            ((Label)FindParent("Node3D").FindChild("TimeLbl")).Visible = true;

            if ( startTime == false)
            {
                timer = GetTree().CreateTimer(60.0f, true, true, true);
                signal = ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
                startTime = true;
            }
            

		}
		else if (id == 1)	// Security
		{
            ((Label)FindParent("Node3D").FindChild("SecLbl")).Visible = true;
        }
		else if (id == 2)	// Privacy
		{
            ((Label)FindParent("Node3D").FindChild("PrvLbl")).Visible = true;
        }
		else if (id == 3)	// Data
		{
			((Label)FindParent("Node3D").FindChild("DatLbl")).Visible = true;
		}
    }

	public void OnPlayerDied(long id, int level)
	{
		var player = Players[id].Player;
		player.Position = _levels.Spawns[level].Position;
		player.LinearVelocity = Vector3.Zero;
		player.AngularVelocity = Vector3.Zero;
    }

    public void OnTimeOut()
    {
        if (_GameOver != null)
        {
            _GameOver.Visible = true;
        }
        
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