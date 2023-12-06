using Godot;
using Marbles.Scripts;
using System;

public partial class GameOver : Node2D
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
       
        _exit = (Button)FindChild("Exit");
        _exit.Pressed += _exit_Pressed;

    }

    private void _settings_Pressed()
    {
        _playerSubMenu.Visible = true;
        _rootMenu.Visible = false;
    }

    private void _exit_Pressed()
    {
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        _exit.Pressed -= _exit_Pressed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
