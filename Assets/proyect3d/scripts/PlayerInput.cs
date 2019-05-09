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
    public bool addedForce;

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
        //inputX = Input.GetAxis("Horizontal");
        //inputZ = Input.GetAxis("Vertical");
        
        anim.SetFloat("MovX", inputX);
        anim.SetFloat("MovZ", inputZ);
        anim.SetBool("inCombo", isAttacking);
        anim.SetInteger("ComboCounter", comboCounter);

        if (inputX == 0 && inputZ == 0)
        {
            if (isGrounded)
            {
                anim.SetBool("Static", true);
            }
        } else
        {
            anim.SetBool("Static", false);
            if (isGrounded)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed * Time.deltaTime);
            }
        }

        movement = new Vector3(inputX * Speed, rb.velocity.y, inputZ * Speed);
        movement = playerPivot.transform.rotation * movement;

        if (!isAttacking)
        {
            if (anim.GetBool("isGrounded"))
            {
                inputX = Input.GetAxis("Horizontal");
                inputZ = Input.GetAxis("Vertical");
            }
            rb.velocity = movement;
            swordSlash.enabled = false;

        }
        else
        {
            
            if (!anim.GetBool("isGrounded"))
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y*Time.deltaTime, rb.velocity.z);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
            swordSlash.enabled = true;
        }


        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < 1)
        {
            comboCounter = 0;
            isAttacking = false;
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
            if (comboCounter < 3)
            {
                comboCounter += 1;
            }
            //transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed);
            
        }
        //anim.SetInteger("ComboCounter", comboCounter);
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

    private void StartCombo()
    {
        isAttacking = true;
    }

    private void MidCombo()
    {
        comboCounter = 0;
        Instantiate(hitBox, attackSpawner.position,new Quaternion(hitBox.transform.rotation.x,transform.rotation.y,hitBox.transform.rotation.z,hitBox.transform.rotation.w));
    }

    private void EndCombo()
    {
        //myImpulseSource.GenerateImpulse(Vector3.right);
        isAttacking = false;
    }

    private void Finisher(int x)
    {
        myImpulseSource.GenerateImpulse(Vector3.right);
        comboCounter = 0;
        
        isAttacking = false;
        Debug.Log(x);
    }
}
