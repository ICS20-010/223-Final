using UnityEngine;

public class EnemyBase : MonoBehaviour {

  // Basic base calss setting state to NONE by default
  protected EnemyState state = EnemyState.AIMLESS;
  protected lookToPlayer ltp;
  public Transform target;

  private void Awake() {
    ltp = GetComponentInChildren<lookToPlayer>();
  }

  public void setTarget(Transform target)
  {
    this.target = target;
    if(target == null)
    {
      this.gameObject.SetActive(false);
    } else {
      this.gameObject.SetActive(true);
    }
    ltp.setTarget(target);
  }

  // Called by implementation of state logic to use as default
  // to ignore extra states...
  protected void noState(EnemyState defaultState)
  {
    state = defaultState;
  }
}