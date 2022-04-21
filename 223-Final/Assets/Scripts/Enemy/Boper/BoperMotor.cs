using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoperMotor : EnemyBase
{
  [SerializeField] private GameObject projectile;
  [SerializeField] private Transform shotSpawnpt;
  [SerializeField] private bool isStatic = false;
  private NavMeshAgent agent;
  // Movement Speed
  private float shootSpeed = 5f;
  private float timeSinceLastShot = 0f;
  private float spottingDistance = 50f;
  private float shootingDistance = 35f;
  private float minHeight = 3f;
  private float maxHeight = 6f;
  private float floatDir = 0f;

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
    if (agent != null || !isStatic)
    {
      agent = gameObject.GetComponent<NavMeshAgent>();
      agent.baseOffset = Random.Range(3f, 6f);
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (target != null && !isStatic)
    {
      switch (state)
      {
        case EnemyState.AIMLESS: aimlessState(); break;
        case EnemyState.SPOTTED: chasingState(); break;
        case EnemyState.ATTACKING: attackingState(); break;
        default: noState(EnemyState.AIMLESS); break;
      }
      offsetBase();
    }
    else
    {
      float distanceTo = Vector3.Distance(transform.position, target.position);
      if (shootSpeed <= timeSinceLastShot)
      {
        GameObject shot = GameObject.Instantiate(projectile, shotSpawnpt.position, Quaternion.identity);
        shot.transform.LookAt(target.transform);
        projectileShot ps = shot.GetComponent<projectileShot>();
        ps.Shoot();
        timeSinceLastShot = 0;
      }
      else
      {
        timeSinceLastShot += Time.deltaTime;
      }
    }
  }

  private void offsetBase()
  {
    if (agent.baseOffset <= minHeight)
    {
      floatDir = 0.5f;
    }
    if (agent.baseOffset >= maxHeight)
    {
      floatDir = -0.5f;
    }
    agent.baseOffset += floatDir * Time.deltaTime;
    minHeight = target.position.y + 3f;
    maxHeight = target.position.y + 6f;
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
    if (shootSpeed <= timeSinceLastShot)
    {
      GameObject shot = GameObject.Instantiate(projectile, shotSpawnpt.position, Quaternion.identity);
      shot.transform.LookAt(target.transform);
      projectileShot ps = shot.GetComponent<projectileShot>();
      ps.Shoot();
      timeSinceLastShot = 0;
    }
    else
    {
      timeSinceLastShot += Time.deltaTime;
    }
    if (!isStatic)
    {
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
        // transform.rotation = Quaternion.LookRotation(transform.position - target.position);
        Vector3 moveTo = transform.position + transform.forward * 5f;
        agent.SetDestination(moveTo);
      }
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
