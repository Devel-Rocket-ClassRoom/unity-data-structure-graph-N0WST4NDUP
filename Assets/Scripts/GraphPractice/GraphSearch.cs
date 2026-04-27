using System.Collections.Generic;
using UnityEngine;

public class GraphSearch
{
    private Graph _graph;
    public List<GraphNode> Path = new();

    public void Init(Graph graph)
    {
        _graph = graph;
    }

    public void DFS(GraphNode node)
    {
        Path.Clear();
        _graph.ResetNodePrevious();

        Stack<GraphNode> stack = new();
        HashSet<GraphNode> visited = new();

        stack.Push(node);
        visited.Add(node);

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            Path.Add(currentNode);

            foreach (var adjacent in currentNode.Adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent)) continue;

                stack.Push(adjacent);
                visited.Add(adjacent);
            }
        }
    }

    public void BFS(GraphNode node)
    {
        Path.Clear();
        _graph.ResetNodePrevious();

        Queue<GraphNode> que = new();
        HashSet<GraphNode> visited = new();

        que.Enqueue(node);
        visited.Add(node);

        while (que.Count > 0)
        {
            var currentNode = que.Dequeue();
            Path.Add(currentNode);

            foreach (var adjacent in currentNode.Adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent)) continue;

                que.Enqueue(adjacent);
                visited.Add(adjacent);
            }
        }
    }

    public void RecursiveDFS(GraphNode node, int depth = 0)
    {
        if (depth == 0)
        {
            Path.Clear();
            _graph.ResetNodePrevious();
        }

        Path.Add(node);

        foreach (var adjacent in node.Adjacents)
        {
            if (adjacent.CanVisit && !Path.Contains(adjacent))
            {
                RecursiveDFS(adjacent, depth + 1);
            }
        }
    }

    public void PathFindingBFS(GraphNode startNode, GraphNode endNode)
    {
        Path.Clear();
        _graph.ResetNodePrevious();

        Queue<GraphNode> que = new();
        HashSet<GraphNode> visited = new();

        que.Enqueue(startNode);
        visited.Add(startNode);

        while (que.Count > 0)
        {
            var currentNode = que.Dequeue();

            if (currentNode == endNode)
            {
                while (currentNode.Previous != null)
                {
                    Path.Add(currentNode);
                    currentNode = currentNode.Previous;
                }
                Path.Add(currentNode);
                return;
            }

            foreach (var adjacent in currentNode.Adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent)) continue;

                que.Enqueue(adjacent);
                visited.Add(adjacent);
                adjacent.Previous = currentNode;
            }
        }
    }

    public void Dijkstra(GraphNode startNode, GraphNode endNode)
    {
        Path.Clear();
        _graph.ResetNodePrevious();

        PriorityQueue<GraphNode, int> pq = new();
        HashSet<GraphNode> visited = new();
        int[] dist = new int[_graph.Nodes.Length];
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = int.MaxValue;
        }
        dist[startNode.Id] = 0;

        pq.Enqueue(startNode, 0);

        while (pq.Count > 0)
        {
            var currentNode = pq.Dequeue();
            if (visited.Contains(currentNode)) continue;
            visited.Add(currentNode);

            if (currentNode == endNode)
            {
                while (currentNode.Previous != null)
                {
                    Path.Add(currentNode);
                    currentNode = currentNode.Previous;
                }
                Path.Add(currentNode);
                return;
            }

            foreach (var adjacent in currentNode.Adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent)) continue;

                int newDist = dist[currentNode.Id] + adjacent.Weight;
                if (newDist < dist[adjacent.Id])
                {
                    dist[adjacent.Id] = newDist;
                    pq.Enqueue(adjacent, newDist);
                    adjacent.Previous = currentNode;
                }
            }
        }
    }

    public void AStar(GraphNode startNode, GraphNode endNode)
    {
        Path.Clear();
        _graph.ResetNodePrevious();

        PriorityQueue<GraphNode, int> pq = new();
        HashSet<GraphNode> visited = new();
        int[] dist = new int[_graph.Nodes.Length];
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = int.MaxValue;
        }
        dist[startNode.Id] = 0;

        pq.Enqueue(startNode, 0);

        while (pq.Count > 0)
        {
            var currentNode = pq.Dequeue();
            if (visited.Contains(currentNode)) continue;
            visited.Add(currentNode);

            if (currentNode == endNode)
            {
                while (currentNode.Previous != null)
                {
                    Path.Add(currentNode);
                    currentNode = currentNode.Previous;
                }
                Path.Add(currentNode);
                return;
            }

            foreach (var adjacent in currentNode.Adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent)) continue;

                int newDist = dist[currentNode.Id] + adjacent.Weight;
                if (newDist < dist[adjacent.Id])
                {
                    dist[adjacent.Id] = newDist;
                    pq.Enqueue(adjacent, newDist + Heuristic(adjacent, endNode));
                    adjacent.Previous = currentNode;
                }
            }
        }
    }

    private int Heuristic(GraphNode a, GraphNode b)
    {
        int ax = a.Id % _graph.Columns;
        int ay = a.Id / _graph.Columns;

        int bx = b.Id % _graph.Columns;
        int by = b.Id / _graph.Columns;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }
}