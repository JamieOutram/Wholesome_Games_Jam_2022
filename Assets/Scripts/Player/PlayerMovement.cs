using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(0,1000)]
    private float force = 200;
    private Rigidbody2D rb;
    private Camera playerCam;
    private Camera mapCam;
    bool up;
    bool down;
    bool left;
    bool right;

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
        up = Input.GetKey(KeyCode.W);
        down = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowMap();
        }
        if (!Input.GetKey(KeyCode.M))
        {
            HideMap();
        }
    }

    private void FixedUpdate()
    {
        if(up)
        {
            rb.AddForce(new Vector2(0, force));
        }
        if (down)
        {
            rb.AddForce(new Vector2(0, -force));
        }
        if (left)
        {
            rb.AddForce(new Vector2(-force, 0));
        }
        if (right)
        {
            rb.AddForce(new Vector2(force, 0));
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
