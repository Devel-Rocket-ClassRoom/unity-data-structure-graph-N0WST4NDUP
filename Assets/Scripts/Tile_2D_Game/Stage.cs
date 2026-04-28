using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject TilePrefab;

    private GameObject[] _tileObjects;

    public int MapWidth = 20;
    public int MapHeight = 20;

    [Range(0f, 0.9f)] public float ErodePercent = 0.5f;
    [Min(2)] public int ErodeIterations = 2;
    [Range(0f, 0.1f)] public float LakePercent = 0.2f;
    [Range(0f, 0.3f)] public float TreePercent = 0.2f;
    [Range(0f, 0.2f)] public float HillPercent = 0.2f;
    [Range(0f, 0.1f)] public float MountainPercent = 0.2f;
    [Range(0f, 0.1f)] public float TownPercent = 0.2f;
    [Range(0f, 0.1f)] public float MonsterPercent = 0.2f;

    public Vector2 TileSize = new(16, 16);
    public int FowRange = 3;

    public Sprite[] IslandSprites;
    public Sprite[] FowSprites;

    public GameObject playerPrefab;
    public PlayerMovement player;

    private Map _map;

    public Map Map => _map;
    public GameObject[] TileObjects => _tileObjects;

    public Vector3 FirstTilePos
    {
        get
        {
            var pos = transform.position;
            pos.x -= MapWidth * TileSize.x * 0.5f;
            pos.y += MapHeight * TileSize.y * 0.5f;
            pos.x += TileSize.x * 0.5f;
            pos.y -= TileSize.y * 0.5f;
            return pos;
        }
    }

    private void Start()
    {
        ResetStage();
    }

    public void ResetStage()
    {
        _map = new();
        _map.Init(MapHeight, MapWidth);
        _map.CreateIsLand(
            ErodePercent,
            ErodeIterations,
            LakePercent,
            TreePercent,
            HillPercent,
            MountainPercent,
            TownPercent,
            MonsterPercent
        );
        CreateGrid();
        CreatePlayer();
        UpdateFow(player.transform.position);
        player.OnMoveComplete += UpdateFow;
    }

    private void CreateGrid()
    {
        if (_tileObjects != null)
        {
            foreach (var tile in _tileObjects)
            {
                Destroy(tile.gameObject);
            }
        }

        _tileObjects = new GameObject[MapWidth * MapHeight];

        var position = Vector3.zero;

        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                var tileId = i * MapWidth + j;
                var newGO = Instantiate(TilePrefab, transform);
                newGO.transform.position = FirstTilePos + position;
                position.x += TileSize.x;

                _tileObjects[tileId] = newGO;
                DecorateTile(tileId);
            }
            position.x = 0;
            position.y -= TileSize.y;
        }
    }

    public void DecorateTile(int tileId)
    {
        var tile = _map.Tiles[tileId];
        var tileGO = _tileObjects[tileId];
        var renderer = tileGO.GetComponent<SpriteRenderer>();
        if (tile.AutoTileId != (int)TileTypes.Empty)
        {
            renderer.sprite = _map.Tiles[tileId].IsVisited ? IslandSprites[tile.AutoTileId] : FowSprites[tile.FowTileId];
        }
        else
        {
            renderer.sprite = null;
        }
    }

    private void UpdateFow(Vector3 pos)
    {
        int centerTileId = WorldPosToTileId(pos);
        int tileX = centerTileId % MapWidth;
        int tileY = centerTileId / MapWidth;

        int xMin = Mathf.Max(0, tileX - FowRange);
        int xMax = Mathf.Min(MapWidth, tileX + FowRange + 1);
        int yMin = Mathf.Max(0, tileY - FowRange);
        int yMax = Mathf.Min(MapHeight, tileY + FowRange + 1);

        for (int col = xMin; col < xMax; col++)
        {
            for (int row = yMin; row < yMax; row++)
            {
                var tileId = row * MapWidth + col;
                var tile = _map.Tiles[tileId];
                if (!tile.IsVisited) tile.IsVisited = true;
            }
        }

        int xMinExt = Mathf.Max(0, xMin - 1);
        int xMaxExt = Mathf.Min(MapWidth, xMax + 1);
        int yMinExt = Mathf.Max(0, yMin - 1);
        int yMaxExt = Mathf.Min(MapHeight, yMax + 1);

        for (int col = xMinExt; col < xMaxExt; col++)
        {
            for (int row = yMinExt; row < yMaxExt; row++)
            {
                var tileId = row * MapWidth + col;
                var tile = _map.Tiles[tileId];
                tile.UpdateFowTileId();
                DecorateTile(tileId);
            }
        }
    }

    private void CreatePlayer()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        player = Instantiate(playerPrefab, transform).GetComponent<PlayerMovement>();
        player.Init(_map.StartTile.Id);
        _map.StartTile.IsVisited = true;
    }

    public int ScreenPosToTileId(Vector3 screenPos)
    {
        screenPos.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        return WorldPosToTileId(Camera.main.ScreenToWorldPoint(screenPos));
    }

    public int WorldPosToTileId(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - FirstTilePos.x) / TileSize.x);
        int y = Mathf.RoundToInt((FirstTilePos.y - worldPos.y) / TileSize.y);
        x = Mathf.Clamp(x, 0, MapWidth - 1);
        y = Mathf.Clamp(y, 0, MapHeight - 1);
        return y * MapWidth + x;
    }

    public Vector3 GetTilePos(int y, int x)
    {
        return FirstTilePos + new Vector3(x * TileSize.x, -y * TileSize.y);
    }

    public Vector3 GetTilePos(int tileId)
    {
        var y = tileId / MapWidth;
        var x = tileId % MapWidth;
        return GetTilePos(y, x);
    }
}