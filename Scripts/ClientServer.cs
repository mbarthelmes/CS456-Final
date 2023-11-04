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
    private ENetMultiplayerPeer _peer;

    private PackedScene _playerScene;
    private PlayerData _playerData;
    public override void _Ready()
    {
        _playerData = (PlayerData)GetParent().FindChild("PlayerData");
        _playerScene = GD.Load<PackedScene>("Scenes/Player.tscn");
        _peer = new ENetMultiplayerPeer();
    }

    public override void _Process(double delta)
    {
        if (Multiplayer.IsServer())
        {
            foreach (var (id, info) in _playerData.Players)
            {
                var player = info.Player;
                Rpc(nameof(UpdateClient), id, player.Position, player.LinearVelocity);
            }
        }
    }

    public void Host()
    {
        GD.Print(_peer.CreateServer(PORT));
        GD.Print($"Server: {Multiplayer.IsServer()}");
        Multiplayer.PeerConnected += Server_PeerConnected;
        Multiplayer.PeerDisconnected += Multiplayer_PeerDisconnected;

        Multiplayer.MultiplayerPeer = _peer;
    }

    private void Multiplayer_PeerDisconnected(long id)
    {
        _playerData.OnPlayerDisconnected(id);
    }

    private void Server_PeerConnected(long id)
    {
        Player newPlayer = (Player)_playerScene.Instantiate();
        newPlayer.Id = id;
        FindParent("Node3D").AddChild(newPlayer);
        _playerData.OnPlayerConnected(newPlayer);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void UpdateServer(long id, Vector3 position, Vector3 velocity)
    {
        GD.Print($"RPC Server: {position} {velocity}");
        var player = _playerData.Players[id].Player;
        player.Position = position;
        player.LinearVelocity = velocity;
    }

    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void UpdateClient(long id, Vector3 position, Vector3 velocity)
    {
        GD.Print($"RPC Client: {position} {velocity}");
        var player = _playerData.Players[id].Player;
        player.Position = position;
        player.LinearVelocity = velocity;
    }

    public void Join(string ip)
    {
        Multiplayer.ConnectedToServer += Multiplayer_ConnectedToServer;

        GD.Print(_peer.CreateClient(ip, PORT));
        GD.Print($"Client: {!Multiplayer.IsServer()}");
        Multiplayer.MultiplayerPeer = _peer;
    }

    private void Multiplayer_ConnectedToServer()
    {
        GD.Print("Connected");
    }
}
