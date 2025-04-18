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

    public int GetData(int y, int x)
    {
        if (_IsValid(y, x))
            return _tileData[y, x];
        else
            return -1;
    }

    public void SetData(int y, int x, int data)
    {
        if (null == _tileData)
            return;

        if (_IsValid(y, x))
        {
            _tileData[y, x] = data;
        }
    }

    bool _IsValid(int y, int x)
    {
        if (x >= _xSize || x < 0)
            return false;

        if (y >= _ySize || y < 0)
            return false;

        return true;
    }
}

public class MapDataGenerator : MonoBehaviour
{
    public enum EMapDataType
    {
        ENTRANCE,
        FLAT_TERRAIN,
        HEIGHT_TERRAIN,
        EXIT
    }

    public int GROUND_Y = 4;
    public int holePerPartTilemap = 1;
    public int xSize = 10;
    public int ySize = 10;

    TilemapData _tilemapData;

    public TilemapData GenerateTilemap(EMapDataType type)
    {
        _tilemapData = _GenerateNewTilemap(ySize, xSize);

        if (type == EMapDataType.ENTRANCE)
        {
            _GenerateEntranceTilemapData(_tilemapData);
        }
        else if (type == EMapDataType.FLAT_TERRAIN)
        {
            _GenerateFlatTerrainTilemapData(_tilemapData);
        }
        else if (type == EMapDataType.HEIGHT_TERRAIN)
        {
            _GenerateHeightTerrainTilemapData(_tilemapData);
        }
        else if (type == EMapDataType.EXIT)
        {
            _GenerateExitTilemapData(_tilemapData);
        }

        return _tilemapData;
    }

    TilemapData _GenerateNewTilemap(int y, int x)
    {
        TilemapData tilemapData = new TilemapData(y, x);
        return tilemapData;
    }

    void _GenerateEntranceTilemapData(TilemapData tilemapData)
    {
        for (int i = 0; i < tilemapData.YSize; ++i)
        {
            for (int j = 0; j < tilemapData.XSize; ++j)
            {
                if (j == 0 || i <= GROUND_Y)
                {
                    tilemapData.SetData(j, i, 1);
                }
            }
        }
    }

    void _GenerateFlatTerrainTilemapData(TilemapData tilemapData, bool isHole = false)
    {
        int holeX = -1;
        if (isHole)
        {
            holeX = Random.Range(0, tilemapData.XSize);
        }

        for (int i = 0; i < tilemapData.YSize; ++i)
        {
            for (int j = 0; j < tilemapData.XSize; ++j)
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
        for (int i = 0; i < tilemapData.XSize; ++i)
        {
            int addValue = Random.Range(-1, 2);
            height = Mathf.Clamp(height + addValue, 0, tilemapData.YSize);

            for (int j = height; j >= 0; --j)
            {
                tilemapData.SetData(i, j, 1);
            }
        }
    }

    void _GenerateExitTilemapData(TilemapData tilemapData)
    {
        for (int i = 0; i < tilemapData.YSize; ++i)
        {
            for (int j = 0; j < tilemapData.XSize; ++j)
            {
                if (j == tilemapData.XSize - 1 || i <= GROUND_Y)
                {
                    tilemapData.SetData(j, i, 1);
                }
            }
        }
    }
}
