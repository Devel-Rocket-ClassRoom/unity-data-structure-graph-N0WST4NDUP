using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
{
    public enum Algorithm
    {
        DFS,
        BFS,
        DFS_RECURSIVE,
        BFS_PATHFINDING,
        DIJKSTRA,
        ASTAR
    }

    public Transform UINodeRoot;

    public UIGraphNode NodePrefab;
    private List<UIGraphNode> _uiNodes = new();

    private Graph _graph;

    public Algorithm algorithm = Algorithm.DFS;
    public int StartId;
    public int EndId;

    private void Start()
    {
        int[,] map = new int[5, 5]
        {
            { 1,  3, -1,  1,  1},
            { 1,  1, 1,  5,  1},
            { 2, -1,  6,  1,  1},
            { 1,  1,  1, -1,  2},
            { 1,  4,  1,  1,  1}
        };
        _graph = new();
        _graph.Init(map);
        InitUINodes(_graph);
    }

    private void InitUINodes(Graph graph)
    {
        foreach (var node in graph.Nodes)
        {
            var uiNode = Instantiate(NodePrefab, UINodeRoot);
            uiNode.SetNode(node);
            uiNode.Reset();
            _uiNodes.Add(uiNode);
        }
    }

    private void ResetUINodes()
    {
        foreach (var uiNode in _uiNodes)
        {
            uiNode.Reset();
        }
    }

    [ContextMenu("Search")]
    public void Search()
    {
        GraphSearch search = new();
        search.Init(_graph);

        switch (algorithm)
        {
            case Algorithm.DFS:
                search.DFS(_graph.Nodes[StartId]);
                break;
            case Algorithm.BFS:
                search.BFS(_graph.Nodes[StartId]);
                break;
            case Algorithm.DFS_RECURSIVE:
                search.RecursiveDFS(_graph.Nodes[StartId]);
                break;
            case Algorithm.BFS_PATHFINDING:
                search.PathFindingBFS(_graph.Nodes[StartId], _graph.Nodes[EndId]);
                break;
            case Algorithm.DIJKSTRA:
                search.Dijkstra(_graph.Nodes[StartId], _graph.Nodes[EndId]);
                break;
            case Algorithm.ASTAR:
                search.AStar(_graph.Nodes[StartId], _graph.Nodes[EndId]);
                break;
            default:
                break;
        }

        ResetUINodes();
        if (search.Path.Count <= 1)
        {
            if (search.Path.Count == 1)
            {
                var only = search.Path[0];
                _uiNodes[only.Id].SetColor(Color.red);
            }

            return;
        }

        for (int i = 0; i < search.Path.Count; i++)
        {
            var node = search.Path[i];
            var color = Color.Lerp(Color.red, Color.green, (float)i / (search.Path.Count - 1));
            _uiNodes[node.Id].SetColor(color);
            _uiNodes[node.Id].SetText($"ID: {node.Id}\nWeight: {node.Weight}\nPath: {i}");
        }
    }
}