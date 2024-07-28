using Godot;
using System;
using Godot.Collections;
public partial class Player : CharacterBody2D
{
    private bool _canTeleport = false;
    private bool _teleporting = false;
    private PackedScene _cloud =  GD.Load<PackedScene>("res://scenes/obstacles/smoke_cloud.tscn");
    private bool _isConfused = false;
    private int _laneIndex; // Player's lane field
    private float _laneSize;
    private Array<int> _laneXs; // lanes from GameParameters
    public int LaneIndex // Player's lane Property
    {
        get { return _laneIndex; }
        set
        {
          // Ensure value stays within array bounds
          int finalValue = value;
          if (_canTeleport) // Handle TelePort Logic
          {
            if (value >= _laneXs.Count)
            {
              finalValue = value - _laneXs.Count;
            }
            if (value < 0)
            {
              finalValue = _laneXs.Count + value;
            }
          }
          _laneIndex = Mathf.Clamp(finalValue, 0, _laneXs.Count - 1);
        }
    }

    [Export]
    private LanesHandler lanesHandler;
    [Export]
    private PlayerStats playerStats;
    [Export]
    private AnimatedSprite2D _animatedSprite;
    private string _animationDirection;

    private float _delay;
    private bool _onBeer = false;
    private Timer _beerTimer = new Timer();
    private bool _onWeed = false;
    private Timer _weedTimer = new Timer();
    private bool _onBaggie = false;
    private Timer _baggieTimer = new Timer();
    private bool _onShroom = false;
    private Timer _shroomTimer = new Timer();

    private SwipeDetector mySwipeDetector;
    public override void _Ready()
    {
        if (lanesHandler ==null || playerStats == null) {GD.PrintErr("Player is missing export variables.");}
        _laneXs = lanesHandler.laneXs;
        _laneSize = lanesHandler.laneSize;
        LaneIndex = 1;
        Position = new Vector2(_laneXs[LaneIndex], 1000);
        mySwipeDetector = GetNode<SwipeDetector>("SwipeDetector");
        mySwipeDetector.SwipedLeft += SwipedLeft;
        mySwipeDetector.SwipedRight += SwipedRight;
        Scale = new Vector2(GameParameters.scale,GameParameters.scale);

        _delay = playerStats.BaseDelay;

        HadConsumable += HandleHadConsumable;
        HitFence += HandleHitFence;

        // consumable timers
        AddChild(_beerTimer);
        AddChild(_weedTimer);
        AddChild(_baggieTimer);
        AddChild(_shroomTimer);

        _weedTimer.Timeout += HandleWeedTimeout;
        _baggieTimer.Timeout += HandleBaggieTimeout;
        _shroomTimer.Timeout += HandleShroomTimeout;

        lanesHandler.ChangedNumberOfLanes += _LanesChanged;

    }

    // Lane Management
    private void _LanesChanged() {
      Scale = new Vector2(lanesHandler.scale, lanesHandler.scale);
    }

    // Obstacles
    // Fence effects
    [Signal]
    public delegate void HitFenceEventHandler();
    private void HandleHitFence()
    {
    }
    // Consumables effects
    [Signal]
    public delegate void HadConsumableEventHandler(string consumableName);
    private void HandleHadConsumable(string consumableName)
    {
      switch (consumableName)
      {
        case "Beer":
          HadBeer(); break;
        case "Weed":
          HadWeed(); break;
        case "Baggie":
          HadBaggie(); break;
        case "Shroom":
          HadShroom(); break;
        default: break;
      }
    }
    private async void HadBeer()
    {
      _delay += playerStats.DelayIncrement;
      await ToSignal(GetTree().CreateTimer(playerStats.AddedDelayDuration), "timeout");
      _delay -= playerStats.DelayReduction;
      _delay = Math.Max(_delay, playerStats.BaseDelay);
    }
    private void HadWeed()
    {
      _onWeed = true;
      _canTeleport = true;
      _delay = (float)Godot.Mathf.Min(1.2, _delay + playerStats.WeedDelayIncrement);
      _weedTimer.Start(timeSec:playerStats.WeedDuration);
    }
    private void HadBaggie()
    {
      _onBaggie = true;
      _baggieTimer.Start(timeSec:playerStats.BaggieDuration);
    }
    private  void HadShroom()
    {
      _isConfused = true;
      _onShroom = true;
      _shroomTimer.Start(timeSec:playerStats.ShroomDuration);
    }
    private void HandleWeedTimeout()
    {
      _weedTimer.Stop();
      _onWeed = false;
      _delay -= playerStats.WeedDelayIncrement;
      _canTeleport = false;
    }
    private void HandleBaggieTimeout()
    {
      _baggieTimer.Stop();
      _onBaggie = false;
    }
    private void HandleShroomTimeout()
    {
      _shroomTimer.Stop();
      _onShroom = false;
      _isConfused = false;
    }


    private void HandleAnimation()
    {
      if (!_laneXs.Contains((int)Position.X))
      {
        _animatedSprite.Play(_animationDirection);
      }
      else
      {
        _animatedSprite.Play("forward");
      }

    }
// Movement related
    private void SwipedLeft()
    {
      if (!_isConfused)
      {
        MoveLeft();
      }
      else
      {
        MoveRight();
      }
    }
    private void SwipedRight()
    {
      if (!_isConfused)
      {
        MoveRight();
      }
      else
      {
        MoveLeft();
      }
    }

    private int GetLaneSkip()
    {
      return !_onBaggie ? (GD.Randf() < playerStats.BaseSkipProbability ? 2 : 1) // if not on Baggie check Base skip proba
                        : (GD.Randf() < playerStats.BaggieSkipProbability ? 2 : 1); // else check Baggie skip proba
    }
    private void MoveLeft()
    {
      int _nLanesMoved = GetLaneSkip();
      if (_laneXs.Contains((int)Position.X) && !_teleporting)
      {
        _animationDirection = "left";
        LaneIndex-=_nLanesMoved;
        if (_canTeleport){
          TeleportPosition();
        }
        else{
          TweenTweenPosition(_nLanesMoved);
        }
      }
      MoveAndSlide();
    }
    private void MoveRight()
    {
      int _nLanesMoved = GetLaneSkip();
      if (_laneXs.Contains((int)Position.X) && !_teleporting)
      {
        _animationDirection = "right";
        LaneIndex+=_nLanesMoved;
        if (_canTeleport){
          TeleportPosition();
        }
        else{
          TweenTweenPosition(_nLanesMoved);
        }
      }
      MoveAndSlide();
    }
    private void TweenTweenPosition(int nLanesMoved)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", new Vector2(_laneXs[LaneIndex], Position.Y), _delay * nLanesMoved);
    }
    private async void TeleportPosition()
    {
      _teleporting = true;

      var cloud = _cloud.Instantiate();
      AddChild(cloud);
      Tween tween = GetTree().CreateTween();
      tween.TweenProperty(cloud, "scale", new Vector2(.3f,.3f), _delay);
      tween.TweenProperty(cloud, "scale", new Vector2(0,0), _delay);
      await ToSignal(GetTree().CreateTimer(_delay), "timeout");
      Position = new Vector2(_laneXs[LaneIndex], Position.Y);
      _teleporting = false;
      await ToSignal(tween, "finished");
      cloud.QueueFree();
    }

  public override void _Process(double delta)
  {
    base._Process(delta);
    HandleAnimation();
  }
}
