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
    public float distanceObj;
    public GameObject head, pivot;
    Vector3 movement;
    public GameObject objetive;
    public bool movingTwds;
    public float dash;
    public GameObject Line;
    public int StateType;
    public MeshRenderer[] Eyes;
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
        }
        else
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
        if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, distanceRay))
        {
            distanceObj = Vector3.Distance(head.transform.position, hit.transform.gameObject.transform.position);
            Line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distanceObj));
            if (!movingTwds)
            { 
                if (hit.transform.tag == "Target")
                {
                    objetive = hit.transform.gameObject;
                    Line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distanceObj));
                    #region robarPoder
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StateType = objetive.GetComponent<EnemyType>().EnemyT;
                        foreach (MeshRenderer a in Eyes)
                        {
                            a.GetComponent<MeshRenderer>().material.color = objetive.GetComponent<MeshRenderer>().material.color;
                        }
                    }
                    #endregion
                    #region movingTo
                    if (Input.GetMouseButtonDown(0))
                    {

                        rb.useGravity = false;
                        movingTwds = true;
                    }
                    #endregion
                }
            }
        }else
        {
            Line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distanceRay));
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

    public void State()
    {
        switch (StateType)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
