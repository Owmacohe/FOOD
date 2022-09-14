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
    Vector3 tempDirection, direction;

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
        }
        
        Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
        boat.rotation = Quaternion.Slerp(boat.rotation, lookRotation, Time.deltaTime * rotationSpeed * 0.5f);
    }

    public void SetDirection()
    {
        SetDirection(arrow.forward);
    }
    
    void SetDirection(Vector3 dir)
    {
        if (!isArriving)
        {
            isRotating = false;
            startTime = Time.time;
        
            tempDirection = dir;
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

            direction = tempDirection;
            rb.velocity += direction * forceSpeed;   
        }
    }

    IEnumerator ArriveGently(float minimumSpeed)
    {
        for (float i = 0; i < 10; i++)
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
    
    IEnumerator FadeArrow(bool fadeOut)
    {
        SpriteRenderer rend = arrow.GetComponentInChildren<SpriteRenderer>();
        Color temp = rend.color;
        
        for (float i = 0; i < 10; i++)
        {
            if (fadeOut)
            {
                rend.color = new Color(temp.r, temp.g, temp.b, temp.a - 0.1f);
            }
            else
            {
                rend.color = new Color(temp.r, temp.g, temp.b, temp.a + 0.1f);
            }

            temp = rend.color;

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
            StartCoroutine(FadeArrow(true));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isRotating = true;
            isArriving = false;
            rb.velocity = Vector3.zero;
            
            StartCoroutine(FadeArrow(false));
        }
    }
}