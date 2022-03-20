using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CreateDamLog : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update

    private Button button;
    private TextMeshPro textMesh;
    private DamLogHandler damLogHandler;
    void Start()
    {
        button = GetComponent<Button>();
        damLogHandler = GameObject.Find("LogHandler").GetComponent<DamLogHandler>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        damLogHandler.CreateLogOnMouse();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        damLogHandler.Unlock();
    }
}
