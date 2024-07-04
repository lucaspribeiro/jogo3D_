using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5;
    public GameObject inventario;

    private Camera mainCam;
    private Vector3 moveInput;
    [SerializeField] private Transform rotSprite;
    [SerializeField] private Animator anim;
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
            //InventoryManager.Instance.ListItems();
        }

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");

        if (moveInput.z > 0)
        {
            anim.SetFloat("direcao", 1);
        }else if(moveInput.z < 0)
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

}
