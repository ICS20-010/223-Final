using UnityEngine;

public class EnemyBase : MonoBehaviour {

  // Basic base calss setting state to NONE by default
  protected EnemyState state = EnemyState.AIMLESS;

  // Called by implementation of state logic to use as default
  // to ignore extra states...
  protected void noState(EnemyState defaultState)
  {
    state = defaultState;
  }
}