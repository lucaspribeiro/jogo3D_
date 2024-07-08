using System.Collections;
using UnityEngine;

public class EnemyFather : MonoBehaviour
{
    public float detectionRange = 10f;
    public float chaseSpeed = 3.5f;
    public float holdDistance = 1f; // Distância para segurar o jogador
    public float chaseDuration = 5f;
    public Vector3 initialPosition;
    private Transform player;
    private bool isChasing;
    private bool isHoldingPlayer;
    private bool isStunned; // Verifica se o Pai está paralisado
    private float chaseTimer;
    private Rigidbody rb;
    public int damage = 2; // Dano causado ao jogador
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
        Debug.Log("Pai começou a perseguir.");
    }

    public void StopChase()
    {
        isChasing = false;
        rb.velocity = Vector3.zero; // Parar o movimento
        Debug.Log("Pai parou de perseguir.");
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
        Debug.Log("Pai está perseguindo.");
    }

    public void ReturnToInitialPosition()
    {
        float distanceToInitial = Vector3.Distance(transform.position, initialPosition);
        if (distanceToInitial > 0.1f)
        {
            Vector3 direction = (initialPosition - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * chaseSpeed, rb.velocity.y, direction.z * chaseSpeed);
            Debug.Log("Pai está retornando à posição inicial.");
        }
        else
        {
            rb.velocity = Vector3.zero; // Parar o movimento
            Debug.Log("Pai chegou à posição inicial.");
        }
    }

    void HoldPlayer()
    {
        if (isStunned) return;

        isChasing = false;
        isHoldingPlayer = true;
        rb.velocity = Vector3.zero; // Parar o movimento
        Debug.Log("Pai está segurando o jogador.");
        StartCoroutine(HoldPlayerCoroutine());
    }

    IEnumerator HoldPlayerCoroutine()
    {
        PlayerTestController playerController = player.GetComponent<PlayerTestController>();
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
        PlayerTestController playerController = player.GetComponent<PlayerTestController>();
        playerController.enabled = true; // Reativar controle do jogador
        gameObject.SetActive(false); // Pai some
        Debug.Log("Pai sumiu.");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHoldingPlayer && !isStunned)
        {
            Debug.Log("Pai colidiu com o jogador.");
            HoldPlayer();
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
        Debug.Log("Pai não está mais paralisado.");
    }
}
