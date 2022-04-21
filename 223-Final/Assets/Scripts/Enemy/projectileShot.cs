using System.Collections;
using UnityEngine;

public class projectileShot : MonoBehaviour
{
  [SerializeField] private Rigidbody rb;
  [SerializeField] private float TTL = 5.0f;
  public int damage = 15;
  private float shotSpeed = 25f;

  public void Shoot()
  {
    rb.AddForce(transform.forward * shotSpeed, ForceMode.Impulse);
    StartCoroutine(waitDestroy());
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.tag != "Projectile")
    {
      Destroy(this.gameObject);
    }
  }

  private IEnumerator waitDestroy()
  {
    Debug.Log("Destroyed Projectile");
    yield return new WaitForSeconds(TTL);
    Destroy(this.gameObject);
  }
}