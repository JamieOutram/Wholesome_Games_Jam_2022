using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope_segment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;

    // Start is called before the first frame update
    void Start()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        rope_segment aboveSegment = connectedAbove.GetComponent<rope_segment>();
        if(aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;
            float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            Debug.Log(spriteBottom);
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom/transform.localScale.y*-1);
        }
        else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        }

    }
}
