using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class ShowRiverPathButton : MonoBehaviour, IPointerDownHandler
{
    public Tilemap tilemap;
    private BeaverMapGenerator mapgen;
    void Start()
    {
        mapgen = tilemap.GetComponent<BeaverMapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mapgen.ToggleRiverPath();
    }
}
