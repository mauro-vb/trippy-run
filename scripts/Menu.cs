using Godot;
using System;

public partial class Menu : Control
{


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
    Button playButton = GetNode<Button>("PanelContainer/BoxContainer/PlayButton");
    Button quitButton = GetNode<Button>("PanelContainer/BoxContainer/QuitButton");
    playButton.Pressed += PlayButtonPressed;
    quitButton.Pressed += QuitButtonPressed;
	}

  private void PlayButtonPressed()
  {
    //GD.Print(GameParameters.ScreenSize);
    GetTree().ChangeSceneToFile("scenes/game.tscn");
  }
  private void QuitButtonPressed()
  {
    GetTree().Quit();
  }
}
