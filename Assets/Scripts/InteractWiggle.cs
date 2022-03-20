using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWiggle : MonoBehaviour
{
    public Transform target;
    public float wiggleTime = 0.5f;
    public Vector3 wiggleOffset = new Vector3(0,0.2f,0);
    Vector3 localPosition;
    public Vector3 wiggleScale = new Vector3(0,0.1f,0);
    bool isWiggle = false;
    bool isGrow = true;
    float timer = 0;
    Vector3 maxPos;
    Vector3 maxScale;
    Vector3 minPos;
    Vector3 minScale;


    void Start()
    {
        localPosition = target.localPosition;
        maxPos = target.localPosition + wiggleOffset;
        maxScale = target.localScale + wiggleScale;
        minPos = target.localPosition;
        minScale = target.localScale;
    }

    private void Update()
    {
        if (isWiggle)
        {
            timer += Time.deltaTime;
            UpdateWiggle();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isWiggle = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isWiggle = false;
    }


    void UpdateWiggle()
    {
        if (timer > wiggleTime)
        {
            isGrow = !isGrow;
            timer=0;
        }
        float t = 0;
        if (isGrow)
        {
            t = timer / wiggleTime;
        }
        else
        {
            t = 1 - timer / wiggleTime;
        }

        target.localPosition = Vector3.Lerp(minPos, maxPos, t);
        target.localScale = Vector3.Lerp(minScale, maxScale, t);
    }


}
