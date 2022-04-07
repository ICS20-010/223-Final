using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookToPlayer : MonoBehaviour
{
    private Transform playerTransfrom;
    private Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = gameObject.GetComponentInParent<Transform>();
        playerTransfrom = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = parentTransform.position;
        transform.rotation = Quaternion.identity;
        transform.LookAt(playerTransfrom);
    }
}