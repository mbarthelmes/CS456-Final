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
        ((Label)FindParent("Node3D").FindChild("TimeLbl")).Text = "Timer: " + Math.Round(timer.TimeLeft, 2);

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
                timer = GetTree().CreateTimer(180.0f, true, true, true);
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
        else if (id == 4)   // End Game
        {
            if (_GameOver != null && _GameOver.Visible == false)
            {
                ((Label)FindParent("Node3D").FindChild("TimeLbl")).Visible = false;
                ((Label)FindParent("Node3D").FindChild("AppWin")).Visible = true;
                _GameOver.Visible = true;
            }
        }
    }

    public void LevelComplete(long level)
    {
        switch(level)
        {
            case 11:    // AccLose
                ((Label)FindParent("Node3D").FindChild("AccLose")).Visible = true;
                break;
            case 12:    // AccWin
                ((Label)FindParent("Node3D").FindChild("AccWin")).Visible = true;
                break;
            case 13:    // SecLose
                ((Label)FindParent("Node3D").FindChild("SecLose")).Visible = true;
                break;
            case 14:    // SecWin
                ((Label)FindParent("Node3D").FindChild("SecWin")).Visible = true;
                break;
            case 15:    // PriWin
                ((Label)FindParent("Node3D").FindChild("PriWin")).Visible = true;
                break;
            case 16:    // PriLose
                ((Label)FindParent("Node3D").FindChild("PriLose")).Visible = true;
                break;
            case 17:    // DatWin
                ((Label)FindParent("Node3D").FindChild("DatWin")).Visible = true;
                break;
            case 18:    // DatLose
                ((Label)FindParent("Node3D").FindChild("DatLose")).Visible = true;
                break;
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
        if (_GameOver != null && _GameOver.Visible == false)
        {
            ((Label)FindParent("Node3D").FindChild("AccLbl")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("SecLbl")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("PrvLbl")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("DatLbl")).Visible = false;

            ((Label)FindParent("Node3D").FindChild("AppLose")).Visible = true;
            ((Label)FindParent("Node3D").FindChild("TimeLose")).Visible = true;
            ((Label)FindParent("Node3D").FindChild("TimeLbl")).Visible = false;

            ((Label)FindParent("Node3D").FindChild("AccWin")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("AccLose")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("SecWin")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("SecLose")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("PriWin")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("PriLose")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("DatWin")).Visible = false;
            ((Label)FindParent("Node3D").FindChild("DatLose")).Visible = false;
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