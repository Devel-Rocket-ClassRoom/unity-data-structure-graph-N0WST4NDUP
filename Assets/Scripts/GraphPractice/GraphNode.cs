using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    public int Id;
    public int Weight;

    public GraphNode Previous = null;
    public List<GraphNode> Adjacents = new();

    public bool CanVisit => Adjacents.Count > 0 && Weight > 0;
}
