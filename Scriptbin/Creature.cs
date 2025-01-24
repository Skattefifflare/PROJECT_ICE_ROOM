using Godot;


namespace Project_Ice_Room.Scriptbin;
public partial class Creature : CharacterBody2D {

    [Export]
    protected int hp = 1;
    [Export]
    protected int speed = 1;

    protected AnimatedSprite2D sprite_player;
    protected bool sprite_done = false;

    protected Weapon whap;
    protected Area2D feet;
    protected Area2D hitbox;
    protected Vector2 direction;
    protected StateHandler sh;

    internal State Walk;
    internal State Idle;

    public override void _Ready() {
        base._Ready();

        sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
        sprite_player.AnimationFinished += () => sprite_done = true;
        sprite_player.AnimationChanged += () => sprite_done = false;


        whap = (Weapon)FindChild("weapon");
        feet = (Area2D)FindChild("feet");
        hitbox = (Area2D)FindChild("hitbox");
        direction = Vector2.Zero;
        sh = new(sprite_player);


        Idle = new State(
            () => direction == Vector2.Zero,
            () => direction != Vector2.Zero,

            () => { return; },
            () => { Velocity = new(Mathf.MoveToward(Velocity.X, 0, (float)GetPhysicsProcessDeltaTime() * 1100), Mathf.MoveToward(Velocity.Y, 0, (float)GetPhysicsProcessDeltaTime()* 1100)); },
            () => { return; },

            "idle",
            false
        );
        Walk = new State(
            () => direction != Vector2.Zero,
            () => direction == Vector2.Zero,

            () => { return; },
            () => { Velocity = direction.Normalized() * speed; },
            () => { return; },

            "walk",
            true
        );
    }
        
    public override void _Process(double delta) {
        base._Process(delta);       
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        sh.Call();
        if (direction.X > 0) sprite_player.FlipH = true;
        else if (direction.X < 0) sprite_player.FlipH = false;
        MoveAndSlide();
    }
}

