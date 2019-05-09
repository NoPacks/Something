using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayer : MonoBehaviour
{
    Rigidbody rb;
    private float inputX, inputY, inputZ;
    public float Speed;
    int jumpCounter;
    public float jumpForce;
    bool isGrounded;
    public bool aiming;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        if (Input.GetMouseButton(1))
        {
            rb.velocity = new Vector3(inputX * Speed, rb.velocity.y, inputZ * Speed);
            if (!aiming)
            {
                Aiming();
            }
        }else
        {
            aiming = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            jumpCounter = 0;
            isGrounded = true;
        }
    }

    void Aiming()
    {
        aiming = true;
        RaycastHit hit;
        if (Physics.Linecast(transform.position,Vector3.forward,out hit))
        {

        }
            
    }
}
