using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    
    // Movement Speed
    private float speed = 3.5f;
    // how often it shoots
    private float shootSpeed = 1.5f;

    private float spottingDistance = 30f;
    private float shootingDistance = 15f;

    private EnemyState rollerState = EnemyState.NONE;

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
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spottingDistance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestDistanceState() 
    {

    }
}
