using Godot;
using System;

public partial class CollectableArea : Area3D
{
    private PlayerData _playerData;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _playerData = (PlayerData)FindParent("Node3D").FindChild("Session").FindChild("PlayerData");
        BodyShapeEntered += WinArea_BodyShapeEntered;
	}

    private void WinArea_BodyShapeEntered(Rid bodyRid, Node3D body, long bodyShapeIndex, long localShapeIndex)
    {
        if(body is Player player && player.Id == Multiplayer.MultiplayerPeer.GetUniqueId())
        {
            _playerData.OnCollectableCollected(player.Id);
            QueueFree();
        }
    }

    public override void _ExitTree()
    {
        BodyShapeEntered -= WinArea_BodyShapeEntered;
    }
}
