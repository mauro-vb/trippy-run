using Godot;
using System;
[GlobalClass]
public partial class PlayerStats : Resource
{
    // Beer and delay related stats
    [Export]
    public float BaseDelay { get; set; }

    [Export]
    public float DelayIncrement { get; set; }

    [Export]
    public float DelayReduction { get; set; }

    [Export]
    public float AddedDelayDuration { get; set; }

    // Weed and cloud related stats
    [Export]
    public float WeedDelayIncrement { get; set; }
    [Export]
    public float WeedDuration { get; set; }

    [Export]
    public float CloudBaseSize { get; set; }

    [Export]
    public float CloudSizeReduction { get; set; }

    // Baggie and skip related stats
    [Export]
    public float BaseSkipProbability { get; set; }

    [Export]
    public float BaggieDuration { get; set; }

    [Export]
    public float BaggieSkipProbability { get; set; }

    // Shroom and trip related stats
    [Export]
    public float ShroomDuration { get; set; }

    // Other
    [Export]
    public float LitDuration { get; set; }
}
