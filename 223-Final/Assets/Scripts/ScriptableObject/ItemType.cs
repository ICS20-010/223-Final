using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ItemType", menuName = "223-Final/Item", order = 0)]
public class ItemType : ScriptableObject
{
  // Define some variables that dictate item behaviour when
  // in the world and interaction with the player.
  public string item_name = "";
  public string item_tag = "Untagged";
  public GameObject item_model = null;
  public Vector3 scale = Vector3.zero;
  public Vector3 spawnOffset = Vector3.zero;
  public bool rotates = false;
  public bool pickup = false;
  public bool interactable = false;
  public bool x = false;
  public bool y = false;
  public bool z = false;
}