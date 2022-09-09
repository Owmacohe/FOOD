using System.Collections;
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
    
    bool isRotating, isArriving;
    int rotationDirection;
    Vector3 direction;

    float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        isRotating = true;
        rotationDirection = 1;
    }

    void Update()
    {
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            arrow.RotateAround(transform.position, Vector3.up, rotationSpeed * rotationDirection);

            if (!direction.Equals(Vector3.zero))
            {
                Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
                boat.rotation = Quaternion.Slerp(boat.rotation, lookRotation, Time.deltaTime * rotationSpeed * 0.5f);   
            }
        }
    }

    public void SetDirection()
    {
        SetDirection((arrow.position - transform.position).normalized);
    }
    
    void SetDirection(Vector3 dir)
    {
        if (!isArriving)
        {
            isRotating = false;
            startTime = Time.time;
        
            direction = dir;
            rotationDirection *= -1;   
        }
    }

    public void SetForce()
    {
        if (!isArriving)
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

    IEnumerator ArriveGently(float minimumSpeed)
    {
        for (float i = 0; i < 1; i += 0.1f)
        {
            rb.velocity /= 1.3f;

            if (rb.velocity.magnitude < (direction * minimumSpeed).magnitude)
            {
                rb.velocity = rb.velocity.normalized * minimumSpeed;
                break;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            isRotating = false;
            isArriving = true;

            Vector3 dir = (other.transform.position - transform.position).normalized;
            SetDirection(dir);
            
            rb.velocity = dir * forceSpeed;
            
            StartCoroutine(ArriveGently(3));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isRotating = true;
            isArriving = false;
            rb.velocity = Vector3.zero;
        }
    }
}