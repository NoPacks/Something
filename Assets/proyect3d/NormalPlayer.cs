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
    public float distanceRay;
    public GameObject head,pivot;
    Vector3 movement;
    public GameObject objetive;
    public bool movingTwds;
    public float dash;
    public GameObject Line;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        movement = new Vector3(inputX * Speed, rb.velocity.y, inputZ * Speed);
        movement = pivot.transform.rotation * movement;

        if (movingTwds)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetive.transform.position, dash * Time.deltaTime);
        }

        if (Input.GetMouseButton(1))
        {
            Aiming();
            if (!aiming)
            {
                aiming = true;
                Line.SetActive(true);
            }
        }else
        {
            if (!movingTwds)
            {
                rb.velocity = movement;
            }
            aiming = false;
            Line.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
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
        RaycastHit hit;
        Debug.DrawRay(head.transform.position, head.transform.forward*distanceRay);
        if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, distanceRay))
        {
            if (!movingTwds)
            {
                if (hit.transform.tag == "Target")
                {
                    objetive = hit.transform.gameObject;
                    if (Input.GetMouseButtonDown(0))
                    {
                        
                        rb.useGravity = false;
                        movingTwds = true;
                    }
                }
            }
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            rb.useGravity = true;
            movingTwds = false;
            objetive = null;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            rb.useGravity = true;
            movingTwds = false;
            objetive = null;
        }
    }
}
