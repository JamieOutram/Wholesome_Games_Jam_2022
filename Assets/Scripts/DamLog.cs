using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamLog : MonoBehaviour
{
    Vector2Int m_loc = new Vector2Int(0, 0);
    SpriteRenderer m_spriteRenderer;

    private float m_initY;

    public float InitY()
    {
        return m_initY;
    }
    public void MoveSprite(Vector3 loc)
    {
        loc.z = 0;
        m_spriteRenderer.transform.position = loc;
    }

    public void RotateSprite(float rotation)
    {
        m_spriteRenderer.transform.Rotate(0, 0, rotation);
    }
    void Start()
    {
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_initY = m_spriteRenderer.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
