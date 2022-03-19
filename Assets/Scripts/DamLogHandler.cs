using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DamLogHandler : MonoBehaviour
{
    public Texture2D tex;
    public Camera cam;
    public GameObject LogPrefab;
    public Tilemap tilemap;

    // Start is called before the first frame update

    private List<GameObject> m_logs = new List<GameObject>();
    private GameObject m_currentLog;
    private bool m_hasActiveLog = false;
    private bool m_lock = false;

    private BeaverMapGenerator mapgen;

    void Start()
    {
        mapgen = tilemap.GetComponent<BeaverMapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_hasActiveLog && !Input.GetMouseButtonDown(0))
        {
            Vector3 screenSpace = cam.ScreenToWorldPoint(Input.mousePosition);
            m_currentLog.GetComponent<DamLog>().MoveSprite(screenSpace);
        }
        else if (!m_lock && Input.GetMouseButtonDown(0))
        {
            NotfiyMapgenOfNewLog();
            m_logs.Add(m_currentLog);
            m_hasActiveLog = false;
        }

        if (m_hasActiveLog && Input.GetKeyDown(KeyCode.Q))
        {
            m_currentLog.GetComponent<DamLog>().RotateSprite(-45);
        }
        if (m_hasActiveLog && Input.GetKeyDown(KeyCode.R))
        {
            m_currentLog.GetComponent<DamLog>().RotateSprite(45);
        }
    }

    public void CreateLogOnMouse()
    {
        if (!m_hasActiveLog)
        {
            m_currentLog = Instantiate(LogPrefab, new Vector3(1000.0f, 1000.0f, 0.0f), Quaternion.identity);
            m_hasActiveLog = true;
            m_lock = true;
        }
    }

    public void Unlock()
    {
        m_lock = false;
    }

    void NotfiyMapgenOfNewLog()
    {
        if (!m_hasActiveLog) { return; }

        SpriteRenderer sprite = m_currentLog.GetComponent<SpriteRenderer>();
        Vector3 lengthVec = new Vector3(0.0f, sprite.bounds.size.y  * 0.5f, 0.0f);
        Vector3 start = sprite.transform.position - (sprite.transform.rotation * lengthVec);
        Vector3 end = sprite.transform.position + (sprite.transform.rotation * lengthVec);

        Vector3Int startTmap = tilemap.WorldToCell(start);
        Vector3Int endTmap = tilemap.WorldToCell(end);

        mapgen.AddLogsAtLoc(startTmap, endTmap);
    }
}
