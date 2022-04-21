using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
  public Attributes playerAttributes;
  [SerializeField] private Transform pTarget;
  private Task staminaTask;

  private void Awake()
  {
    // Add listeners to calculate health and stamina
    // and coroutine for regenerating stamina that is
    // stopped when stamina is consumed for 5 seconds
    // then refills
    Messenger<int>.AddListener(GameEvents.HEALTH_CONSUMED, onHealthConsumed);
    Messenger<int>.AddListener(GameEvents.PLAYER_HEALED, onHealthRestored);
    Messenger<int>.AddListener(GameEvents.STAMINA_CONSUMED, onStaminaConsumed);
    Messenger.AddListener(GameEvents.KEY_OBTAINED, onKeyObtained);
    Messenger.AddListener(GameEvents.KEY_USED, onKeyUsed);
    staminaTask = new Task(regenWait(), false);

    refreashUI();
  }

  public Transform getTarget()
  {
    return pTarget;
  }

  public void refreashUI()
  {
    Messenger<float>.Broadcast(GameEvents.HEALTH_CHANGED, ((float)playerAttributes.health / (float)playerAttributes.healthTotal));
    Messenger<float>.Broadcast(GameEvents.STAMINA_CHANGED, ((float)playerAttributes.stamina / (float)playerAttributes.staminaTotal));
  }

  public bool hasKey()
  {
    return playerAttributes.keysHeld > 0;
  }

  void onKeyObtained()
  {
    playerAttributes.keysHeld += 1;
  }

  void onKeyUsed()
  {
    playerAttributes.keysHeld -= 1;
  }

  void onHealthRestored(int healthRestored)
  {
    playerAttributes.health += healthRestored;
    if (playerAttributes.health > playerAttributes.healthTotal)
    {
      playerAttributes.health = playerAttributes.healthTotal;
    }
    float healthPercentage = (float)playerAttributes.health / (float)playerAttributes.healthTotal;
    Messenger<float>.Broadcast(GameEvents.HEALTH_CHANGED, healthPercentage);
  }

  void onHealthConsumed(int healthConsumed)
  {
    // update health and broadcast to Health_Changed the float of health / healthTotal
    playerAttributes.health -= healthConsumed;
    if (playerAttributes.health <= 0)
    {
      // dead
      Messenger.Broadcast(GameEvents.PLAYER_DEAD);
    }
    else
    {
      float healthPercentage = (float)playerAttributes.health / (float)playerAttributes.healthTotal;
      Messenger<float>.Broadcast(GameEvents.HEALTH_CHANGED, healthPercentage);
    }
  }

  void onStaminaConsumed(int staminaConsumed)
  {
    // update health and broadcast to Health_Changed the float of stamina / staminaTotal
    // set regenStamina to false and start to coRoutine, if it is running, restart it
    playerAttributes.stamina -= staminaConsumed;
    if (playerAttributes.stamina < 1)
    {
      playerAttributes.stamina = 1;
    }
    float staminaPercentage = (float)playerAttributes.stamina / (float)playerAttributes.staminaTotal;
    Messenger<float>.Broadcast(GameEvents.STAMINA_CHANGED, staminaPercentage);
    playerAttributes.regenStamina = false;
    if (staminaTask.Running)
    {
      staminaTask.Stop();
      staminaTask = new Task(regenWait());
    }
    else
    {
      staminaTask = new Task(regenWait());
    }
  }

  private IEnumerator regenWait()
  {
    //Wait for 3 seconds to continue regenerating stamina
    yield return new WaitForSeconds(1);
    playerAttributes.regenStamina = true;
  }

  private void FixedUpdate()
  {
    if (playerAttributes.regenStamina && playerAttributes.stamina != playerAttributes.staminaTotal)
    {
      playerAttributes.stamina += playerAttributes.regenAmount;
      if (playerAttributes.stamina > playerAttributes.staminaTotal)
      {
        playerAttributes.stamina = playerAttributes.staminaTotal;
      }
      float staminaPercentage = (float)playerAttributes.stamina / (float)playerAttributes.staminaTotal;
      Messenger<float>.Broadcast(GameEvents.STAMINA_CHANGED, staminaPercentage);
    }
  }

  private void OnDestroy()
  {
    Messenger<int>.RemoveListener(GameEvents.HEALTH_CONSUMED, onHealthConsumed);
    Messenger.RemoveListener(GameEvents.KEY_OBTAINED, onKeyObtained);
    Messenger<int>.RemoveListener(GameEvents.STAMINA_CONSUMED, onStaminaConsumed);
  }
}