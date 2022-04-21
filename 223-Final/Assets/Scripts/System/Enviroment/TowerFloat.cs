using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFloat : MonoBehaviour
{
  [SerializeField] private List<Transform> waypoints = new List<Transform>();
  [SerializeField] private float moveSpeed = 10f;
  [SerializeField] private float timeToWait = 1.5f;

  private bool moving = true;
  private Rigidbody rb;
  public List<CharacterController> cc = new List<CharacterController>();
  public int waypointIndex = 1;

  private void Awake()
  {
    rb = this.gameObject.AddComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;
    rb.interpolation = RigidbodyInterpolation.Interpolate;
    float magnitude = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex - 1].position);
    moveSpeed = magnitude / moveSpeed;
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      CharacterController otherCC = other.GetComponent<CharacterController>();
      if (!cc.Contains(otherCC))
      {
        cc.Add(otherCC);
      }
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      cc.Remove(other.GetComponent<CharacterController>());
    }
  }

  private void FixedUpdate()
  {
    // transition between spawn waypoints at the moveSpeed
    if (moving)
    {
      if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) > 1)
      {
        // rb.MovePosition((waypoints[waypointIndex].position) * moveSpeed * Time.deltaTime);
        Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPos);
        foreach (CharacterController item in cc)
        {
          item.Move(rb.velocity * Time.deltaTime);
        }
      }
      else
      {
        moving = false;
        StartCoroutine(waitAtPoint());
      }
    }
  }

  private IEnumerator waitAtPoint()
  {
    yield return new WaitForSeconds(timeToWait);
    waypointIndex += 1;
    if (waypointIndex >= waypoints.Count)
    {
      waypointIndex = 0;
    }
    moving = true;
  }
}
