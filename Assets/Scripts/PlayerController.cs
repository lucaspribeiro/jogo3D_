using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5;
    public GameObject inventario;

    private Vector2 moveInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && inventario.active == false){
            //inventario.GetComponent<InventoryManager>().ListItems();
            inventario.SetActive(true);
            //InventoryManager.Instance.ListItems();
        }

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        rb.velocity = new Vector3(moveInput.x*speed, rb.velocity.y, moveInput.y*speed);
    
    }

}
