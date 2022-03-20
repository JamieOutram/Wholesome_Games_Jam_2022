using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBehaviour : MonoBehaviour
{
    static Animator anim;
    static string nextScene;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("FadeClear");
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Black"))
        {
            if (nextScene != null)
                SceneManager.LoadScene(nextScene);
            anim.SetTrigger("FadeClear");
        }
    }

    public static void TriggerFade(string scene)
    {
        nextScene = scene;
        anim.SetTrigger("FadeBlack");
    }
}
