using System;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField]
    Transform arrow;
    [SerializeField]
    Transform boat;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float forceSpeed;
    [SerializeField]
    float forceSpeedMax;
    [SerializeField]
    float forceSpeedModifier;

    Rigidbody rb;
    
    bool isRotating;
    Vector3 direction;

    float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        isRotating = true;
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            arrow.RotateAround(transform.position, Vector3.up, rotationSpeed);

            if (!direction.Equals(Vector3.zero))
            {
                Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
                boat.rotation = Quaternion.Slerp(boat.rotation, lookRotation, Time.deltaTime * rotationSpeed * 0.5f);   
            }
        }
    }

    public void SetDirection()
    {
        isRotating = false;
        startTime = Time.time;
        
        direction = (arrow.position - transform.position).normalized;
    }

    public void SetForce()
    {
        isRotating = true;
        forceSpeed = (Time.time - startTime) * forceSpeedModifier;

        if (forceSpeed > forceSpeedMax)
        {
            forceSpeed = forceSpeedMax;
        }
        
        rb.velocity += direction * forceSpeed;
    }
}