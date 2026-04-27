public class Graph
{
    public int Rows = 0;
    public int Columns = 0;

    public GraphNode[] Nodes;

    public void Init(int[,] grid)
    {
        Rows = grid.GetLength(0);
        Columns = grid.GetLength(1);

        Nodes = new GraphNode[grid.Length];
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i] = new();
            Nodes[i].Id = i;
        }

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                int index = r * Columns + c;
                Nodes[index].Weight = grid[r, c];

                if (grid[r, c] == -1)
                {
                    continue;
                }

                if (r - 1 >= 0 && grid[r - 1, c] >= 0)
                {
                    Nodes[index].Adjacents.Add(Nodes[index - Columns]); // Up
                }

                if (c + 1 < Columns && grid[r, c + 1] >= 0)
                {
                    Nodes[index].Adjacents.Add(Nodes[index + 1]); // Right
                }

                if (r + 1 < Rows && grid[r + 1, c] >= 0)
                {
                    Nodes[index].Adjacents.Add(Nodes[index + Columns]); // Down
                }

                if (c - 1 >= 0 && grid[r, c - 1] >= 0)
                {
                    Nodes[index].Adjacents.Add(Nodes[index - 1]); // Left
                }
            }
        }
    }

    public void ResetNodePrevious()
    {
        foreach (var Node in Nodes)
        {
            Node.Previous = null;
        }
    }
}