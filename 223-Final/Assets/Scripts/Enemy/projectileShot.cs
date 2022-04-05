using System.Collections;
using UnityEngine;

public class projectileShot : MonoBehaviour {
  [SerializeField] private Rigidbody rb;
  private float shotSpeed = 25f;

  public void Shoot()
  {
    rb.AddForce(Vector3.forward * shotSpeed, ForceMode.Impulse);
    StartCoroutine(waitDestroy());
  }

  private IEnumerator waitDestroy()
  {
    Debug.Log("Destroyed Projectile");
    yield return new WaitForSeconds(1f);
    Destroy(this.gameObject);
  }
}