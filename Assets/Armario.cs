using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armario : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    public GameObject centro;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fechado"))
                {
                    animator.SetBool("Abrindo", true);
                    player.transform.position = centro.transform.position;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aberto"))
                {
                    animator.SetBool("Abrindo", false);
                }
               
            }
        }
    }
}
