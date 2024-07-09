using UnityEngine;
using System.Collections;

public class ImaginaryFriend : MonoBehaviour
{
    public float detectionRange = 15f;
    public float chaseSpeed = 2f;
    public float holdDistance = 1f; // Distância para segurar o jogador
    public float chaseDuration = 5f;
    public Vector3 initialPosition;
    private Transform player;
    private Rigidbody rb;
    private bool isChasing;
    private bool isHoldingPlayer;
    private bool isStunned; // Verifica se o Amigo Imaginário está paralisado
    public int damage = 3; // Dano causado ao jogador
    public bool isHiding; // Verifica se o jogador está escondido

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;

        if (player == null)
        {
            Debug.LogError("Player not found. Ensure the player object has the tag 'Player'.");
        }
    }

    void Update()
    {
        if (isHiding || isHoldingPlayer || isStunned)
        {
            if (isHiding)
            {
                ReturnToInitialPosition();
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            StartChase();
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            ReturnToInitialPosition();
        }
    }

    void StartChase()
    {
        isChasing = true;
        Debug.Log("Amigo Imaginário começou a perseguir.");
    }

    public void StopChase()
    {
        isChasing = false;
        rb.velocity = Vector3.zero; // Parar o movimento
        Debug.Log("Amigo Imaginário parou de perseguir.");
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            StopChase();
            ReturnToInitialPosition();
            return;
        }

        if (distanceToPlayer <= holdDistance)
        {
            HoldPlayer();
        }
        else
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
        }
    }

    void HoldPlayer()
    {
        isChasing = false;
        isHoldingPlayer = true;
        rb.velocity = Vector3.zero; // Parar o movimento do inimigo
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = false; // Desativar controle do jogador
        StartCoroutine(HoldPlayerCoroutine(playerController));
    }

    IEnumerator HoldPlayerCoroutine(PlayerController playerController)
    {
        while (isHoldingPlayer)
        {
            playerController.TakeDamage(damage);
            Debug.Log("Jogador tomou dano.");

            float timer = 3f;
            while (timer > 0)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Debug.Log("Jogador pressionou X.");
                    ReleasePlayer(playerController);
                    yield break;
                }
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }

    void ReleasePlayer(PlayerController playerController)
    {
        isHoldingPlayer = false;
        playerController.enabled = true; // Reativar controle do jogador
        PushEnemyBack(); // Empurrar o inimigo para trás
        Debug.Log("Jogador empurrou o Amigo Imaginário para trás.");
    }

    void PushEnemyBack()
    {
        Vector3 pushDirection = (transform.position - player.position).normalized;
        rb.AddForce(pushDirection * 30000); // Empurrar o inimigo para trás com mais força
    }

    void ReturnToInitialPosition()
    {
        float distanceToInitial = Vector3.Distance(transform.position, initialPosition);
        if (distanceToInitial > 0.1f)
        {
            Vector3 direction = (initialPosition - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
            Debug.Log("Amigo Imaginário está retornando à posição inicial.");
        }
        else
        {
            rb.velocity = Vector3.zero; // Parar o movimento
            Debug.Log("Amigo Imaginário chegou à posição inicial.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHoldingPlayer && !isStunned)
        {
            HoldPlayer();
        }
        else if (collision.gameObject.CompareTag("MagicCircle"))
        {
            Debug.Log("Amigo Imaginário entrou no círculo mágico.");
            StartCoroutine(SlowDown());
        }
    }

    IEnumerator SlowDown()
    {
        isStunned = true;
        float originalSpeed = chaseSpeed;
        chaseSpeed = chaseSpeed / 2; // Reduz a velocidade pela metade
        yield return new WaitForSeconds(10f);
        chaseSpeed = originalSpeed; // Restaura a velocidade original
        isStunned = false;
        Debug.Log("Amigo Imaginário não está mais paralisado.");
    }
}
