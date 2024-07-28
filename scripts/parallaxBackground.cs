using Godot;

public partial class parallaxBackground : ParallaxBackground
{
  [Export]
  private float speed = GameParameters.initialSpeed;
  [Export]
  private LanesHandler lanesHandler;

  public override void _Ready()
  {
    lanesHandler.ChangedNumberOfLanes += _Rescale;
    Scale = new Vector2(GameParameters.scale, GameParameters.scale);
  }
  public override void _Process(double delta)
	{
    ScrollOffset = new Vector2(ScrollOffset.X, (float)(ScrollOffset.Y + speed * delta));
	}

  private void _Rescale() {
    Scale = new Vector2(GameParameters.scale, GameParameters.scale);
    speed = GameParameters.initialSpeed * lanesHandler.nLanes;
  }
}
