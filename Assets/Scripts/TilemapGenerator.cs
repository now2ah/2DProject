using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class TilemapData
{
    int _xSize;
    int _ySize;

    int[,] _tileData;

    public int XSize => _xSize;
    public int YSize => _ySize;

    public TilemapData(int xSize, int ySize)
    {
        _xSize = xSize;
        _ySize = ySize;

        _tileData = new int[ySize, xSize];
    }

    public int GetData(int x, int y)
    {
        if (_IsValid(x, y))
            return _tileData[y, x];
        else
            return -1;
    }

    public void SetData(int x, int y, int data)
    {
        if (null == _tileData)
            return;

        if (_IsValid(x, y))
        {
            _tileData[y, x] = data;
        }
    }

    bool _IsValid(int x, int y)
    {
        if (x >= _xSize || x < 0)
            return false;

        if (y >= _ySize || y < 0)
            return false;

        return true;
    }
}

public class TilemapGenerator : MonoBehaviour
{
    public int GROUND_Y = 4;
    public int holePerPartTilemap = 1;
    public int xSize = 10;
    public int ySize = 10;

    public TileBase ruleTile;

    GameObject _gridObject;
    Grid _grid;
    TilemapData _tilemapData;

    GameObject _tilemapObject;
    Tilemap _tileMap;


    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public void GenerateFlatTerrainTilemap()
    {
        if (null == _tileMap)
        {
            _GenerateGrid();
            _GenerateTilemap();
            _tilemapData = _GenerateNewTilemap(xSize, ySize);
            _GenerateFlatTerrainTilemapData(_tilemapData);
            _PaintTiles(_tileMap, _tilemapData, ruleTile);
        }
        else
        {
            _ClearTiles();
            _tilemapData = _GenerateNewTilemap(xSize, ySize);
            _GenerateFlatTerrainTilemapData(_tilemapData);
            _PaintTiles(_tileMap, _tilemapData, ruleTile);
        }
    }

    public void GenerateHeightTerrainTilemap()
    {
        if (null == _tileMap)
        {
            _GenerateGrid();
            _GenerateTilemap();
            _tilemapData = _GenerateNewTilemap(xSize, ySize);
            _GenerateHeightTerrainTilemapData(_tilemapData);
            _PaintTiles(_tileMap, _tilemapData, ruleTile);
        }
        else
        {
            _ClearTiles();
            _tilemapData = _GenerateNewTilemap(xSize, ySize);
            _GenerateHeightTerrainTilemapData(_tilemapData);
            _PaintTiles(_tileMap, _tilemapData, ruleTile);
        }
    }

    void _GenerateGrid()
    {
        _gridObject = new GameObject();
        _gridObject.name = "Grid";
        _grid = _gridObject.AddComponent<Grid>();
    }

    void _GenerateTilemap()
    {
        _tilemapObject = new GameObject();
        _tilemapObject.name = "Tilemap";
        _tileMap = _tilemapObject.AddComponent<Tilemap>();
        _tilemapObject.AddComponent<TilemapRenderer>();
        _tilemapObject.AddComponent<TilemapCollider2D>();
        _tilemapObject.transform.SetParent(_gridObject.transform);
        
    }

    TilemapData _GenerateNewTilemap(int x, int y)
    {
        TilemapData tilemapData = new TilemapData(x, y);
        return tilemapData;
    }

    void _GenerateFlatTerrainTilemapData(TilemapData tilemapData, bool isHole = false)
    {
        int holeX = -1;
        if (isHole)
        {
            holeX = Random.Range(0, tilemapData.XSize);
        }

        for (int i=0; i<tilemapData.YSize; ++i)
        {
            for (int j=0; j<tilemapData.XSize; ++j)
            {
                if (i <= GROUND_Y && j != holeX)
                {
                    tilemapData.SetData(j, i, 1);
                }
            }
        }
    }

    void _GenerateHeightTerrainTilemapData(TilemapData tilemapData)
    {
        int height = GROUND_Y;
        for (int i=0; i<tilemapData.XSize; ++i)
        {
            int addValue = Random.Range(-1, 2);
            height = Mathf.Clamp(height + addValue, 0, tilemapData.YSize);

            for (int j=height; j>=0; --j)
            {
                tilemapData.SetData(i, j, 1);
            }
        }
    }

    void _PaintTiles(Tilemap tilemap, TilemapData tilemapData, TileBase tile)
    {
        for (int i = 0; i < tilemapData.YSize; ++i)
        {
            for (int j = 0; j < tilemapData.XSize; ++j)
            {
                if (tilemapData.GetData(j, i) == 1)
                    tilemap.SetTile(new Vector3Int(j, i), tile);
            }
        }
    }

    void _ClearTiles()
    {
        _tileMap.ClearAllTiles();
    }
}
