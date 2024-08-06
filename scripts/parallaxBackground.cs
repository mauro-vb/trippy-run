using Godot;

public partial class parallaxBackground : ParallaxBackground
{
  private float speed;
  [Export]
  private LanesHandler lanesHandler;

  public override void _Ready()
  {
    lanesHandler.ChangedNumberOfLanes += _Rescale;
    Scale = new Vector2(GameParameters.scale, GameParameters.scale);
    speed = GameParameters.initialSpeed;
  }
  public override void _Process(double delta)
	{
    float adjustedSpeed = speed / GameParameters.scale;
    ScrollOffset = new Vector2(ScrollOffset.X, (float)(ScrollOffset.Y + adjustedSpeed * delta));
	}

  private void _Rescale() {
    Vector2 newScale = new Vector2(GameParameters.scale, GameParameters.scale);
    Tween tween = GetTree().CreateTween();
    tween.TweenProperty(this, "scale", newScale, GameParameters.scaleTweenSpeed);
    speed = GameParameters.initialSpeed;

  }
}
