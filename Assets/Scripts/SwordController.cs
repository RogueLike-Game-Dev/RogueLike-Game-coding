using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Vector3 target;

    private Vector3 normalizeDirection;
    private float speed = 8f;
    private float rotationsPerMinute = 100f;
    private EntityStats playerStats;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRb;
    private Vector2 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        normalizeDirection = (target - transform.position).normalized;
        playerStats = GameObject.Find("Player").GetComponent<EntityStats>();
        playerRb =  GameObject.Find("Player").GetComponent<Rigidbody2D>();
        playerPosition = GameObject.Find("Player").transform.position;
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += normalizeDirection * speed * Time.deltaTime;
        transform.Rotate(0f, 0f, 6.0f * rotationsPerMinute * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (playerMovement.bubbleShieldActive &&
                PlayerMovement.characterType.Equals(PlayerMovement.CharacterType.Esteros))
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                var collisionStats = this.gameObject.GetComponent<EntityStats>(); //Daca e colision cu ceva care da DMG 
                if (collisionStats != null)
                {
                    playerStats.Damage(collisionStats.DMG);
                    playerRb.velocity = Vector2.zero;
                    playerRb.inertia = 0;
                    playerRb.AddForce(playerPosition * collisionStats.knockBackStrength, ForceMode2D.Impulse);

                }
            }
        }

    }
}
