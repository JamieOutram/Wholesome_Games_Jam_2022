using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamLog : MonoBehaviour
{
    Vector2Int m_loc = new Vector2Int(0, 0);
    SpriteRenderer m_spriteRenderer;


    public void MoveSprite(Vector3 loc)
    {
        loc.z = 0;
        m_spriteRenderer.transform.position = loc;
    }

    public void RotateSprite(float rotation)
    {
        m_spriteRenderer.transform.Rotate(0, 0, rotation);
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
