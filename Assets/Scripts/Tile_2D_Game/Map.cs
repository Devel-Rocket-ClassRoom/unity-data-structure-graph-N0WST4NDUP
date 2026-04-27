public enum TileTypes
{
    Empty = -1,
    Grass = 15,
    Tree,
    Hills,
    Mountains,
    Towns,
    Castle,
}

public class Map
{
    public int Rows = 0;
    public int Columns = 0;

    public Tile[] Tiles;

    public void Init(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;

        Tiles = new Tile[rows * cols];
        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i] = new();
            Tiles[i].Id = i;
        }

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                int index = r * Columns + c;
                var adgacents = Tiles[index].Adjacents;
                if (r - 1 >= 0)
                {
                    adgacents[(int)Sides.Top] = Tiles[index - Columns]; // Up
                }

                if (c + 1 < Columns)
                {
                    adgacents[(int)Sides.Right] = Tiles[index + 1]; // Right
                }

                if (c - 1 >= 0)
                {
                    adgacents[(int)Sides.Left] = Tiles[index - 1]; // Left
                }

                if (r + 1 < Rows)
                {
                    adgacents[(int)Sides.Bottom] = Tiles[index + Columns]; // Down
                }
            }
        }

        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i].UpdateAutoTileId();
        }
    }
}