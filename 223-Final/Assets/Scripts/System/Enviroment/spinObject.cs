using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinObject : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 180;
    [SerializeField] private bool rotateX = false;
    [SerializeField] private bool rotateY = false;
    [SerializeField] private bool rotateZ = false;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float zRotation = 0f;

    private void Start() {
        if(rotateX)
        {
            xRotation = rotateSpeed;
        }
        if(rotateY)
        {
            yRotation = rotateSpeed;
        }
        if(rotateZ)
        {
            zRotation = rotateSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xRotation * Time.deltaTime, yRotation * Time.deltaTime, zRotation * Time.deltaTime);
    }

    public void setRotations(bool x, bool y, bool z)
    {
        rotateX = x;
        rotateY = y;
        rotateZ = z;
    }
}
