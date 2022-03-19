using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMapCamToSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        Debug.Log(spriteRenderer.bounds);
        UpdateSize();
    }

    private void Update()
    {
        UpdateSize();
    }

    void UpdateSize()
    {
        //float spriteWidth = 0.5f*spriteTransform.lossyScale.x * sprite.rect.width / sprite.pixelsPerUnit;
        //float spriteHeight = 0.5f*spriteTransform.lossyScale.y * sprite.rect.height / sprite.pixelsPerUnit;
        float spriteWidth = spriteRenderer.bounds.extents.x;
        float spriteHeight = spriteRenderer.bounds.extents.y;
        float spriteAspect = spriteWidth / spriteHeight;
        //orthsize = camh*0.5f
        if (cam.aspect > spriteAspect)
        {
            //wider cam so limit to height
            //camh = spriteh
            cam.orthographicSize = spriteHeight;
        }
        else
        {
            //taller cam so limit to width
            //calculate camera height for camera width of spriteWidth
            //camw = camh*camAspect
            //spritew=camh*camAspect
            //camh = spritew/camAspect
            cam.orthographicSize = spriteWidth / cam.aspect;
        }
        //sprite scale, unit

    }
}
