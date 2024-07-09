using System.Collections;
using UnityEngine;

public class EnemyMother : MonoBehaviour
{
    public float detectionRange = 12f;
    public float chaseSpeed = 5f;
    public float holdDistance = 1f; // Distância para segurar o jogador
    public float chaseDuration = 3f;
    public Vector3 initialPosition;
    private Transform player;
    private bool isChasing;
    private bool isHoldingPlayer;
    private bool isStunned; // Verifica se a Mãe está paralisada
    private float chaseTimer;
    private Rigidbody rb;
    public int damage = 1; // Dano causado ao jogador
    public bool isHiding; // Verifica se o jogador está escondido

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isHiding || isHoldingPlayer || isStunned)
        {
            if (isHoldingPlayer && Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Jogador pressionou X.");
                ReleasePlayer();
            }

            if (isHiding)
            {
                ReturnToInitialPosition();
            }

            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isHiding)
        {
            StopChase();
            ReturnToInitialPosition();
            return;
        }

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
                ReturnToInitialPosition();
            }
            else if (distanceToPlayer <= holdDistance)
            {
                HoldPlayer();
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
        Debug.Log("Mãe começou a perseguir.");
    }

    public void StopChase()
    {
        isChasing = false;
        rb.velocity = Vector3.zero; // Parar o movimento
        Debug.Log("Mãe parou de perseguir.");
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
        Debug.Log("Mãe está perseguindo.");
    }

    public void ReturnToInitialPosition()
    {
        float distanceToInitial = Vector3.Distance(transform.position, initialPosition);
        if (distanceToInitial > 0.1f)
        {
            Vector3 direction = (initialPosition - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
            Debug.Log("Mãe está retornando à posição inicial.");
        }
        else
        {
            rb.velocity = Vector3.zero; // Parar o movimento
            Debug.Log("Mãe chegou à posição inicial.");
        }
    }

    void HoldPlayer()
    {
        if (isStunned) return;

        isChasing = false;
        isHoldingPlayer = true;
        rb.velocity = Vector3.zero; // Parar o movimento
        Debug.Log("Mãe está segurando o jogador.");
        StartCoroutine(HoldPlayerCoroutine());
    }

    IEnumerator HoldPlayerCoroutine()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = false; // Desativar controle do jogador

        while (isHoldingPlayer)
        {
            playerController.TakeDamage(damage);
            Debug.Log("Jogador tomou dano.");

            yield return new WaitForSeconds(3f);

            if (isHoldingPlayer && Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Jogador pressionou X durante a espera.");
                ReleasePlayer();
                yield break;
            }
        }
    }

    void ReleasePlayer()
    {
        isHoldingPlayer = false;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = true; // Reativar controle do jogador
        gameObject.SetActive(false); // Mãe some
        Debug.Log("Mãe sumiu.");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHoldingPlayer && !isStunned)
        {
            Debug.Log("Mãe colidiu com o jogador.");
            HoldPlayer();
        }
        else if (collision.gameObject.CompareTag("MagicCircle"))
        {
            Debug.Log("Mãe entrou no círculo mágico.");
            StartCoroutine(Stun());
        }
    }

    IEnumerator Stun()
    {
        isStunned = true;
        rb.velocity = Vector3.zero; // Parar o movimento
        rb.isKinematic = true; // Desativa a física do Rigidbody
        yield return new WaitForSeconds(10f);
        rb.isKinematic = false; // Reativa a física do Rigidbody
        isStunned = false;
        Debug.Log("Mãe não está mais paralisada.");
    }
}
