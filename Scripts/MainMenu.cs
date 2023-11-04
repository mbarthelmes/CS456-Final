using Godot;
using Marbles.Scripts;
using System;

public partial class MainMenu : Node2D
{
    private Control _rootMenu;

    private Control _playerSubMenu;

    private Control _joinSubmenu;
    private Button _connect;
    private Button _host;
    private Button _join;
    private Button _settings;
	private Button _exit;
    private Button _playerSubMenuBack;

    private TextEdit _textEdit;
    private Button _joinSubMenuBack;
    private OptionButton _ballOptions;
    private ClientServer _clientServer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _join = (Button)FindChild("Host");
        _join.Pressed += _host_Pressed;

        _join = (Button)FindChild("Join");
        _join.Pressed += _join_Pressed;

        _textEdit = (TextEdit)FindChild("IP");

        _connect = (Button)FindChild("Connect");
        _connect.Pressed += _connect_Pressed;

        _exit = (Button)FindChild("Exit");
        _exit.Pressed += _exit_Pressed;

        _joinSubmenu = (Control)FindChild("JoinSubMenu");
        _joinSubmenu.Visible = false;

        _rootMenu = (Control)FindChild("RootMenu");
        _rootMenu.Visible = true;

        _settings = (Button)FindChild("Settings");
        _settings.Pressed += _settings_Pressed;

        _joinSubMenuBack = (Button)_joinSubmenu.FindChild("Back");
        _joinSubMenuBack.Pressed += _joinSubMenuBack_Pressed;

        _playerSubMenu = (Control)FindChild("SettingsSubMenu");
        _playerSubMenu.Visible = false;

        _playerSubMenuBack = (Button)_playerSubMenu.FindChild("Back");
        _playerSubMenuBack.Pressed += _joinSubMenuBack_Pressed;

        _ballOptions = (OptionButton)_playerSubMenu.FindChild("OptionButton");
        _clientServer = ((ClientServer)FindParent("Node3D").FindChild("Session").FindChild("ClientServer"));
    }

    private void _settings_Pressed()
    {
        _playerSubMenu.Visible = true;
        _rootMenu.Visible = false;
    }

    private void _connect_Pressed()
    {
        _clientServer.Join(_textEdit.Text);
        Multiplayer.ConnectedToServer += Multiplayer_ConnectedToServer;
    }

    private void Multiplayer_ConnectedToServer()
    {
        Visible = false;
        Multiplayer.ConnectedToServer -= Multiplayer_ConnectedToServer;
        ((Player)FindParent("Node3D").FindChild("Player")).SetId(Multiplayer.MultiplayerPeer.GetUniqueId());
        Multiplayer.ServerDisconnected += Multiplayer_ServerDisconnected;
        _clientServer.SetMaterial(Multiplayer.MultiplayerPeer.GetUniqueId(), _ballOptions.GetItemText(_ballOptions.Selected));
    }

    private void Multiplayer_ServerDisconnected()
    {
        Multiplayer.MultiplayerPeer = null;
        Visible = true;
        Multiplayer.ServerDisconnected -= Multiplayer_ServerDisconnected;
    }

    private void _host_Pressed()
    {
        _clientServer.Host();
        _clientServer.SetMaterial(Multiplayer.MultiplayerPeer.GetUniqueId(), _ballOptions.GetItemText(_ballOptions.Selected));
        Visible = false;
    }

    private void _joinSubMenuBack_Pressed()
    {
        _joinSubmenu.Visible = false;
        _playerSubMenu.Visible = false;
        _rootMenu.Visible = true;
    }

    private void _join_Pressed()
    {
        _joinSubmenu.Visible = true;
        _rootMenu.Visible = false;
    }

    private void _exit_Pressed()
    {
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        _exit.Pressed -= _exit_Pressed;
        _join.Pressed -= _join_Pressed;
        _joinSubMenuBack.Pressed -= _joinSubMenuBack_Pressed;
        _connect.Pressed -= _connect_Pressed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
