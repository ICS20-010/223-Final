using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
  [SerializeField] private GameObject menuHeader;
  [SerializeField] private GameObject helpHeader;
  [SerializeField] private GameObject gameOverHeader;
  [SerializeField] private GameObject gameEndHeader;

  public void onStartPressed()
  {
    Messenger.Broadcast(GameEvents.GAME_START);
    Messenger<int>.Broadcast(GameEvents.EXIT_ENTERED, 0);
  }
  public void onHelpPressed()
  {
    // show a help window explaining the game idea
    menuHeader.SetActive(false);
    helpHeader.SetActive(true);
  }
  public void onHelpBackPressed()
  {
    menuHeader.SetActive(true);
    helpHeader.SetActive(false);
  }
  public void onRestartPressed()
  {
    Messenger.Broadcast(GameEvents.GAME_START);
    gameEndHeader.SetActive(false);
    gameOverHeader.SetActive(false);
    Messenger<int>.Broadcast(GameEvents.EXIT_ENTERED, 0);
  }
  public void onExitPressed()
  {
    Application.Quit(0);
  }
}
