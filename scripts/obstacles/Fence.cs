using Godot;
using System;

public partial class Fence : BaseObstacle
{
  protected override async void BodyEntered(Node2D body)
  {
    if (body is Player)
    {
      body.EmitSignal(Player.SignalName.HitFence);
      Speed = 0.0f;
      FlyAway();
      await ToSignal(GetTree().CreateTimer(3),"timeout");
      QueueFree();
    }

  }

  private void FlyAway()
  {
    Tween tween = GetTree().CreateTween();
    tween.TweenProperty(this, "position", new Vector2((float)GD.RandRange(-300,GameParameters.screenSize.X + 300), -100), 2)
    .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
    tween.Parallel().TweenProperty(this,"rotation", Rotation + 10, 1).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
  }
}
