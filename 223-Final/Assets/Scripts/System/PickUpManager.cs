using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
  [SerializeField] private ItemType keyType;
  [SerializeField] private ItemType healthType;

  private float pointValue = 10.0f;

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Pickup")
    {
      Item item;
      if (other.TryGetComponent<Item>(out item))
      {
        if (keyType.Equals(item.itemType))
        {
          Messenger.Broadcast(GameEvents.KEY_OBTAINED);
          Messenger<float>.Broadcast(GameEvents.POINTS_GAINED, pointValue);
        }
        if (healthType.Equals(item.itemType))
        {
          Messenger<float>.Broadcast(GameEvents.POINTS_GAINED, pointValue);
          HealthData hData;
          if (item.TryGetComponent<HealthData>(out hData))
          {
            int healthGained = hData.healthRestored;
            if (hData.isRegen)
            {
              float healDelta = 0;
              while (healDelta < hData.secondsToHeal)
              {
                float healing = healDelta / healthGained;
                Messenger<int>.Broadcast(GameEvents.PLAYER_HEALED, (int)healing);
              }
            }
            else
            {
              Messenger<int>.Broadcast(GameEvents.PLAYER_HEALED, healthGained);
            }
          }
        }
      }
      Destroy(other.gameObject);
    }
  }
}
