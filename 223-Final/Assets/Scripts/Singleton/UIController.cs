using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  [SerializeField] private TMPro.TMP_Text roomNumber;
  [SerializeField] private Slider healthSlider;
  [SerializeField] private Slider staminaSlider;
  [SerializeField] private TMPro.TMP_Text keyValue;
  [SerializeField] private TMPro.TMP_Text timerValue;
  [SerializeField] private TMPro.TMP_Text finalTimerValue;

  [SerializeField] private GameObject mainMenu;
  [SerializeField] private GameObject menuCamera;
  [SerializeField] private GameObject inGameUI;
  [SerializeField] private GameObject gameOverMenu;
  [SerializeField] private GameObject endMenu;

  private int keys = 0;
  private double timePassed = 0;

  private void Awake()
  {
    Messenger.AddListener(GameEvents.KEY_OBTAINED, onKeyGet);
    Messenger.AddListener(GameEvents.KEY_USED, onKeyUse);
    Messenger.AddListener(GameEvents.GAME_START, onGameBegins);
    Messenger.AddListener(GameEvents.TIMER_UPDATE, onTimePassed);
    Messenger.AddListener(GameEvents.PLAYER_DEAD, onPlayerDeath);
    Messenger.AddListener(GameEvents.END, onGameEnd);
    Messenger<int>.AddListener(GameEvents.EXIT_ENTERED, onRoomNumberChanged);
    Messenger<float>.AddListener(GameEvents.POINTS_GAINED, onTimeReduced);
    Messenger<float>.AddListener(GameEvents.HEALTH_CHANGED, onHealthChanged);
    Messenger<float>.AddListener(GameEvents.STAMINA_CHANGED, onStaminaChanged);
  }

  private void Start()
  {
    onRoomNumberChanged(0);
    healthSlider.value = 1;
    staminaSlider.value = 1;
  }

  void onGameBegins()
  {
    mainMenu.SetActive(false);
    inGameUI.SetActive(true);
    if (menuCamera != null)
    {
      Destroy(menuCamera);
    }
    Messenger.Broadcast(GameEvents.GAME_RESUMED);
  }

  void onGameEnd()
  {
    Messenger.Broadcast(GameEvents.GAME_PAUSED);
    finalTimerValue.SetText(System.TimeSpan.FromSeconds(timePassed).ToString(@"mm\:ss\.ff"));
    endMenu.SetActive(true);
    inGameUI.SetActive(false);
  }

  void onPlayerDeath()
  {
    Messenger.Broadcast(GameEvents.GAME_PAUSED);
    gameOverMenu.SetActive(true);
    inGameUI.SetActive(false);
  }

  void onHealthChanged(float value)
  {
    healthSlider.value = value;
  }

  void onStaminaChanged(float value)
  {
    staminaSlider.value = value;
  }

  void onRoomNumberChanged(int roomNumber)
  {
    this.roomNumber.SetText("" + (roomNumber + 1));
    onTimeReduced(30.0f);
  }

  void onKeyGet()
  {
    keys += 1;
    keyValue.SetText("" + keys);
  }

  void onKeyUse()
  {
    keys -= 1;
    keyValue.SetText("" + keys);
  }

  void updateTime()
  {
    timerValue.SetText(System.TimeSpan.FromSeconds(timePassed).ToString(@"mm\:ss\.ff"));
  }

  void onTimeReduced(float points)
  {
    float pointsInSecods = points / 5;
    timePassed -= pointsInSecods;
    updateTime();
  }

  private void onTimePassed()
  {
    timePassed += Time.deltaTime;
    updateTime();
  }

  private void OnDestroy()
  {
    Messenger.RemoveListener(GameEvents.KEY_OBTAINED, onKeyGet);
    Messenger.RemoveListener(GameEvents.KEY_USED, onKeyUse);
    Messenger.RemoveListener(GameEvents.GAME_START, onGameBegins);
    Messenger.RemoveListener(GameEvents.TIMER_UPDATE, onTimePassed);
    Messenger.RemoveListener(GameEvents.END, onGameEnd);
    Messenger<int>.RemoveListener(GameEvents.EXIT_ENTERED, onRoomNumberChanged);
    Messenger<float>.AddListener(GameEvents.POINTS_GAINED, onTimeReduced);
    Messenger<float>.RemoveListener(GameEvents.HEALTH_CHANGED, onHealthChanged);
    Messenger<float>.RemoveListener(GameEvents.STAMINA_CHANGED, onStaminaChanged);
  }
}
