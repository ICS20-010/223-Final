using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollerMotor : EnemyBase
{
  [SerializeField] private GameObject projectile;
  [SerializeField] private Transform shotSpawnpt;
  private Transform target;
  private NavMeshAgent agent;
  // Movement Speed
  private float shootSpeed = 10f;
  private float timeSinceLastShot = 0f;
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
    target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
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
    switch (state)
    {
      case EnemyState.AIMLESS: aimlessState(); break;
      case EnemyState.SPOTTED: chasingState(); break;
      case EnemyState.ATTACKING: attackingState(); break;
      default: noState(EnemyState.AIMLESS); break;
    }

    // Debug.DrawLine(transform.position, agent.destination, Color.red, 1.5f);
  }

  void aimlessState()
  {
    if (Vector3.Distance(transform.position, target.position) <= spottingDistance)
    {
      state = EnemyState.SPOTTED;
      agent.SetDestination(target.position);
    }
    agent.SetDestination(transform.position);
  }

  void attackingState()
  {
    float distanceTo = Vector3.Distance(transform.position, target.position);
    if(shootSpeed <= timeSinceLastShot)
    {
      GameObject shot = GameObject.Instantiate(projectile, shotSpawnpt.position, Quaternion.identity);
      projectileShot ps = shot.GetComponent<projectileShot>();
      ps.Shoot();
      timeSinceLastShot = 0;
    }
    else 
    {
      timeSinceLastShot += Time.deltaTime;
    }

    if (distanceTo <= shootingDistance && distanceTo > shootingDistance - 5)
    {
      transform.RotateAround(target.transform.position, Vector3.up, 45f * Time.deltaTime);
      Vector3 orbitOffset = target.transform.position + (transform.position - target.transform.position).normalized * 5f;
      agent.SetDestination(orbitOffset);
    }
    if (distanceTo >= shootingDistance)
    {
      state = EnemyState.SPOTTED;
    }
    if (distanceTo <= shootingDistance - 3f)
    {
      transform.rotation = Quaternion.LookRotation(transform.position - target.position);
      Vector3 moveTo = transform.position + transform.forward * 5f;
      agent.SetDestination(moveTo);
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
