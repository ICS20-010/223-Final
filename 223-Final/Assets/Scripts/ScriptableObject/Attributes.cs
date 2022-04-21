using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Attributes", menuName = "223-Final/Attributes", order = 1)]
public class Attributes : ScriptableObject {
  public int healthTotal = 120;
  public int health = 0;
  public int staminaTotal = 300;
  public int stamina = 0;
  public int regenAmount = 2;
  public bool regenStamina = true;
  public int keysHeld = 0;
}