using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armario : MonoBehaviour
{
    private bool playerNearby = false;
    private PlayerTestController player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.GetComponent<PlayerTestController>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }

    void Update()
    {
        if (playerNearby && player != null && Input.GetKeyDown(KeyCode.E))
        {
            if (player.IsHidden())
            {
                player.Unhide();
            }
            else
            {
                player.Hide();
            }
        }
    }

}
