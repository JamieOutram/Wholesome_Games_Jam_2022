using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColourBlindButton : MonoBehaviour, IPointerDownHandler
{
    public SpriteRenderer basketSquareSprite;
    public SpriteRenderer basketCircleSprite;
    public SpriteRenderer basketHexagonSprite;
    public SpriteRenderer basketDiamondSprite;
    public SpriteRenderer snakeSquareSprite;
    public SpriteRenderer snakeCircleSprite;
    public SpriteRenderer snakeHexagonSprite;
    public SpriteRenderer snakeDiamondSprite;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSprites(active);
    }

    void UpdateSprites(bool active)
    {
        basketSquareSprite.enabled = active;
        basketCircleSprite.enabled = active;
        basketHexagonSprite.enabled = active;
        basketDiamondSprite.enabled = active;
        snakeSquareSprite.enabled = active;
        snakeCircleSprite.enabled = active;
        snakeHexagonSprite.enabled = active;
        snakeDiamondSprite.enabled = active;
    }

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        active = !active;

        UpdateSprites(active);
    }
}
