using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class FlowFieldButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update

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
        mapgen.ToggleFlows();
    }
}
