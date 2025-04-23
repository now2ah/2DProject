using Cainos.LucidEditor;
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

    [Header("Common")]
    public int xSize = 10;
    public int ySize = 10;

    [Header("Entrance")]
    public int flatTileOfEntrance = 4;

    [Header("Height")]
    public int holePerPartTilemap = 2;
    public float intense = 0.2f;

    [Header("Exit")]
    public int flatTileOfExit = 4;

    TilemapData _tilemapData;
    int _currentHeight = -1;

    public int CurrentHeight => _currentHeight;

    public TilemapData GenerateTilemap(EMapDataType type, int startHeight)
    {
        _tilemapData = _GenerateNewTilemap(ySize, xSize);

        if (type == EMapDataType.ENTRANCE)
        {
            _currentHeight = _GenerateEntranceTilemapData(_tilemapData, startHeight, flatTileOfEntrance);
        }
        else if (type == EMapDataType.FLAT_TERRAIN)
        {
            _currentHeight = _GenerateFlatTerrainTilemapData(_tilemapData, startHeight);
        }
        else if (type == EMapDataType.HEIGHT_TERRAIN)
        {
            _currentHeight = _GenerateHeightTerrainTilemapData(_tilemapData, startHeight, intense, holePerPartTilemap);
        }
        else if (type == EMapDataType.EXIT)
        {
            _GenerateExitTilemapData(_tilemapData, startHeight, flatTileOfExit);
        }

        return _tilemapData;
    }

    TilemapData _GenerateNewTilemap(int y, int x)
    {
        TilemapData tilemapData = new TilemapData(y, x);
        return tilemapData;
    }

    int _GenerateEntranceTilemapData(TilemapData tilemapData, int startHeight, int entrance)
    {
        int height = startHeight;

        for (int i = 0; i < tilemapData.XSize; ++i)
        {
            //block end
            if (i == 0)
            {
                height = tilemapData.YSize - 1;
            }
            //make entrance flat
            else if (i <= entrance)
            {
                height = startHeight;
            }
            else if (i > entrance)
            {
                int addValue = Random.Range(-1, 2);
                height = Mathf.Clamp(height + addValue, 0, tilemapData.YSize);
            }

            for (int j = height; j >= 0; --j)
            {
                tilemapData.SetData(i, j, 1);
            }
        }

        return height;
    }

    int _GenerateFlatTerrainTilemapData(TilemapData tilemapData, int startHeight, bool isHole = false)
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
                if (i <= startHeight && j != holeX)
                {
                    tilemapData.SetData(j, i, 1);
                }
            }
        }

        return startHeight;
    }

    int _GenerateHeightTerrainTilemapData(TilemapData tilemapData, int startHeight, float intense, int holePerTilemap)
    {
        int[] holeXs = new int[holePerPartTilemap];
        for (int i=0; i<holePerPartTilemap; ++i)
        {
            holeXs[i] = Random.Range(0, xSize);
        }

        int height = startHeight;
        for (int i = 0; i < tilemapData.XSize; ++i)
        {
            int paint = 1;

            if (Random.value < intense)
            {
                int addValue = Random.Range(-1, 2);
                height = Mathf.Clamp(height + addValue, 0, tilemapData.YSize);
            }
            
            for (int j=0; j<holePerPartTilemap; ++j)
            {
                if (i == holeXs[j])
                {
                    paint = 0;
                    break;
                }  
                else
                {
                    paint = 1;
                } 
            }

            for (int j = height; j >= 0; --j)
            {
                tilemapData.SetData(i, j, paint);
            }
        }

        return height;
    }

    void _GenerateExitTilemapData(TilemapData tilemapData, int startHeight, int exit)
    {
        int height = startHeight;

        for (int i = 0; i < tilemapData.XSize; ++i)
        {
            //block end
            if (i == tilemapData.XSize - 1)
            {
                height = tilemapData.YSize - 1;
            }
            //make exit flat
            else if (i >= exit)
            {
                height = startHeight;
            }
            else if (i < exit)
            {
                int addValue = Random.Range(-1, 2);
                height = Mathf.Clamp(height + addValue, 0, tilemapData.YSize);
            }

            for (int j = height; j >= 0; --j)
            {
                tilemapData.SetData(i, j, 1);
            }
        }
    }
}
