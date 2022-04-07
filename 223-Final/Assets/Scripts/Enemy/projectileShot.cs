using System.Collections;
using UnityEngine;

public class projectileShot : MonoBehaviour {
  [SerializeField] private Rigidbody rb;
  [SerializeField] private float TTL = 5.0f;
  private float shotSpeed = 25f;

  public void Shoot()
  {
    rb.AddForce(Vector3.forward * shotSpeed, ForceMode.Impulse);
    StartCoroutine(waitDestroy());
  }

  private IEnumerator waitDestroy()
  {
    Debug.Log("Destroyed Projectile");
    yield return new WaitForSeconds(TTL);
    Destroy(this.gameObject);
  }
}