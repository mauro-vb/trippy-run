using Godot;
using Godot.Collections;

[GlobalClass]
public partial class LanesHandler : Node2D
{
    [Export]
    private int initialLanes;

    // Backing field for the nLanes property
    private int _nLanes;

    // Public property for nLanes
    public int nLanes
    {
        get { return _nLanes; }
        set
        {
            if (_nLanes != value)
            {
                _nLanes = value;
                OnNumberOfLanesChanged();
            }
        }
    }

    public Array<int> laneXs;
    public float laneSize;
    public float scale;

    [Signal]
    public delegate void ChangedNumberOfLanesEventHandler();

    public override void _Ready()
    {
        if (initialLanes == 0)
        {
            GD.PrintErr("lanesHandler is missing initialLanes export.");
        }
        nLanes = initialLanes; // Use the property to initialize
    }

    private void OnNumberOfLanesChanged()
    {
        RecalculateLanes();
        EmitSignal(SignalName.ChangedNumberOfLanes);
    }

    private void RecalculateLanes()
    {
        laneSize = GameParameters.screenSize.X / (nLanes * 2) * 2;
        laneXs = new Array<int>();
        for (int i = 0; i < nLanes; i++)
        {
            laneXs.Add((int)((i + 0.5f) * laneSize));
        }
        GameParameters.scale = 3.0f / nLanes;
    }
}
