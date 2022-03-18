using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remove_wool : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "wool")
        {
            print("colliding");
            Destroy(other.gameObject);
        }
    }
}
