using Godot;
using Godot.Collections;

public partial class GameParameters : Node
{
  public static Vector2 screenSize = new Vector2(720, 1280);
  public static Array<int> laneXs;
  public static float laneSize;
  public static int nLanes = 3;
  public static float scale;
  public static float initialSpeed = 500.0f;

  private GameParameters() // constructor for the GameParameters class method that is called when an instance of the class is created
  {
    laneSize = (screenSize.X/(nLanes*2)) * 2;
    laneXs = new Array<int>();
    scale = 3.0f/nLanes;
    {
        for (int i = 0; i < nLanes; i++)
      {
          laneXs.Add((int)((i + 0.5f) * laneSize));//[i] = (int)((i + 0.5f) * laneSize);
      }
    };
  }
}
