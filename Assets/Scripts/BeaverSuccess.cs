using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeaverSuccess : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D sadBeaver;
    public Texture2D happyBeaver;
    public BeaverMapGenerator mapgen;

    private Sprite sadSprite;
    private Sprite happySprite;

    private Image image;
    private bool m_success = false;
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        sadSprite = Sprite.Create(sadBeaver, image.sprite.rect, image.sprite.pivot, image.sprite.pixelsPerUnit);
        happySprite = Sprite.Create(happyBeaver, image.sprite.rect, image.sprite.pivot, image.sprite.pixelsPerUnit);
        image.sprite = sadSprite;
    }

    // Update is called once per frame
    void Update()
    {
        bool success = mapgen.GetWin();
        if (success != m_success)
        {
            image.sprite = success ? happySprite : sadSprite;
            m_success = success;
        }
    }
}
