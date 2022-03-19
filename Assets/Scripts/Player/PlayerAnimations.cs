using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    float walkSensitivity = 0.01f; 
    enum Dir
    {
        UP=0,
        DOWN=1,
        LEFT=2,
        RIGHT=3,
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > walkSensitivity)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.x > walkSensitivity)
            {
                anim.SetFloat("WalkDir", (float)Dir.RIGHT);
            }
            else if (rb.velocity.x < -walkSensitivity)
            {
                anim.SetFloat("WalkDir", (float)Dir.LEFT);
            }
        }
        else
        {
            if (rb.velocity.y < -walkSensitivity)
            {
                anim.SetFloat("WalkDir", (float)Dir.DOWN);
            }
            else if (rb.velocity.y > walkSensitivity)
            {
                anim.SetFloat("WalkDir", (float)Dir.UP);
            }
        }
    }
}
