using Godot;
using System.Linq;


namespace Project_Ice_Room.Scriptbin;
public partial class Creature : CharacterBody2D {

    [Export]
    public int hp = 1;
    [Export]
    protected int speed = 1;

    private bool facing_right = true;

    protected AnimatedSprite2D sprite_player;
    protected bool sprite_done = false;

    protected Weapon whap;
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
        try {
            whap = (Weapon)FindChild("weapon");
        }
        catch {
            GD.Print("No weapon found");
        }
        hitbox = (Area2D)FindChild("hitbox");
        direction = Vector2.Zero;

        Die = new(DieStart);
    }

    public override void _Process(double delta) {
        base._Process(delta);
        ZIndex = (int)GlobalPosition.Y;
        current_state.Call(ref current_state);
        CheckForHit();
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        FlipFlop();
        MoveAndSlide();
    }
    private void FlipFlop() {
        if (direction.X < 0 && facing_right) {
            this.Scale = new Vector2(-1, 1);
            facing_right = false;
        }
        else if (direction.X > 0 && !facing_right) {
            this.Scale = new Vector2(1, 1);
            facing_right = true;
        }
    }


    protected Weapon enemy_weapon = null;
    protected virtual void CheckForHit() {
        if (enemy_weapon != null) {
            if (!enemy_weapon.is_dangerous) {
                enemy_weapon = null;
            }
        }
        else {
            var overlaps = hitbox.GetOverlappingAreas();
            if (overlaps.Count == 0) return;
            foreach (Area2D area in overlaps) {
                if (area.Name != "dmg_box") continue;
                if (!((Weapon)area.GetParent()).is_dangerous) continue;
                enemy_weapon = (Weapon)area.GetParent();
                hp -= enemy_weapon.dmg;
                GD.Print(hp);
            }
        }
    }
}
