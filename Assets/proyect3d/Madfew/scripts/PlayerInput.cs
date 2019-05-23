using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

public class PlayerInput : MonoBehaviour
{

    #region variables
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

    public bool isAttacking;
    int comboCounter;

    public bool isDead;

    public SkinnedMeshRenderer sMRenderer_Player;
    public MeshRenderer sMRenderer_Sword;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip[] Sounds;

    [Header ("CameraShake")]
    //public CinemachineVirtualCamera myVirtualCamera;
    //private CinemachineImpulseSource myImpulseSource;

    [Header("Instance")]
    public GameObject hitBox;
    public Transform attackSpawner;

    [Header("Trail")]
    public TrailRenderer swordSlash;
    
    [Header("Raycast")]
    public float distanceRay;
    public GameObject objetive;
    public int stateType;

    public MeshRenderer[] changeColor;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //myImpulseSource = myVirtualCamera.GetComponent<CinemachineImpulseSource>();
        isAttacking = false;
    }

    void Update()
    {
        #region Animator
        anim.SetFloat("MovX", inputX);
        anim.SetFloat("MovZ", inputZ);
        anim.SetBool("inCombo", isAttacking);
        anim.SetInteger("ComboCounter", comboCounter);
        anim.SetBool("isGrounded", isGrounded);
        #endregion
        Aiming();
        #region Movement
        movement = new Vector3(inputX * Speed, rb.velocity.y, inputZ * Speed);
        if(rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 1.1f, rb.velocity.z);
        }
        #endregion

        #region MovementConditions
        if (!isDead)
        {
            if (isGrounded && !isAttacking)
            {
                movement = playerPivot.transform.rotation * movement;
                inputX = Input.GetAxis("Horizontal");
                inputZ = Input.GetAxis("Vertical");
                rb.velocity = movement;
                swordSlash.enabled = false;
                if(inputX!=0 || inputZ != 0)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed * Time.deltaTime);
                }
            }
            else if (isAttacking)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed * Time.deltaTime * 5);
                swordSlash.enabled = true;
                rb.velocity = Vector3.zero;
                if (!isGrounded)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * Time.deltaTime, rb.velocity.z);
                    
                }
            } else if (inputX == 0 && inputZ == 0)
            {

            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, playerPivot.transform.rotation, Speed * Time.deltaTime);
            }
        }


        #endregion

        #region MouseOrKeyDown

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        #endregion

    }

    #region Collisions

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            jumpCounter = 0;
            isGrounded = true;
            anim.SetBool("isGrounded", true);
        } else if (col.gameObject.tag == "Dead")
        {
            StartCoroutine(Die());
        }
    }

    #endregion

    #region Die&&Respawn
    IEnumerator Die()
    {
        isDead = true;
        audioSource.PlayOneShot(Sounds[0]);
        Input.ResetInputAxes();
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(3f);
        sMRenderer_Player.enabled = false;
        sMRenderer_Sword.enabled = false;
    }

    private void Respawn()
    {
        StopAllCoroutines();
        inputX = 0;
        inputZ = 0;
        anim.SetTrigger("Respawn");
        //transform.position = new Vector3(0, 0.3f, 0);
        
        transform.SetPositionAndRotation(new Vector3(0, 0.3f, 0), new Quaternion(0, 0, 0, 0));
        rb.velocity = Vector3.zero;
        sMRenderer_Player.enabled = true;
        sMRenderer_Sword.enabled = true;
        isDead = false;
    }

    #endregion

    #region ComboSection

    private void StartCombo()
    {
        
    }

    private void MidCombo()
    {
        comboCounter = 0;
        Instantiate(hitBox, attackSpawner.position,new Quaternion(hitBox.transform.rotation.x,transform.rotation.y,hitBox.transform.rotation.z,hitBox.transform.rotation.w));
    }

    private void EndCombo()
    {
        //myImpulseSource.GenerateImpulse(Vector3.right);
        if (comboCounter == 0)
        {
            isAttacking = false;
        }
    }

    private void Finisher(int x)
    {
        //myImpulseSource.GenerateImpulse(Vector3.right);
        comboCounter = 0;
        
        isAttacking = false;
        Debug.Log(x);
    }

    #endregion 

    void Aiming()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerPivot.transform.position, playerPivot.transform.forward, out hit, distanceRay))
        {
            //distanceObj = Vector3.Distance(playerPivot.transform.position, hit.transform.gameObject.transform.position);
            //Line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distanceObj));
            if (!isAttacking)
            {
                if (hit.transform.tag == "Enemy")
                {
                    objetive = hit.transform.gameObject;
                    //Line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distanceObj));
                    #region robarPoder
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        stateType = objetive.GetComponent<EnemyType>().EnemyT;
                        foreach (MeshRenderer a in changeColor)
                        {
                            switch (stateType)
                            {
                                case 1: a.GetComponent<MeshRenderer>().material.color = Color.red;
                                    break;
                                case 2: a.GetComponent<MeshRenderer>().material.color = Color.blue;
                                    break;
                                case 3: a.GetComponent<MeshRenderer>().material.color = Color.green;
                                    break;
                            }
                        }
                    }
                    #endregion
                }
            }
        }
        /*else
        {
            Line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distanceRay));
        }*/
    }
}
