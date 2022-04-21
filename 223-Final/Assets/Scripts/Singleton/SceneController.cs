using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
  // Controll scene from here - Manage entering and exiting rooms and activating and deactivating rooms
  [SerializeField] private GameObject[] rooms;
  [SerializeField] private GameObject playerPrefab;
  private RoomData currentRoom = null;

  void Awake()
  {
    Messenger<int>.AddListener(GameEvents.EXIT_ENTERED, moveToNextRoom);
    Messenger.AddListener(GameEvents.GAME_START, onGameStart);
    Messenger.AddListener(GameEvents.GAME_PAUSED, onPause);
    Messenger.AddListener(GameEvents.GAME_RESUMED, onResume);

    foreach (GameObject obj in rooms)
    {
      RoomData rd = obj.GetComponent<RoomData>();
      PlayerData pData = playerPrefab.GetComponentInChildren<PlayerData>();
      pData.playerAttributes.health = pData.playerAttributes.healthTotal;
      pData.playerAttributes.stamina = pData.playerAttributes.staminaTotal;
    }
  }

  void onPause()
  {
    Time.timeScale = 0;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  void onResume()
  {
    Time.timeScale = 1;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = false;
  }

  void onGameStart()
  {
    GameObject[] items = GameObject.FindGameObjectsWithTag("Pickup");
    GameObject[] exits = GameObject.FindGameObjectsWithTag("Exit");
    if (items.Length != 0)
    {
      foreach (GameObject item in items)
      {
        Destroy(item);
      }
    }
    if (exits.Length != 0)
    {
      foreach (GameObject exit in exits)
      {
        exit.GetComponent<exitTrigger>().resetExitCount();
        Destroy(exit);
      }
    }
    foreach (GameObject obj in rooms)
    {
      RoomData rd = obj.GetComponent<RoomData>();
      rd.cleanRoom();
      rd.initRoom();
      PlayerData pData = playerPrefab.GetComponentInChildren<PlayerData>();
      pData.playerAttributes.health = pData.playerAttributes.healthTotal;
      pData.playerAttributes.stamina = pData.playerAttributes.staminaTotal;
    }
  }

  void moveToNextRoom(int exitId)
  {
    if (exitId < rooms.Length)
    {
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player != null)
      {
        Destroy(player.transform.parent.gameObject);
      }
      if (currentRoom != null)
      {
        currentRoom.deinitEnemy();
      }
      currentRoom = rooms[exitId].GetComponent<RoomData>();
      Transform nextSpawn = currentRoom.spawnPoint;
      player = GameObject.Instantiate(playerPrefab, nextSpawn.position, Quaternion.identity);
      PlayerData pData = player.GetComponentInChildren<PlayerData>();

      pData.refreashUI();
      currentRoom.initEnemy(pData.getTarget());
    }
    else
    {
      Messenger.Broadcast(GameEvents.END);
    }
  }

  void OnDestroy()
  {
    Messenger<int>.RemoveListener(GameEvents.EXIT_ENTERED, moveToNextRoom);
    Messenger.RemoveListener(GameEvents.GAME_START, onGameStart);
    Messenger.RemoveListener(GameEvents.GAME_PAUSED, onPause);
    Messenger.RemoveListener(GameEvents.GAME_RESUMED, onResume);
  }
}
