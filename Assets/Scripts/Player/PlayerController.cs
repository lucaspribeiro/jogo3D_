using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5;
    public GameObject inventario;
    public int opcao = 0;

    private Camera mainCam;
    private Vector3 moveInput;
    [SerializeField] private Transform rotSprite;
    [SerializeField] private Animator anim;

    
    // **adicionei pra vida do personagem atualizar (nao sei como eh pra ser so fiz de exemplo)**
    public int maxHealth = 100;
    public int currentHealth;
    public Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && inventario.active == false){
            //inventario.GetComponent<InventoryManager>().ListItems();
            inventario.SetActive(true);
            MouseController.Instance.UnlockMouse();
            //InventoryManager.Instance.ListItems();
        }

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");

        if (moveInput.z > 0)
        {
            anim.SetFloat("direcao", 1);
        }
        else if(moveInput.z < 0)
        {
            anim.SetFloat("direcao", 2);
        }
        else
        {
            anim.SetFloat("direcao", 0);
        }

        if(moveInput.x == 0)
        {
            anim.SetBool("andando_lado", false);
            //rotSprite.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            if (anim.GetFloat("direcao") == 0)
            {
                anim.SetFloat("direcao", 2);
            }
            anim.SetBool("andando_lado", true);

            if (moveInput.x > 0)
            {
                rotSprite.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                rotSprite.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        moveInput.Normalize();
        moveInput *= speed;

        rb.velocity = transform.forward * moveInput.z + transform.right * moveInput.x + (new Vector3(0,rb.velocity.y,0));

    }

    void LateUpdate()
    {
        var rotation = mainCam.transform.rotation;
        rotSprite.LookAt(rotSprite.position + rotation * Vector3.forward, rotation * Vector3.up);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = rotation;
        
        //transform.LookAt(transform.position, rotation*Vector3.up);
        //transform.rotation = Quaternion.Euler(0,-rotSprite.rotation.y,0);

    }

    // **adicionei**
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth < 0)
        {
            Die();
        }
    }

    // **adicionei**
    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }

    // **adicionei**
    public void Die()
    {
        SceneManager.LoadScene("scenne Luis");
    }

}
