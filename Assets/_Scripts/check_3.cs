using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class check_3 : MonoBehaviour
{
    private bool IsAllDone()
    {
        for(int i = 0; i < check_1.coll.Length; i++)
        {
            if(check_1.coll[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "snake3")
        {
            check_1.coll[0] = true;
            Debug.Log("snake3");
            if(IsAllDone())
            {
                Debug.Log("All Done!");
                //SceneManager.LoadScene("SnakeDone");
            }
        }
    }
}
