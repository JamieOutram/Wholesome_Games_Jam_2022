using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetAfterDelay : MonoBehaviour
{
    bool yeeting = false;
    float timer = 0;
    Vector3 Startpos;
    public Vector3 Endpos;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartYeet), 5);
        Startpos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (yeeting)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(Startpos, Endpos, timer / 5);
            if(timer/5 > 1)
            {
                yeeting = false;
            }
        }
    }

    void StartYeet()
    {
        yeeting = true;
    }
}
