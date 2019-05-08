using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    CapsuleCollider capsule;
    public Transform player;
    public float atractionForce;
    public int life;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            onDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            life -= 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!anim.GetBool("Die"))
        {
            transform.position = Vector3.MoveTowards(transform.position, other.gameObject.transform.position, atractionForce * Time.deltaTime);
        }
    }

    void onDead()
    {
        //capsule.enabled = false;
        transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
        anim.SetBool("Die", true);
        Destroy(this.gameObject, 3f);
    }
}
