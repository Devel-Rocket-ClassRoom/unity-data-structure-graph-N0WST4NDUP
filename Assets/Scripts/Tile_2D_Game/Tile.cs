using UnityEngine;

public enum Sides
{
    Bottom,
    Right,
    Left,
    Top,
}

public class Tile
{
    public int Id;
    public Tile[] Adjacents = new Tile[4];
    public int AutoTileId;

    public bool IsVisited = false;

    public void UpdateAutoTileId()
    {
        AutoTileId = 0;
        for (int i = 0; i < Adjacents.Length; i++)
        {
            if (Adjacents[i] != null)
            {
                // 1000 Bottom
                // 0100 Right
                // 0010 Left
                // 0001 Top

                AutoTileId |= 1 << Adjacents.Length - 1 - i;
            }
        }
    }
}
