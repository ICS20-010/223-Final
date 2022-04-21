using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
  [SerializeField] private GameObject itemPrefab;
  [SerializeField] private GameObject exitPrefab;
  private List<EnemyBase> enemyList = new List<EnemyBase>();
  public Transform spawnPoint { get; private set; }
  private static int roomNumber = 0;
  public int roomId { get; private set; } = 0;
  public TowerData[] towers;
  private List<Transform> usedSpawns = new List<Transform>();

  private void Awake()
  {
    roomId = roomNumber;
    roomNumber += 1;
    
    spawnPoint = GetComponentInChildren<spawnLocation>().transform;
  }

  public void cleanRoom()
  {
    if (enemyList.Count != 0)
    {
      foreach (EnemyBase baseEnemy in enemyList)
      {
        Destroy(baseEnemy.gameObject);
      }
    }
    usedSpawns.Clear();
  }

  public void initRoom()
  {
    // generate exits on towers, set exits to be locked, if locked add a key
    // 1 key per exit
    // must have 1 exit
    // an exit does not need a key
    // ---------------------------------------------------------------------
    // first get the towers in a room, then if exitCount < 1 set an exit
    // using above rules generate at least one exit and maybe a key for it.
    towers = GetComponentsInChildren<TowerData>();
    if (towers.Length == 0)
    {
      Debug.LogError("ERROR: RoomData failed to gather any towers");
      Debug.Break();
    }

    int exitTower = Random.Range(0, towers.Length - 1);
    int exitKey = Random.Range(0, towers.Length - 1);
    // if the tower is valid to ahve an exit
    while (!towers[exitTower].hasExit)
    {
      exitTower = Random.Range(0, towers.Length - 1);
    }
    // if the tower has one item spawn location AND exittower is the same as the exit key
    if (towers[exitTower].itemSpawns.Count == 1 && exitTower == exitKey)
    {
      // regenerate the exitKey till they are not equal
      while (exitTower == exitKey)
      {
        exitKey = Random.Range(0, towers.Length - 1);
      }
      GameObject exitObject = GameObject.Instantiate(exitPrefab, towers[exitTower].itemSpawns[0].position, Quaternion.identity);
      exitTrigger eTrigger = exitObject.GetComponent<exitTrigger>();
      usedSpawns.Add(towers[exitTower].itemSpawns[0]);

      if (exitTower % 2 == 0)
      {
        Item item = GameObject.Instantiate(itemPrefab, towers[exitKey].itemSpawns[0].position, Quaternion.identity).GetComponent<Item>();
        item.setItemType(towers[exitKey].itemSpawnPrefabs[0]);
        item.init();
        eTrigger.setLocked(true);
        usedSpawns.Add(towers[exitTower].itemSpawns[0]);
        item.gameObject.transform.parent = towers[exitKey].itemSpawns[0];
      }
      else
      {
        eTrigger.setLocked(false);
      }
    }
    else if (towers[exitTower].itemSpawns.Count > 1 && exitTower == exitKey)
    {
      int exitSpawn = Random.Range(0, towers[exitTower].itemSpawns.Count - 1);
      int keySpawn = Random.Range(0, towers[exitTower].itemSpawns.Count - 1);
      while (exitSpawn == keySpawn)
      {
        exitSpawn = Random.Range(0, towers.Length - 1);
      }

      GameObject exitObject = GameObject.Instantiate(exitPrefab, towers[exitTower].itemSpawns[exitSpawn].position, Quaternion.identity);
      exitTrigger eTrigger = exitObject.GetComponent<exitTrigger>();
      usedSpawns.Add(towers[exitTower].itemSpawns[exitSpawn]);

      if (exitTower % 2 == 0)
      {
        Item item = GameObject.Instantiate(itemPrefab, towers[exitKey].itemSpawns[keySpawn].position, Quaternion.identity).GetComponent<Item>();
        item.setItemType(towers[exitKey].itemSpawnPrefabs[0]);
        item.init();
        eTrigger.setLocked(true);
        usedSpawns.Add(towers[exitKey].itemSpawns[keySpawn]);
        item.gameObject.transform.parent = towers[exitKey].itemSpawns[0];
      }
      else
      {
        eTrigger.setLocked(false);
      }
    }
    else
    {
      GameObject exitObject = GameObject.Instantiate(exitPrefab, towers[exitTower].itemSpawns[0].position, Quaternion.identity);
      exitTrigger eTrigger = exitObject.GetComponent<exitTrigger>();
      usedSpawns.Add(towers[exitTower].itemSpawns[0]);

      if (exitTower % 2 == 0)
      {
        Item item = GameObject.Instantiate(itemPrefab, towers[exitKey].itemSpawns[0].position, Quaternion.identity).GetComponent<Item>();
        item.setItemType(towers[exitKey].itemSpawnPrefabs[0]);
        item.init();
        eTrigger.setLocked(true);
        usedSpawns.Add(towers[exitKey].itemSpawns[0]);
        item.gameObject.transform.parent = towers[exitKey].itemSpawns[0];
      }
      else
      {
        eTrigger.setLocked(false);
      }
    }

    foreach (TowerData tower in towers)
    {
      int enemyCount = Random.Range(1, tower.enemySpawns.Count);
      int itemCount = Random.Range(0, tower.itemSpawns.Count);
      foreach (Transform itemSpawn in tower.itemSpawns)
      {
        if (itemCount != 0 && !usedSpawns.Contains(itemSpawn))
        {
          int itemIndex = Random.Range(0, tower.itemSpawnPrefabs.Count);

          GameObject item_prefab = GameObject.Instantiate(itemPrefab, itemSpawn.position, Quaternion.identity);
          item_prefab.transform.parent = itemSpawn.transform;
          Item item = item_prefab.GetComponent<Item>();
          item.setItemType(tower.itemSpawnPrefabs[itemIndex]);
          item.init();
          itemCount -= 1;
        }
      }
      foreach (Transform enemySpawn in tower.enemySpawns)
      {
        if (enemyCount != 0 && tower.enemySpawnPrefabs.Count != 0)
        {
          GameObject enemy = GameObject.Instantiate(tower.enemySpawnPrefabs[Random.Range(0, tower.enemySpawnPrefabs.Count)], enemySpawn.position, Quaternion.identity);
          enemy.transform.parent = this.transform;
          enemy.SetActive(false);
          RollerMotor enemyRoller;
          BoperMotor enemyBoper;
          if (enemy.TryGetComponent<RollerMotor>(out enemyRoller))
          {
            enemyList.Add((EnemyBase)enemyRoller);
          }
          if (enemy.TryGetComponent<BoperMotor>(out enemyBoper))
          {
            enemyList.Add((EnemyBase)enemyBoper);
          }
          enemyCount -= 1;
        }
      }
    }
  }

  public void initEnemy(Transform playerTransfrom)
  {
    foreach (EnemyBase enemy in enemyList)
    {
      enemy.setTarget(playerTransfrom);
    }
  }
  public void deinitEnemy()
  {
    foreach (EnemyBase enemy in enemyList)
    {
      enemy.setTarget(null);
    }
  }

}