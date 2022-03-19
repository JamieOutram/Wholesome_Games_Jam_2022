using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundPlayerCamToSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    Camera cam;
    Transform player;
    public Vector2 offset = new Vector2(0, 0);
        
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent;
        cam = GetComponent<Camera>();
        Debug.Log(spriteRenderer.bounds);
    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(0, 0);
        Debug.Log(cam.rect);
        
        Vector2 camExtent = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);
        Vector2 camMax = (Vector2)player.position + camExtent;
        Vector2 camMin = (Vector2)player.position - camExtent;
        Debug.Log(camMax);
        if (camMax.y > spriteRenderer.bounds.max.y)
        {
            offset -= new Vector2(0, camMax.y - spriteRenderer.bounds.max.y);
        }
        if (camMax.x > spriteRenderer.bounds.max.x)
        {
            offset -= new Vector2(camMax.x - spriteRenderer.bounds.max.x, 0);
        }
        if (camMin.y < spriteRenderer.bounds.min.y)
        {
            offset -= new Vector2(0, camMin.y - spriteRenderer.bounds.min.y);
        }
        if (camMin.x < spriteRenderer.bounds.min.x)
        {
            offset -= new Vector2(camMin.x - spriteRenderer.bounds.min.x, 0);
        }
        cam.transform.localPosition = new Vector3(offset.x,offset.y,-10);
    }
}
