using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerInput : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    float inputX;
    float inputZ;
    public float Speed;
    public float jumpForce;
    public int jumpCounter;
    public bool isGrounded;
    Vector3 movement;
    public GameObject playerPivot;
    public CinemachineVirtualCamera myVirtualCamera;
    private CinemachineImpulseSource myImpulseSource;

    public Transform attackSpawner;

    public bool isAttacking;
    int comboCounter;

    [Header("Instance")]
    public GameObject hitBox;

    [Header("Trail")]
    public TrailRenderer swordSlash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        myImpulseSource = myVirtualCamera.GetComponent<CinemachineImpulseSource>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        anim.SetFloat("MovX", inputX);
        anim.SetFloat("MovZ", inputZ);



        if (inputX == 0 && inputZ == 0)
        {
            anim.SetBool("Static", true);
        } else
        {
            anim.SetBool("Static", false);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed * Time.deltaTime);
        }

        movement = new Vector3(inputX * Speed, rb.velocity.y, inputZ * Speed);
        movement = playerPivot.transform.rotation * movement;

        if (!isAttacking)
        {
            rb.velocity = movement;
            swordSlash.enabled = false;

        }
        else
        {
            rb.velocity = Vector3.zero;
            swordSlash.enabled = true;
        }


        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < 1)
        {
            anim.SetTrigger("Jumping");
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
            jumpCounter++;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                isAttacking = true;
            }
            comboCounter += 1;
            transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed);
            
        }
        anim.SetInteger("ComboCounter", comboCounter);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            jumpCounter = 0;
            isGrounded = true;
            anim.SetBool("isGrounded", true);
        }
    }

    private void EndCombo()
    {
        //myImpulseSource.GenerateImpulse(Vector3.right);
        Instantiate(hitBox, attackSpawner.position, transform.rotation);
        comboCounter = 0;
        isAttacking = false;
    }

}
