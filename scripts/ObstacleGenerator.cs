using Godot;
using System;
using Godot.Collections;


public partial class ObstacleGenerator : Node2D
{
  [Export]
  private SpawnProbabilities spawnProbabilities;
  private  Dictionary<string, float> probaDict;
  private static Array<int> _laneXs =  GameParameters.laneXs.Duplicate();
  private static int ySpawnLine = -20;

  private static PackedScene fence = GD.Load<PackedScene>("res://scenes/obstacles/fence.tscn");
  private static Dictionary<string, PackedScene> consumableScenes = new Dictionary<string, PackedScene>
    {
        { "Beer", GD.Load<PackedScene>("res://scenes/obstacles/consumables/beer.tscn") },
        { "Weed", GD.Load<PackedScene>("res://scenes/obstacles/consumables/weed.tscn") },
        { "Baggie", GD.Load<PackedScene>("res://scenes/obstacles/consumables/baggie.tscn") },
        { "Shroom", GD.Load<PackedScene>("res://scenes/obstacles/consumables/shroom.tscn") }
    };
  private static int _fenceCounter = 0;
  private Timer waveTimer = new Timer();
	public override void _Ready()
	{
    probaDict = spawnProbabilities.GetProbabilitiesDictionary();
    AddChild(waveTimer);
    waveTimer.WaitTime = 3;
    waveTimer.Start();
    waveTimer.Timeout += SpawnLine;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

  private static Func<int,float, bool> ShouldSpawnFence = (counter,fenceProba) => (GD.Randf() < fenceProba) && (_fenceCounter < _laneXs.Count - 1) ? true :  false;
  private static Func<float,bool> ShouldSpawnConsumable = consumableProba => GD.Randf() < consumableProba ? true : false;
  private string GetRandomConsumable()
    {
        float total = 0;
        foreach (float proba in probaDict.Values)
        {
            total += proba;
        }

        float randomPoint = GD.Randf() * total;

        foreach (var kvp in probaDict)
        {
            if (randomPoint < kvp.Value)
                return kvp.Key;
            else
                randomPoint -= kvp.Value;
        }
        return null; // This should never happen if probabilities are set up correctly
    }
  private void SpawnObstacle(int xPos, int wallCounter, float fenceProba, float consumableProba)
  {
    BaseObstacle obstacle;
    if (ShouldSpawnFence(wallCounter, fenceProba))
    {
      obstacle = (Fence)fence.Instantiate();
      obstacle.Position = new Vector2(xPos,ySpawnLine);
      AddChild(obstacle);
      _fenceCounter ++;
    }
    else if (ShouldSpawnConsumable(consumableProba))
    {
      string consumableName = GetRandomConsumable();
      obstacle = (Consumable)consumableScenes[consumableName].Instantiate();
      obstacle.Position = new Vector2(xPos,ySpawnLine);
      AddChild(obstacle);
    }
  }
  private void SpawnLine()
  {
    _fenceCounter = 0;
    _laneXs.Shuffle();
    foreach (int laneX in _laneXs)
    {
      SpawnObstacle(laneX, _fenceCounter, spawnProbabilities.FenceProba, spawnProbabilities.ConsumableProba);
    }
  }
  }
