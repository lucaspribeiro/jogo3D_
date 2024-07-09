using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Closet : MonoBehaviour
{
    private Animator animator;
    private bool aberto;
    public GameObject centro;


    private bool isPlayerNearby = false;
    private bool isPlayerHiding = false;
    private Transform player;
    private EnemyFather enemyFatherScript;
    private EnemyMother enemyMotherScript;
    private ImaginaryFriend imaginaryFriendScript;

    void Start()
    {
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        GameObject enemyFatherObject = GameObject.FindGameObjectWithTag("EnemyFather");
        if (enemyFatherObject != null)
        {
            enemyFatherScript = enemyFatherObject.GetComponent<EnemyFather>();
        }

        GameObject enemyMotherObject = GameObject.FindGameObjectWithTag("EnemyMother");
        if (enemyMotherObject != null)
        {
            enemyMotherScript = enemyMotherObject.GetComponent<EnemyMother>();
        }

        GameObject imaginaryFriendObject = GameObject.FindGameObjectWithTag("ImaginaryFriend");
        if (imaginaryFriendObject != null)
        {
            imaginaryFriendScript = imaginaryFriendObject.GetComponent<ImaginaryFriend>();
        }
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fechado"))
                {
                    animator.SetBool("Abrindo", true);
                    aberto = true;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aberto"))
                {
                    animator.SetBool("Abrindo", false);
                    aberto = false;
                }

                if (isPlayerHiding && aberto)
                {
                    // Lógica para sair do armário
                    if (player != null)
                    {
                        player.gameObject.SetActive(true); // Torna o jogador visível
                    }
                    if (enemyFatherScript != null)
                    {
                        enemyFatherScript.isHiding = false; // Atualiza o estado de hiding
                    }
                    if (enemyMotherScript != null)
                    {
                        enemyMotherScript.isHiding = false; // Atualiza o estado de hiding
                    }
                    if (imaginaryFriendScript != null)
                    {
                        imaginaryFriendScript.isHiding = false; // Atualiza o estado de hiding
                    }
                    isPlayerHiding = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isPlayerHiding && aberto)
                {
                    // Lógica para entrar no armário
                    if (player != null)
                    {
                        player.gameObject.SetActive(false); // Esconde o jogador
                    }
                    if (enemyFatherScript != null)
                    {
                        enemyFatherScript.StopChase(); // Faz o Pai parar de perseguir e retornar
                        enemyFatherScript.isHiding = true; // Atualiza o estado de hiding
                    }
                    if (enemyMotherScript != null)
                    {
                        enemyMotherScript.StopChase(); // Faz a Mãe parar de perseguir e retornar
                        enemyMotherScript.isHiding = true; // Atualiza o estado de hiding
                    }
                    if (imaginaryFriendScript != null)
                    {
                        imaginaryFriendScript.StopChase(); // Faz o Amigo Imaginário parar de perseguir e retornar
                        imaginaryFriendScript.isHiding = true; // Atualiza o estado de hiding
                    }
                    isPlayerHiding = true;

                    player.transform.position = centro.transform.position;
                    animator.SetBool("Abrindo", false);
                }
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
