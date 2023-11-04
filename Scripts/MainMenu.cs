using Godot;
using System;

public partial class MainMenu : Node2D
{
    private Control _rootMenu;

    private Control _joinSubmenu;
    private Button _join;
	private Button _exit;

    private Button _joinSubMenuBack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _join = (Button)FindChild("Join");
        _join.Pressed += _join_Pressed;

        _exit = (Button)FindChild("Exit");
        _exit.Pressed += _exit_Pressed;

        _joinSubmenu = (Control)FindChild("JoinSubMenu");
        _joinSubmenu.Visible = false;
        _rootMenu = (Control)FindChild("RootMenu");
        _rootMenu.Visible = true;

        _joinSubMenuBack = (Button)_joinSubmenu.FindChild("Back");
        _joinSubMenuBack.Pressed += _joinSubMenuBack_Pressed;

    }

    private void _joinSubMenuBack_Pressed()
    {
        _joinSubmenu.Visible = false;
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
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
