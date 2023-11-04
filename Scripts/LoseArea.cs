using Godot;
using System;

public partial class LoseArea : Area3D
{
    [Export]
    public int Level;

    private PlayerData _playerData;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _playerData = (PlayerData)FindParent("Node3D").FindChild("Session").FindChild("PlayerData");
        BodyShapeEntered += LoseArea_BodyShapeEntered;
	}

    private void LoseArea_BodyShapeEntered(Rid bodyRid, Node3D body, long bodyShapeIndex, long localShapeIndex)
    {
        if(body is Player player)
        {
            _playerData.OnPlayerDied(player.Id, Level);
            GD.Print($"Player {player.Id} died in level {Level}");
        }
    }

    public override void _ExitTree()
    {
        BodyShapeEntered -= LoseArea_BodyShapeEntered;
    }
}
