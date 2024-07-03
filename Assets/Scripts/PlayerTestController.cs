using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PlayerTestController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5;
    private Vector2 moveInput;

    public int maxHealth = 20;
    public int currentHealth;
    public Text healthText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if(currentHealth < 0) 
        { 
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }

    public void Die()
    {
        SceneManager.LoadScene("scenne Lucas");
    }
}
