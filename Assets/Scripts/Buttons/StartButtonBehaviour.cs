using UnityEngine;

public class StartButtonBehaviour : MonoBehaviour
{
    public void OnPress()
    {
        Debug.Log("Start Pressed");
        TransitionBehaviour.TriggerFade("Village");
    }
}
