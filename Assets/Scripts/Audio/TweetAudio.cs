using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweetAudio : MonoBehaviour
{
    AudioSource audio;
    public Vector2 delayRangeSeconds;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        Scedule();
    }

    void Tweet()
    {
        audio.Play();
        Scedule();
    }

    void Scedule()
    {
        Invoke(nameof(Tweet), Random.Range(delayRangeSeconds.x, delayRangeSeconds.y));
    }
}
