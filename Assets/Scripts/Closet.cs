using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private bool isPlayerHiding = false;
    private Transform player;
    private EnemyFather enemyFatherScript;
    private EnemyMother enemyMotherScript;

    void Start()
    {
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
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            if (isPlayerHiding)
            {
                // L�gica para sair do arm�rio
                if (player != null)
                {
                    player.gameObject.SetActive(true); // Torna o jogador vis�vel
                }
                if (enemyFatherScript != null)
                {
                    enemyFatherScript.isHiding = false; // Atualiza o estado de hiding
                }
                if (enemyMotherScript != null)
                {
                    enemyMotherScript.isHiding = false; // Atualiza o estado de hiding
                }
                isPlayerHiding = false;
            }
            else
            {
                // L�gica para entrar no arm�rio
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
                    enemyMotherScript.StopChase(); // Faz a M�e parar de perseguir e retornar
                    enemyMotherScript.isHiding = true; // Atualiza o estado de hiding
                }
                isPlayerHiding = true;
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
