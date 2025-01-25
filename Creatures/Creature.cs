using Godot;


namespace Project_Ice_Room.Scriptbin;
public partial class Creature : CharacterBody2D {

    [Export]
    public int hp = 1;
    [Export]
    protected int speed = 1;

    protected AnimatedSprite2D sprite_player;
    protected bool sprite_done = false;

    protected Weapon whap;
    protected Area2D feet;
    protected Area2D hitbox;
    protected Vector2 direction;

    protected State current_state;

    protected State Die;
    protected void DieStart() {
        sprite_player.Play("die");
        Velocity = Vector2.Zero;
    }
    

    public override void _Ready() {
        base._Ready();

        sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
        sprite_player.AnimationFinished += () => sprite_done = true;
        sprite_player.AnimationChanged += () => sprite_done = false;

        whap = (Weapon)FindChild("weapon");
        feet = (Area2D)FindChild("feet");
        hitbox = (Area2D)FindChild("hitbox");
        direction = Vector2.Zero;

        hitbox.AreaEntered += (area) => {
            if (area.Name == "dmg_box") {
                hp -= area.GetParent<Weapon>().dmg;
                GD.Print(hp);
            }
        };

        Die = new(DieStart);
    }
    
    public override void _Process(double delta) {
        base._Process(delta);      
        current_state.Call(ref current_state);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        FlipFlop();
        MoveAndSlide();
    }
    private void FlipFlop() {
        if (direction.X > 0) sprite_player.FlipH = true;
        else if (direction.X < 0) sprite_player.FlipH = false;
    }
}

