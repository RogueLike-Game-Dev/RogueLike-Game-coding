using UnityEngine;

public class FallingPlatformController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;
    private float dropAfter = 1.0f;
    private float respawnAfter = 5.0f;
    
    private void Start() 
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = transform.GetChild(0).GetComponent<CapsuleCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.name.Equals("Player")) 
        {
            Invoke("DropPlatform", dropAfter);
            Invoke("ResetPlatform", respawnAfter);
        }
    }
    
    void DropPlatform() 
    {
        rb.isKinematic = false;
        capsuleCollider2D.isTrigger = true;
    }

    void ResetPlatform() 
    {
        rb.isKinematic = true;
        capsuleCollider2D.isTrigger = false;
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;
    }
}
