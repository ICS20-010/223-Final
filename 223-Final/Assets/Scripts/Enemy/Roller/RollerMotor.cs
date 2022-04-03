using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollerMotor : EnemyBase
{

  [SerializeField] private Transform target;

  private NavMeshAgent agent;

  // Movement Speed
  private float shootSpeed = 1.5f;

  private float spottingDistance = 30f;
  private float shootingDistance = 15f;

  /**
  *   Roller_Enemy movement script...
  *   The roller enemy will maintain a set distance from the player and
  *   move parallel with the player and shoot, They are destroyed on one hit.
  *   They move slower when moving away from the player then when the moving parallel
  *   Allowing the player to catchup faster.
  **/

  // Start is called before the first frame update
  void Start()
  {
    agent = gameObject.GetComponent<NavMeshAgent>();
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, shootingDistance);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, spottingDistance);
  }

  // Update is called once per frame
  void Update()
  {
    Debug.Log(state);
    switch (state)
    {
      case EnemyState.AIMLESS: aimlessState(); break;
      case EnemyState.SPOTTED: chasingState(); break;
      case EnemyState.ATTACKING: attackingState(); break;
      default: noState(EnemyState.AIMLESS); break;
    }
  }

  void aimlessState()
  {
    if (Vector3.Distance(transform.position, target.position) <= spottingDistance)
    {
      state = EnemyState.SPOTTED;
      agent.SetDestination(target.position);
    }
  }

  void attackingState()
  {

    transform.RotateAround(target.transform.position, Vector3.up, 45f * Time.deltaTime);
    Vector3 orbitOffset = target.transform.position + (transform.position - target.transform.position).normalized * 5f;
    agent.SetDestination(orbitOffset);
    if (Vector3.Distance(transform.position, target.position) >= shootingDistance)
    {
      state = EnemyState.SPOTTED;
    }
  }

  void chasingState()
  {
    agent.SetDestination(target.position);
    if (Vector3.Distance(transform.position, target.position) <= shootingDistance)
    {
      state = EnemyState.ATTACKING;
    }
    else if (Vector3.Distance(transform.position, target.position) >= spottingDistance)
    {
      state = EnemyState.AIMLESS;
    }
  }
}
