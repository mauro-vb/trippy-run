using Godot;
using System;

public partial class SwipeDetector : Node2D
{
    [Export]
    private int _swipeLength = 20;
    [Export]
    private int _yDisplacementThreshold = 10;
    private bool _swiping = false;
    private Vector2 _startPos;
    private Vector2 _currentPos;

    [Signal]
    public delegate void SwipedRightEventHandler();
    [Signal]
    public delegate void SwipedLeftEventHandler();

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("press"))
        {
            if (!_swiping)
            {
                _swiping = true;
                _startPos = GetGlobalMousePosition();
            }
        }

        if (Input.IsActionPressed("press"))
        {
            if (_swiping)
            {
                _currentPos = GetGlobalMousePosition();
                if (_startPos.DistanceTo(_currentPos) > _swipeLength)
                {
                    if (Math.Abs(_startPos.Y - _currentPos.Y) <= _yDisplacementThreshold)
                    {
                        if (_currentPos.X > _startPos.X)
                        {
                            EmitSignal(SignalName.SwipedRight);
                        }
                        else
                        {
                            EmitSignal(SignalName.SwipedLeft);
                        }
                        _swiping = false; // Reset swiping state after detecting a swipe
                    }
                }
            }
        }
        else
        {
            _swiping = false; // Reset swiping state if the action is no longer pressed
        }
    }
}
