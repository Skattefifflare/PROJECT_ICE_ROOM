using Godot;
using System;

public partial class InventoryUI : Control
{
    bool isOpen;
    public override void _Ready()
    {
        Visible = false;
        isOpen = false;

        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_inv"))
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        base._Process(delta);
    }

    private void Open()
    {
        Visible = true;
        isOpen = true;
    }
    private void Close()
    {
        Visible = false;
        isOpen = false;
    }
}
