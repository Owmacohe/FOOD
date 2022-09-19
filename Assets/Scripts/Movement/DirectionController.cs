using System.Collections;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField]
    Transform arrow;
    [SerializeField]
    Transform strengthMeter;
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
    SoundEffectManager sound;
    IslandManager islands;
    
    bool isRotating, isArriving, isFadingOut, isShrinking;
    int rotationDirection;
    Vector3 tempDirection, direction;

    float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<SoundEffectManager>();
        islands = FindObjectOfType<IslandManager>();

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

        if (!direction.Equals(Vector3.zero))
        {
            Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
            boat.rotation = Quaternion.Slerp(boat.rotation, lookRotation, Time.deltaTime * rotationSpeed * 0.5f);   
        }
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

            StartCoroutine(StrengthMeterChange(true));
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

            StartCoroutine(StrengthMeterChange(false));
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
    
    public IEnumerator FadeArrow(bool fadeOut)
    {
        SpriteRenderer rend = arrow.GetComponentInChildren<SpriteRenderer>();
        Color temp = rend.color;

        if (fadeOut)
        {
            isFadingOut = true;
            
            while (rend.color.a > 0)
            {
                rend.color = new Color(temp.r, temp.g, temp.b, temp.a - 0.1f);
                temp = rend.color;

                yield return new WaitForSeconds(0.1f);
            }
            
            isFadingOut = false;
        }
        else
        {
            while (rend.color.a < 1 && !isFadingOut)
            {
                rend.color = new Color(temp.r, temp.g, temp.b, temp.a + 0.1f);
                temp = rend.color;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator StrengthMeterChange(bool grow)
    {
        if (grow)
        {
            while (strengthMeter.localPosition.z < 1.2f && !isShrinking)
            {
                strengthMeter.localPosition += Vector3.forward * 0.03f;
                
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            isShrinking = true;
            
            while (strengthMeter.localPosition.z > -1)
            {
                strengthMeter.localPosition -= Vector3.forward * 0.03f;
                
                yield return new WaitForSeconds(0.01f);
            }

            isShrinking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish") && islands.IsDeliveryTarget(other.gameObject))
        {
            isRotating = false;
            isArriving = true;

            Vector3 dir = (other.transform.position - transform.position).normalized;
            SetDirection(dir);
            
            rb.velocity = dir * forceSpeed;
            
            StartCoroutine(ArriveGently(3));
            StartCoroutine(FadeArrow(true));
            StartCoroutine(StrengthMeterChange(false));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform grandparent = collision.transform.parent.parent;
    
        if ((collision.gameObject.CompareTag("Finish") && islands.IsDeliveryTarget(collision.gameObject))
            || (grandparent != null && grandparent.gameObject.CompareTag("Finish") && islands.IsDeliveryTarget(grandparent.gameObject)))
        {
            isRotating = true;
            isArriving = false;
            rb.velocity = Vector3.zero;
            
            sound.Play();
            
            StartCoroutine(FadeArrow(false));
            
            islands.CompleteDelivery();
        }
    }
}