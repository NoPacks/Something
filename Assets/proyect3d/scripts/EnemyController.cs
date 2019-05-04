using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            onDead();
        }
    }

    void onDead()
    {
        transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
        anim.SetBool("Die", true);
        //Instantiate(hitBox, attackSpawner.position, transform.rotation);
        Destroy(this.gameObject, 3f);
    }
}
