using System.Collections;
using UnityEngine;

public class ImaginaryFriend : MonoBehaviour
{
    public float detectionRange = 20f;
    public float chaseSpeed = 2f;
    public float chaseDuration = 5f;
    public Vector3 initialPosition;
    private Transform player;
    private bool isChasing;
    private float chaseTimer;
    private Rigidbody rb;
    public int damage = 3; // Dano causado ao jogador
    public bool isHiding; // Adicionado para verificar se o jogador está escondido

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isHiding)
        {
            StopChase();
            ReturnToInitialPosition();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            StartChase();
        }

        if (isChasing)
        {
            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0 || distanceToPlayer > detectionRange)
            {
                StopChase();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            ReturnToInitialPosition();
        }
    }

    void StartChase()
    {
        isChasing = true;
        chaseTimer = chaseDuration;
    }

    public void StopChase()
    {
        isChasing = false;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
    }

    void ReturnToInitialPosition()
    {
        Vector3 direction = (initialPosition - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerTestController playerController = collision.gameObject.GetComponent<PlayerTestController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }
}
