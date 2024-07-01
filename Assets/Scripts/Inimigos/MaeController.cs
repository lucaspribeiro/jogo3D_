using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaeController : MonoBehaviour
{
    public Transform player; 
    public float speed = 6f; 
    public int damage = 1; 
    private float detectionRange = 15f;
    public float returnSpeed = 8f;
    public float breakTime = 8f;

    private Rigidbody rb;
    private bool playerDetected = false;
    private bool returningToStart = false;

    public float paralyzeDuration = 3f; // Duração da paralisação em segundos
    private bool paralyzed = false;
    private float originalSpeed;

    private Vector3 startingPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
        originalSpeed = speed; // Salvar a velocidade original
    }

    void Update()
    {

        if (paralyzed)
        {
            // Se estiver paralisada, não fazer nada (parar o movimento)
            rb.velocity = Vector3.zero;
        }

        if (!returningToStart)
        {
            DetectPlayer();
            if (playerDetected)
            {
                ChasePlayer();
            }
            else
            {
                ReturnToStartPosition();
            }
        }
        else
        {
            ReturnToStartPosition();
        }
    }

    void DetectPlayer()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            playerDetected = true;
            //currentTime = 0f;
        }
        else
        {
            playerDetected = false;
            //currentTime += Time.deltaTime;
            //if (currentTime >= timeToFindPlayer)
            //{
                //playerDetected = false;
                //currentTime = 0f;
            //}
        }
    }

    void ChasePlayer()
    {
        if (player.GetComponent<PlayerTestController>().IsHidden())
        {
            playerDetected = false;
            returningToStart = true;
            //rb.velocity = Vector3.zero; // Parar movimento
            return;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
    }

    void ReturnToStartPosition()
    {
        
        Vector3 direction = (startingPosition - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * returnSpeed, rb.velocity.y, direction.z * returnSpeed);

        if (Vector3.Distance(transform.position, startingPosition) < 0.1f)
        {
            rb.velocity = Vector3.zero; // Parar movimento ao chegar ao ponto de partida
            returningToStart = false; // Resetar flag de retorno ao ponto de origem
        }
        
    }
    public void Paralyze(float duration)
    {
        paralyzed = true;
        Invoke("EndParalyze", duration);
    }

    void EndParalyze()
    {
        paralyzed = false;
    }
}
