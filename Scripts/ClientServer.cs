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

    private Resource _resource;

    public override void _Ready()
    {
        _peer = new ENetMultiplayerPeer();
    }

    public void Host()
    {
        GD.Print(_peer.CreateServer(PORT));
        GD.Print($"Server: {Multiplayer.IsServer()}");
        Multiplayer.PeerConnected += Server_PeerConnected;

        Multiplayer.MultiplayerPeer = _peer;
    }

    private void Server_PeerConnected(long id)
    {
        RpcId(id, "Hello", new Vector3(1, 1, 0), new Vector3(9, 9, 9));
        GD.Print("Hello", new Vector3(1, 1, 0), new Vector3(9, 9, 9));
    }

    public struct PlayerPacket
    {
        public Vector3 Velocity = new Vector3(1, 0, 1);

        public PlayerPacket()
        {

        }
    }

    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void Hello(Vector3 position, Vector3 velocity)
    {
        GD.Print($"RPC: {position} {velocity}");
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
