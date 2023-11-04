using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Marbles.Scripts;

public partial class ClientServer : Node
{
    private const int PORT = 8910;
    private const int UPDATE_FREQUENCY = 10;
    private ENetMultiplayerPeer _peer;

    private double _timeElapsed;
    private double _lastUpdate;

    private PackedScene _playerScene;
    private PlayerData _playerData;

    public override void _Ready()
    {
        _playerData = (PlayerData)GetParent().FindChild("PlayerData");
        _playerScene = GD.Load<PackedScene>("Scenes/Player.tscn");
        _peer = new ENetMultiplayerPeer();

        Multiplayer.PeerConnected += Multiplayer_PeerConnected;
        Multiplayer.PeerDisconnected += Multiplayer_PeerDisconnected;
    }

    private void Multiplayer_PeerConnected(long id)
    {
        Player newPlayer = (Player)_playerScene.Instantiate();
        newPlayer.Id = id;
        FindParent("Node3D").AddChild(newPlayer);
        _playerData.OnPlayerConnected(newPlayer);
    }

    public override void _Process(double delta)
    {
        if (Multiplayer.MultiplayerPeer != null && Multiplayer.IsServer())
        {
            _timeElapsed += delta;

            if (_timeElapsed - _lastUpdate > 1.0 / UPDATE_FREQUENCY)
            {
                _lastUpdate = _timeElapsed;

                foreach (var (id, info) in _playerData.Players)
                {
                    var player = info.Player;
                    Rpc(nameof(UpdateClient), id, player.Position, player.Quaternion, player.LinearVelocity, player.AngularVelocity);
                }
            }
        }
    }

    public void Host()
    {
        GD.Print(_peer.CreateServer(PORT));
        Multiplayer.MultiplayerPeer = _peer;
        GD.Print($"Server: {Multiplayer.IsServer()}");
    }

    private void Multiplayer_PeerDisconnected(long id)
    {
        _playerData.OnPlayerDisconnected(id);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void UpdateClient(long id, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        if (id == Multiplayer.MultiplayerPeer.GetUniqueId())
            return;

        var player = _playerData.Players[id].Player;
        player.Position = position;
        player.Quaternion = rotation;
        player.LinearVelocity = velocity;
        player.AngularVelocity = angularVelocity;
    }

    public void Join(string ip)
    {
        Multiplayer.ConnectedToServer += Multiplayer_ConnectedToServer;
        Multiplayer.ConnectionFailed += Multiplayer_ConnectionFailed;
        
        GD.Print(_peer.CreateClient(ip, PORT));
        GD.Print($"Client: {!Multiplayer.IsServer()}");
        Multiplayer.MultiplayerPeer = _peer;
    }

    private void Multiplayer_ConnectionFailed()
    {
        Multiplayer.MultiplayerPeer = null;
    }

    private void Multiplayer_ConnectedToServer()
    {
        GD.Print("Connected");
    }
}
