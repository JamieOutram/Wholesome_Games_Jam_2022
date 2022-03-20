using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    Rigidbody2D rb;
    Transform trans;
    float walkSensitivity = 0.01f;
    enum Dir
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.x > walkSensitivity)
            {
                trans.rotation = Quaternion.Euler(0,0,90);
            }
            else if (rb.velocity.x < -walkSensitivity)
            {
                trans.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        else
        {
            if (rb.velocity.y < -walkSensitivity)
            {
                trans.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (rb.velocity.y > walkSensitivity)
            {
                trans.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }
}
