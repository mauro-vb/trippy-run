using Godot;

public partial class Consumable : BaseObstacle
{
    [Export]
    public string name;

    protected override void BodyEntered(Node2D body)
    {
        if (body is Player)
        {
            body.EmitSignal(Player.SignalName.HadConsumable, name);
            QueueFree();
        }
    }
}
