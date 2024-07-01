using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5f;
    private bool isHidden = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isHidden)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.velocity = movement * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void Hide()
    {
        isHidden = true;
        gameObject.SetActive(false); // vai tornar o jogador invisivel
    }

    public void Unhide()
    {
        isHidden = false;
        gameObject.SetActive(true); // torna o jogador visivel de novo
    }

    public bool IsHidden()
    {
        return isHidden;
    }
}
