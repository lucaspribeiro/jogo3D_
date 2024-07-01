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
    //private float timeToFindPlayer = 10f; 
    //private float currentTime = 0f;
    private Vector3 startingPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    void Update()
    {
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


    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Causar dano ao jogador
            // Exemplo: collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);   // tem que criar no player
        }
    }
        
    // tem que criar no mapa para testar depois 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Armario"))
        {
            playerDetected = true;
        }
    }
    */

}
