using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SimplexNoise;

public class BeaverMapGenerator : MonoBehaviour
{
    public int WIDTH = 89 * 2;
    public int HEIGHT = 100;

    public TileBase waterTile, grassTile, mountainTile, mudTile, startTile, endTile;
    public TileBase leftTile, upLeftTile, upTile, upRightTile, rightTile, downRightTile, downTile, downLeftTile;
    public Tilemap tiles;

    public float waterHeightScale = 0.25f;
    public float mountainHeightScale = 0.8f;

    public float noiseScale = 0.005f;
    public float directionNoise = 2.5f;
    public float momentumChangePenalty = 1.0f;
    public float startWidth = 5.0f;
    public float endWidth = 10.0f;

    public int minSearch = 3;
    public int maxSearch = 10;
    public int minRadius = 5;

    public float carveStrength = 5;
    public int carveRadius = 5;

    private float m_noiseMax;
    private Vector2Int m_noiseMaxLoc;
    private float m_noiseMin;
    private float m_noiseSpan;

    private float[,] m_noiseMap;
    private float[,] m_terrain;
    private bool[,] m_isDam;
    private bool[,] m_isWater;
    private bool[,] m_isMud;
    private bool[,] m_isMountain;

    private int[,] m_distanceFromRiver;
    private Vector2Int[,] m_dirToRiver;

    private bool m_showFlows;

    private List<Vector2Int> m_riverPath = new List<Vector2Int>();

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

    // Start is called before the first frame update
    void Start()
    {
        m_isDam = new bool[WIDTH, HEIGHT];
        m_isWater = new bool[WIDTH, HEIGHT];
        m_isMud = new bool[WIDTH, HEIGHT];
        m_isMountain = new bool[WIDTH, HEIGHT];

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
            }
        }

        SetupRiver();
        DrawRiver();
    }

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

    // Todo: add some sort of shortcut in here for the case where it creates a giant lake!
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

    List<Vector2Int> GetStepsBetween(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> steps = new List<Vector2Int>();
        Vector2Int loc = start;
        while(loc != end)
        {
            Vector2Int remaining = end - loc;
            Vector2Int nextDir = new Vector2Int(
                remaining.x > 0 ? 1 : (remaining.x < 0 ? -1 : 0),
                remaining.y > 0 ? 1 : (remaining.y < 0 ? -1 : 0)
                );

            Debug.Log("Chose direction " + nextDir);

            loc += nextDir;
            steps.Add(loc);
        }

        Debug.Assert(loc == end);
        return steps;
    }

    void SimplifyPath()
    {
        List<Vector2Int> reducedPath = new List<Vector2Int>();

        int itrs = 0;
        bool reducing;
        do
        {
            reducing = false;
            for (int i = 0; i < m_riverPath.Count; i++)
            {
                Vector2Int start = m_riverPath[i];

                reducedPath.Add(start);

                //Debug.Log("Adding " + start + " to path");

                for (int j = maxSearch; j > minSearch; j--)
                {
                    if(i + j >= m_riverPath.Count) { continue; }

                    Vector2Int end = m_riverPath[i + j];

                    if ((start - end).magnitude < minRadius)
                    {
                        Debug.Log("Getting steps between " + start + " and " + end);
                        List<Vector2Int> steps = GetStepsBetween(start, end);
                        Debug.Log("Done in " + steps.Count + " steps");
                        reducedPath.AddRange(steps);
                        i = i + j;
                        reducing = true;
                        break;
                    }
                }
            }
            m_riverPath = reducedPath;
        } while (reducing && false && itrs++ < 1);

        m_isWater = new bool[WIDTH, HEIGHT];

        foreach(Vector2Int loc in m_riverPath)
        {
            m_isWater[loc.x, loc.y] = true;
        }
    }

    float GetMaxHeightOnRiver(ref int idx)
    {
        float maxHeight = 0;
        int i = 0;
        foreach (Vector2Int loc in m_riverPath)
        {
            if (m_noiseMap[loc.x, loc.y] > maxHeight)
            {
                maxHeight = m_noiseMap[loc.x, loc.y];
                idx = i;
            }
            i++;
        }
        return maxHeight;
    }

    int GetMinDistNeighbour(int x, int y)
    {
        int minVal = int.MaxValue;
        foreach(Vector2Int dir in dirs)
        {
            Vector2Int loc = new Vector2Int(x, y) + dir;
            if (OutOfRange(loc)) { continue; }

            minVal = Mathf.Min(minVal, m_distanceFromRiver[x, y]);
        }
        return minVal;
    }

    Vector2Int GetDirToMinDistNeighbour(int x, int y)
    {
        int minVal = int.MaxValue;
        Vector2Int downDir = new Vector2Int(0, 0);
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int loc = new Vector2Int(x, y) + dir;
            if (OutOfRange(loc)) { continue; }

            int dist = m_distanceFromRiver[x, y];
            if (dist < minVal)
            {
                minVal = dist;
                downDir = loc;
            }
            if (dist == minVal)
            {
                downDir += dir;
            }
        }

        if (Mathf.Abs(downDir.x) <= 1 && Mathf.Abs(downDir.y) <= 1)
        {
            return downDir;
        }


        int maxVal = int.MinValue;
        Vector2Int correctedDownDir = new Vector2Int(0, 0);
        foreach (Vector2Int dir in dirs)
        {
            int correlation = dir.x * downDir.x + dir.y * downDir.y;
            if (correlation > maxVal)
            {
                correctedDownDir = dir;
                maxVal = correlation;
            }
        }

        return correctedDownDir;
    }
    void CalculateDistancesFromRiverPath()
    {
        m_distanceFromRiver = new int[WIDTH, HEIGHT];
        
        for (int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                m_distanceFromRiver[x, y] = int.MaxValue;
            }
        }

        foreach (Vector2Int loc in m_riverPath)
        {
            m_distanceFromRiver[loc.x, loc.y] = 0;
        }

        bool bUpdated;
        do
        {
            bUpdated = false;

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    int minVal = GetMinDistNeighbour(x, y);
                    if(minVal + 1 < m_distanceFromRiver[x, y])
                    {
                        bUpdated = true;
                    }
                }
            }
        } while (bUpdated);
    }

    void CalculateDirToRiver()
    {
        m_dirToRiver = new Vector2Int[WIDTH, HEIGHT];
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                m_dirToRiver[x, y] = GetDirToMinDistNeighbour(x, y);
            }
        }
    }

    void ApplyToRadius(Vector2Int centre, int radius, Func<float, float, float> func)
    {
        for (int i = -radius; i <= radius; i++)
        {
            int range = (int)Mathf.Sqrt((radius * radius) - (i * i));
            for (int j = -range; j <= range; j++)
            {
                Vector2Int dir = new Vector2Int(i, j);
                Vector2Int loc = centre + dir;
                if (OutOfRange(loc)) { continue; }
                m_noiseMap[loc.x, loc.y] = func(m_noiseMap[loc.x, loc.y], dir.magnitude);
            }
        }
    }

    void RaiseInitialPath()
    {
        int idx = -1;
        float maxHeight = GetMaxHeightOnRiver(ref idx);
        Debug.Assert(idx >= 0);
        for (int i = 0; i < idx; i++)
        {
            Func<float, float, float> func = (height, radius) => Mathf.Max(HEIGHT, maxHeight) + 20 * (idx - i) / idx;
            ApplyToRadius(m_riverPath[i], 10, func);
        }
    }

    void CarveRiverPath()
    {
        int dist = 0;
        foreach(Vector2Int loc in m_riverPath)
        {
            Func<float, float, float> carver = (height, radius) => height - (dist * Mathf.Max(0, carveRadius - radius));
            ApplyToRadius(loc, carveRadius, carver);
            dist++;
        }    
    }

    Vector2Int GetLocalFlowDir(Vector2Int loc)
    {
        int count = 0;
        return GetLocalFlowDir(loc, ref count);
    }
    Vector2Int GetLocalFlowDir(Vector2Int loc, ref int blockedByDam)
    {
        List<Tuple<Vector2Int, float>> localFlow = new List<Tuple<Vector2Int, float>>();
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int nloc = new Vector2Int(loc.x + dir.x, loc.y + dir.y);
            if (OutOfRange(nloc)) { continue; }
            localFlow.Add(
                new Tuple<Vector2Int, float>(dir, m_noiseMap[loc.x + dir.x, loc.y + dir.y])
                );
        }

        localFlow.Sort((lhs, rhs) => lhs.Item2.CompareTo(rhs.Item2));

        blockedByDam = 0;
        foreach (var tuple in localFlow)
        {
            Vector2Int nloc = new Vector2Int(loc.x + tuple.Item1.x, loc.y + tuple.Item1.y);
            if (OutOfRange(nloc)) { continue; }
            if (m_isDam[nloc.x, nloc.y])
            {
                ++blockedByDam;
                continue;
            }
            return tuple.Item1;
        }
        Debug.Assert(false);
        return Vector2Int.zero;
    }

    int CountDams(Vector2Int loc)
    {
        int blockedByDam = 0;
        GetLocalFlowDir(loc, ref blockedByDam);
        return blockedByDam;
    }

    void FillInAdjacent(ref bool[,] toFill, Vector2Int loc, float threshold, int depth, Func<Vector2Int, int, bool> failFunc, Func<Vector2Int, float> adjustFunc)
    {
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int river = new Vector2Int(loc.x + dir.x, loc.y + dir.y);
            if (OutOfRange(river)) { continue; }
            if (toFill[river.x, river.y]) { continue; }
            if (failFunc(river, depth)) { continue; }
            if(m_isDam[river.x, river.y]) { continue; }

            float height = m_noiseMap[loc.x, loc.y];


            if (height >= threshold) { continue; }
            float adjust = adjustFunc(river);
            if (adjust > 0) { Debug.Log(adjust); }



            toFill[river.x, river.y] = true;
            FillInAdjacent(ref toFill, river, threshold + adjust, depth + 1, failFunc, adjustFunc);
        }
    }

    float DamAdjust(Vector2Int loc)
    {
        return 50;
    }
    void FillInRiver(int maxDepth)
    {
        Func<Vector2Int, int, bool> failFunc = (loc, depth) => m_isWater[loc.x, loc.y] && depth < maxDepth;
        Func<Vector2Int, float> adjustFunc = loc => CountDams(loc) * DamAdjust(loc) - 1.5f;
        for (int i = 0; i < m_riverPath.Count; ++i)
        {
            Vector2Int loc = m_riverPath[i];
            float progress = (float)i / m_riverPath.Count;
            float riverDepth = (startWidth + ((endWidth - startWidth) * progress));
            float threshold = riverDepth + m_noiseMap[loc.x, loc.y];
            FillInAdjacent(ref m_isWater, loc, threshold, 0, failFunc, adjustFunc);
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

    void FillInBanks(Vector2Int loc)
    {
        Func<Vector2Int, int, bool> failFunc = (loc, depth) => m_isWater[loc.x, loc.y]&& depth < 4;
        Func<Vector2Int, float> adjustFunc = loc => -1.5f;
        float threshold = m_noiseMap[loc.x, loc.y] + 0.01f * (loc - m_riverPath[0]).magnitude;
        FillInAdjacent(ref m_isMud, loc, threshold, 0, failFunc, adjustFunc);
    }

    void AddMud()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                Vector2Int loc = new Vector2Int(x, y);
                if (!m_isWater[x, y] && CountNeighbours(loc) > 2)
                {
                    m_isMud[x, y] = true;
                    FillInBanks(loc);
                }
            }
        }
    }

    void AddMountains()
    {
        float maxDist = Mathf.Sqrt(HEIGHT * HEIGHT + WIDTH * WIDTH);
        float mountainHeight = m_noiseMin + (m_noiseSpan * mountainHeightScale);
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (m_isMud[x, y] || m_isWater[x, y]) { continue; }
                Vector2Int loc = new Vector2Int(x, y);
                float distToStart = (m_riverPath[0] - loc).magnitude / maxDist;
                float distToEnd = (m_riverPath[m_riverPath.Count - 1] - loc).magnitude / maxDist;

                float height = m_noiseMap[x, y] * 0.5f + (distToEnd - distToStart) * 0.5f;
                m_isMountain[x, y] |= height >= mountainHeight;
            }
        }
    }

    TileBase GetTileForDir(Vector2Int dir)
    {
        if (dir.x == 1 && dir.y == 1)
        {
            return upRightTile;
        }
        if (dir.x == 1 && dir.y == 0)
        {
            return rightTile;
        }
        if (dir.x == 1 && dir.y == -1)
        {
            return downRightTile;
        }
        if (dir.x == 0 && dir.y == 1)
        {
            return upTile;
        }
        if (dir.x == 0 && dir.y == 0)
        {
            return waterTile;
        }
        if (dir.x == 0 && dir.y == -1)
        {
            return downTile;
        }
        if (dir.x == -1 && dir.y == 1)
        {
            return upLeftTile;
        }
        if (dir.x == -1 && dir.y == 0)
        {
            return leftTile;
        }
        if (dir.x == -1 && dir.y == -1)
        {
            return downLeftTile;
        }
        return waterTile;
    }

    // Update is called once per frame
    void SetupRiver()
    {

        CreateIntialPath();
        SimplifyPath();
        RaiseInitialPath();
        CarveRiverPath();

        FillInRiver(25);
        AddMud();
        AddMountains();
    }

    void DrawRiver()
    { 
        for (int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                if (m_isDam[x, y])
                {
                    tiles.SetTile(new Vector3Int(x, y, 0), startTile);
                }
                else if (m_isWater[x, y])
                {
                    if (!m_showFlows || x % 2 != 0 || y % 2 != 0)
                    {
                        tiles.SetTile(new Vector3Int(x, y, 0), waterTile);
                    }
                    else
                    {
                        Vector2Int loc = new Vector2Int(x, y);
                        Vector2Int dir = GetLocalFlowDir(loc);
                        tiles.SetTile(new Vector3Int(x, y, 0), GetTileForDir(dir));
                    }
                }
                else if (m_isMud[x, y])
                {
                    tiles.SetTile(new Vector3Int(x, y, 0), mudTile);
                }
                else if (m_isMountain[x, y])
                {
                    tiles.SetTile(new Vector3Int(x, y, 0), mountainTile);
                }
                else
                {
                    tiles.SetTile(new Vector3Int(x, y, 0), grassTile);
                }
            }
        }
    }

    void RecalculateForNewDam()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                m_isWater    [x, y] = false;
                m_isMud      [x, y] = false;
                m_isMountain [x, y] = false;
            }
        }
        FillInRiver(25);
        AddMud();
    }

    public void AddLogsAtLoc(Vector3Int start, Vector3Int end)
    {
        Vector2Int start2d = new Vector2Int(start.x, start.y);
        Vector2Int end2d = new Vector2Int(end.x, end.y);

        List<Vector2Int> locs = GetStepsBetween(start2d, end2d);

        foreach (Vector2Int loc in locs)
        {
            m_isDam [loc.x, loc.y]= true;
        }

        RecalculateForNewDam();
        DrawRiver();
    }

    public void ToggleFlows()
    {
        m_showFlows = !m_showFlows;
        DrawRiver();
    }

}
