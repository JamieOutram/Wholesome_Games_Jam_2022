using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class check_1 : MonoBehaviour
{
    public static bool[] coll = new bool[4];

    private bool IsAllDone()
    {
        for(int i = 0; i < coll.Length; i++)
        {
            if(coll[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "snake1")
        {
            coll[0] = true;
            if(IsAllDone())
            {
                Debug.Log("All Done!");
                SceneManager.LoadScene("Village");
            }
        }
    }
}
