using Godot;
using Godot.Collections;

[GlobalClass]
public partial class LanesHandler : Node2D
{
    [Export]
    private int initialLanes;
    public int nLanes;
    public Array<int> laneXs;
    public float laneSize;
    public float scale;

    // public LanesHandler()
    // {
    // }

    public override void _Ready()
    {
        GD.Print(initialLanes);
        ChangeNumberOfLanes(initialLanes);
    }

    public void ChangeNumberOfLanes(int newNumberOfLanes)
    {
        nLanes = newNumberOfLanes;
        RecalculateLanes();
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
