using Godot;

[GlobalClass]
public partial class BaseObstacle : Node2D
{
  [Export]
  private Area2D collisionArea;
  [Export]
  private AnimatedSprite2D _sprite;
  private float _speed = GameParameters.initialSpeed; // field
  public float Speed // Property
  {
    get { return _speed; }
    set { _speed = value; }
  }

	public override void _Ready()
	{
    collisionArea.BodyEntered += BodyEntered;
    Scale = new Vector2(GameParameters.scale,GameParameters.scale);
	}

	public override void _Process(double delta)
	{
    Position = new Vector2(Position.X, Position.Y + _speed * (float)delta);
	}
  protected virtual void BodyEntered(Node2D body)
  {
  }
}
