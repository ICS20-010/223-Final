using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinObject : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 180;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
