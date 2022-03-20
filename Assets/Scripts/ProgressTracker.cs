using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public bool snake = false;
    public bool mouse = false;
    public bool beaver = false;
    public bool shark = false;
    public bool sheep = false;
    public static ProgressTracker Instance;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
