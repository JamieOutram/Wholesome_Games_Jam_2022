using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(0,2000)]
    private float force = 100;
    private Rigidbody2D rb;
    private Camera playerCam;
    private Camera mapCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCam = GetComponentInChildren<Camera>();
        mapCam = GameObject.FindGameObjectsWithTag("Map Cam")[0].GetComponent<Camera>();
        HideMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector2(0, force));
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector2(0, -force));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-force, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(force, 0));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowMap();
        }
        if (!Input.GetKey(KeyCode.M))
        {
            HideMap();
        }
    }

    void ShowMap()
    {
        if(mapCam.enabled == false)
        {
            mapCam.enabled = true;
            playerCam.enabled = false;
        }
    }
    void HideMap()
    {
        if (mapCam.enabled == true)
        {
            playerCam.enabled = true;
            mapCam.enabled = false;
        }
    }
}
