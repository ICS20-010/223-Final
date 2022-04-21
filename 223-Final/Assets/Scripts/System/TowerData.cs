using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public List<GameObject> enemySpawnPrefabs;
    public bool hasExit;
    public List<ItemType> itemSpawnPrefabs;
    public List<Transform> enemySpawns;
    public List<Transform> itemSpawns;
}
