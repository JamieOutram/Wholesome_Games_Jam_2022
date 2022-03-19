using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SimplexNoise;

public class BeaverMapGenerator : MonoBehaviour
{
    public int WIDTH = 89 * 2;
    public int HEIGHT = 100;

    public TileBase waterTile, grassTile, brownTile;
    public Tilemap tiles;

    public float waterHeightScale = 0.25f;
    public float noiseScale = 0.005f;
    public float directionNoise = 2.5f;
    public float momentumChangePenalty = 1.0f;
    public float startWidth = 5.0f;
    public float endWidth = 10.0f;

    private float[,] m_noiseMap;
    private float m_noiseMax;
    private Vector2Int m_noiseMaxLoc;
    private float m_noiseMin;
    private Vector2Int m_noiseMinLoc;
    private float m_noiseSpan;
    private float m_threshold;
    private float m_prev_threshold = -0.1f;

    private bool[,] m_isWater;
    private List<Vector2Int> m_riverPath = new List<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        m_isWater = new bool[WIDTH, HEIGHT];

        //Random.InitState(20);
        Noise.Seed = (int)System.DateTime.Now.Ticks;
        m_noiseMap = Noise.Calc2D(WIDTH, HEIGHT, noiseScale);

        m_noiseMin = 256.0f;
        m_noiseMax = 0.0f;

        int horiztonalBuffer = WIDTH / 4;
        int verticalBuffer = HEIGHT / 4;

        for (int x = horiztonalBuffer; x < WIDTH - horiztonalBuffer; x++)
        {
            for (int y = verticalBuffer; y < HEIGHT - verticalBuffer; y++)
            {
                float noise = m_noiseMap[x, y]; 
                if (m_noiseMax < noise)
                {
                    m_noiseMax = noise;
                    m_noiseMaxLoc = new Vector2Int(x, y);
                }
                if (m_noiseMin > noise)
                {
                    m_noiseMin = noise;
                    m_noiseMinLoc = new Vector2Int(x, y);
                }
            }
        }

        m_noiseSpan = m_noiseMax - m_noiseMin;
    }

    static List<Vector2Int> dirs = new List<Vector2Int>
    { 
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
    };

    bool OutOfRange(Vector2Int loc)
    {
        return loc.x < 0 || loc.x >= WIDTH || loc.y < 0 || loc.y >= HEIGHT;
    }

    bool IsEdge(Vector2Int loc)
    {
        return loc.x == 0 || loc.x == WIDTH - 1 || loc.y == 0 || loc.y == HEIGHT - 1;
    }

    int CountNeighbours(Vector2Int loc)
    {
        int count = 0;
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int neighbour = loc + dir;
            if (OutOfRange(neighbour)) { continue; }

            count += m_isWater[neighbour.x, neighbour.y] ? 1 : 0;
        }
        return count;
    }

    Vector2Int MomentumToDir(Vector2 momentum)
    {
        return new Vector2Int(
        momentum.x > 0.5 ? 1 : (momentum.x < -0.5 ? -1 : 0),
        momentum.y > 0.5 ? 1 : (momentum.y < -0.5 ? -1 : 0)
        );
    }

    //void TraverseToEdge(Vector2 momentum, Vector2Int loc, bool append)
    //{
    //    List<Vector2Int> path = new List<Vector2Int>();
    //    Debug.Log("Traversing from " + loc + " to edge");
    //    int itr = 0;
    //    while (itr++ < 10000)
    //    {
    //        m_isWater[loc.x, loc.y] = true;
    //        path.Add(loc);

    //        if (IsEdge(loc)) { Debug.Log("Found Edge at " + loc);  break; }

    //        float distToMax = (loc - m_noiseMaxLoc).magnitude;
    //        float distToMin = (loc - m_noiseMinLoc).magnitude;
    //        float noise = float.MaxValue;

    //        Vector2Int newDir = new Vector2Int(0, 0);
    //        Vector2Int prevLoc = loc;
    //        foreach (Vector2Int dir in dirs)
    //        {
    //            Vector2Int nloc = prevLoc + dir;
    //            if (OutOfRange(nloc)) { continue; }
    //            if (m_isWater[nloc.x, nloc.y]) { continue; }
    //            if(CountNeighbours(nloc) > 3) { continue; }

    //            float ndistToMin = (nloc - m_noiseMinLoc).magnitude;
    //            float ndistToMax = (nloc - m_noiseMaxLoc).magnitude;

    //            float nlocNoise = append ? m_noiseMap[nloc.x, nloc.y] : 256.0f -m_noiseMap[nloc.x, nloc.y];
    //            float momentumChangeScore = (momentum - dir).magnitude * momentumChangePenalty;
    //            float disruption = Random.Range(-directionNoise, directionNoise);
    //            float distPenalty = (ndistToMax - distToMax + ndistToMin - distToMin) * 0.0f;

    //            float value = distPenalty + nlocNoise + disruption + momentumChangeScore;

    //            if (value < noise)
    //            {
    //                loc = nloc;
    //                noise = value;
    //                newDir = dir;
    //            }
    //        }

    //        momentum = momentum + newDir;
    //        momentum /= momentum.magnitude;

    //        if (loc == prevLoc)
    //        {
    //            loc = prevLoc + MomentumToDir(momentum);
    //        }
    //    }
    //    Debug.Log("Ended at " + loc + " after " + itr + " iterations");

    //    if (append)
    //    {
    //        m_riverPath.AddRange(path);
    //    }
    //    else
    //    {
    //        path.Reverse();
    //        path.AddRange(m_riverPath);
    //        m_riverPath = path;
    //    }
    //}

    //Vector2Int TraverseMaxToMin(ref Vector2Int momentum)
    //{
    //    Vector2Int startDir = new Vector2Int(0, 0);
    //    Vector2Int loc = m_noiseMaxLoc;

    //    Debug.Log("Traversing between " + m_noiseMaxLoc + " and " + m_noiseMinLoc);

    //    int itr = 0;
    //    while (itr++ < 10000)
    //    {
    //        if (loc == m_noiseMinLoc) { break; }

    //        m_isWater[loc.x, loc.y] = true;
    //        m_riverPath.Add(loc);

    //        float dist = (loc - m_noiseMinLoc).magnitude;
    //        float noise = float.MaxValue;

    //        Vector2Int prevLoc = loc;
    //        foreach (Vector2Int dir in dirs)
    //        {
    //            Vector2Int nloc = prevLoc + dir;
    //            if (OutOfRange(nloc)) { continue; }
    //            if (m_isWater[nloc.x, nloc.y]) { continue; }
    //            if(CountNeighbours(nloc) > 2) { continue; }

    //            float ndist = (nloc - m_noiseMinLoc).magnitude;
    //            if (ndist > dist) { continue; }

    //            float momentumChange = (momentum - dir).magnitude;
    //            float value = m_noiseMap[nloc.x, nloc.y] + Random.Range(-directionNoise, directionNoise) + momentumChange * momentumChangePenalty;
    //            if (value < noise)
    //            {
    //                loc = nloc;
    //                noise = value;

    //                if(itr == 0) { startDir = dir; }

    //                momentum = dir;
    //            }
    //        }
    //    }

    //    Debug.Log("Done in " + itr + " iterations");
    //    return startDir;
    //}
    void CreateIntialPath()
    {
        Vector2Int loc = m_noiseMaxLoc;

        Debug.Log("Starting downhill 1 at " + loc);
        int itrs = 0;
        while (itrs++ < 10000)
        {
            m_riverPath.Add(loc);
            m_isWater[loc.x, loc.y] = true;

            if (IsEdge(loc)) { break; }

            float height = float.MaxValue;
            Vector2Int prevLoc = loc;

            foreach(Vector2Int dir in dirs)
            {
                Vector2Int nextLoc = prevLoc + dir;
                if(OutOfRange(nextLoc)) { continue; }
                if (m_isWater[nextLoc.x, nextLoc.y]) { continue; }
                float nextHeight = m_noiseMap[nextLoc.x, nextLoc.y];

                if(nextHeight < height)
                {
                    height = nextHeight;
                    loc = nextLoc;
                }            
            }
        }
        Debug.Log("Ended at: " + loc);

        loc = m_noiseMaxLoc;
        List<Vector2Int> backPath = new List<Vector2Int>();
        itrs = 0;
        while (itrs++ < 10000)
        {
            if (itrs != 0)
            {
                backPath.Add(loc);
            }
            m_isWater[loc.x, loc.y] = true;

            if (IsEdge(loc)) { break; }

            float height = float.MaxValue;
            Vector2Int prevLoc = loc;
            float distToEnd = (prevLoc - m_riverPath[m_riverPath.Count - 1]).magnitude;

            foreach (Vector2Int dir in dirs)
            {
                Vector2Int nextLoc = prevLoc + dir;
                if (OutOfRange(nextLoc)) { continue; }
                if (m_isWater[nextLoc.x, nextLoc.y]) { continue; }

                float nextDistToEnd = (nextLoc - m_riverPath[m_riverPath.Count - 1]).magnitude;
                if(nextDistToEnd < distToEnd) { continue; }

                float nextHeight = m_noiseMap[nextLoc.x, nextLoc.y];

                if (nextHeight < height)
                {
                    height = nextHeight;
                    loc = nextLoc;
                }
            }

            Debug.Assert(loc != prevLoc);
        }

        Debug.Log("Ended Downhill 2 at " + loc);

        backPath.Reverse();
        backPath.AddRange(m_riverPath);
        m_riverPath = backPath;
    }

    void FillInAdjacent(Vector2Int loc, float threshold)
    {
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int river = new Vector2Int(loc.x + dir.x, loc.y + dir.y);
            if (OutOfRange(river)) { continue; }
            if (m_isWater[river.x, river.y]) { continue; }

            float height = m_noiseMap[loc.x, loc.y];


            if (height >= threshold) { continue; }

            m_isWater[river.x, river.y] = true;
            FillInAdjacent(river, threshold - 1.5f);
        }
    }

    void FillInRiver()
    {
        for (int i = 0; i < m_riverPath.Count; ++i)
        {
            Vector2Int loc = m_riverPath[i];
            float progress = (float)i / m_riverPath.Count;
            float riverDepth = (startWidth + ((endWidth - startWidth) * progress));
            float threshold = riverDepth + m_noiseMap[loc.x, loc.y];
            FillInAdjacent(loc, threshold);
        }

        bool bModified = true;
        while (bModified)
        {
            bModified = false;
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (m_isWater[x, y]) { continue; }
                    Vector2Int loc = new Vector2Int(x, y);
                    bool bFill = CountNeighbours(loc) > 4;
                    m_isWater[x, y] |= bFill;
                    bModified |= bFill;
                }
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_threshold = m_noiseMin + m_noiseSpan * waterHeightScale;
        if (m_threshold == m_prev_threshold) { return; }

        Debug.Log("updating");

        m_prev_threshold = m_threshold;

        Vector2Int momentum = new Vector2Int(0, 0);

        //Vector2Int startMomentum = TraverseMaxToMin(ref momentum);
        //TraverseToEdge(-startMomentum, m_noiseMaxLoc, false);
        //TraverseToEdge(momentum, m_noiseMinLoc, true);

        CreateIntialPath();

        //Vector2Int lastLoc = m_riverPath[0];
        //foreach(Vector2Int loc in m_riverPath)
        //{
        //    Debug.Assert((loc - lastLoc).magnitude <= Mathf.Sqrt(2));
        //    Debug.Log(loc);
        //    lastLoc = loc;
        //}

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (!m_isWater[x, y]) { continue; }
                tiles.SetTile(new Vector3Int(x, y, 0), brownTile);
            }
        }

        FillInRiver();

        for (int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                Vector2Int loc = new Vector2Int(x, y);
                if (tiles.GetTile(new Vector3Int(x, y, 0)) == brownTile){ continue; }
                tiles.SetTile(new Vector3Int(x, y, 0), m_isWater[x, y] ? waterTile : grassTile);
            }
        }
    }
}
