using Godot;
using Godot.Collections;

[GlobalClass]
public partial class SpawnProbabilities : Resource
{
    [Export]
    public float FenceProba { get; set; }
    [Export]
    public float ConsumableProba { get; set; }
    [Export]
    public float BeerProba { get; set; }
    [Export]
    public float WeedProba { get; set; }
    [Export]
    public float BaggieProba { get; set; }
    [Export]
    public float ShroomProba { get; set; }

    // Method to get the dictionary of probabilities
    public Dictionary<string, float> GetProbabilitiesDictionary()
    {
        return new Dictionary<string, float>
        {
            { "Beer", BeerProba },
            { "Weed", WeedProba },
            { "Baggie", BaggieProba },
            { "Shroom", ShroomProba }
        };
    }
}
