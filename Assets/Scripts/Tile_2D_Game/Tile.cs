using UnityEngine;

public enum Sides
{
    None = -1,
    Top,
    Left,
    Right,
    Bottom,
}

public class Tile
{
    public int Id;
    public Tile[] Adjacents = new Tile[4];
    public int AutoTileId;
    public int FowTileId = 15;

    public bool IsVisited = false;
    public bool CanMove => AutoTileId != (int)TileTypes.Empty;

    public void UpdateAutoTileId()
    {
        AutoTileId = 0;
        for (int i = 0; i < Adjacents.Length; i++)
        {
            if (Adjacents[i] != null)
            {
                AutoTileId |= 1 << i;
            }
        }
    }

    public void UpdateFowTileId()
    {
        FowTileId = 0;
        for (int i = 0; i < Adjacents.Length; i++)
        {
            if (Adjacents[i] == null || !Adjacents[i].IsVisited)
            {
                FowTileId |= 1 << i;
            }
        }
    }

    public void RemoveAdjacents(Tile tile)
    {
        for (int i = 0; i < Adjacents.Length; i++)
        {
            if (Adjacents[i] == null) continue;

            if (Adjacents[i].Id == tile.Id)
            {
                Adjacents[i] = null;
                UpdateAutoTileId();
                break;
            }
        }
    }

    public void ClearAdjacents()
    {
        for (int i = 0; i < Adjacents.Length; i++)
        {
            if (Adjacents[i] == null) continue;

            Adjacents[i].RemoveAdjacents(this);
            Adjacents[i] = null;
        }

        UpdateAutoTileId();
    }
}
