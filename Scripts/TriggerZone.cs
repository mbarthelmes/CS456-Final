using Godot;
using System;

public partial class TriggerZone : Area3D
{
    [Export]
    public int Level;

    private PlayerData _playerData;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _playerData = (PlayerData)FindParent("Node3D").FindChild("Session").FindChild("PlayerData");
        BodyShapeEntered += WinArea_BodyShapeEntered;
	}

    private void WinArea_BodyShapeEntered(Rid bodyRid, Node3D body, long bodyShapeIndex, long localShapeIndex)
    {
        if(body is Player player)
        {
            SceneTreeTimer timer;
            _playerData.OnTriggerZone(Level);
            GD.Print($"Player {player.Id} accessed level {Level}.");
        }
    }

    public override void _ExitTree()
    {
        BodyShapeEntered -= WinArea_BodyShapeEntered;
    }
}
