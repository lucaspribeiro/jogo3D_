using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private bool isPlayerHiding = false;
    private Transform player;
    private EnemyFather enemyScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyFather>();
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            if (isPlayerHiding)
            {
                // Lógica para sair do armário
                player.gameObject.SetActive(true); // Torna o jogador visível
                enemyScript.isHiding = false; // Atualiza o estado de hiding
                isPlayerHiding = false;
            }
            else
            {
                // Lógica para entrar no armário
                player.gameObject.SetActive(false); // Esconde o jogador
                enemyScript.StopChase(); // Faz o Pai parar de perseguir e retornar
                enemyScript.isHiding = true; // Atualiza o estado de hiding
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
