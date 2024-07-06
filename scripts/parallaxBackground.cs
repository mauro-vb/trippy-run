using Godot;
using System;

public partial class parallaxBackground : ParallaxBackground
{
  [Export]
  private float speed = GameParameters.initialSpeed;
	public override void _Process(double delta)
	{
    ScrollOffset = new Vector2(ScrollOffset.X, (float)(ScrollOffset.Y + speed * delta));
	}
}
