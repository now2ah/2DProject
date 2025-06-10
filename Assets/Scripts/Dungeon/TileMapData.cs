using System.Linq;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace twoDProject.Dungeon
{
    public enum ETileType
    {
        Empty,
        Tile,
    }

    /// <summary>
    /// two dimensional integer array tilemap data class
    /// </summary>
    /// 
    public abstract class TilemapData
    {
        protected int _xSize;
        protected int _ySize;

        protected int[,] _tileData;

        protected int _startY;
        protected int _lastY;

        public int XSize => _xSize;
        public int YSize => _ySize;
        public int[,] TileData => _tileData;

        public int StartY { get { return _startY; } set { _startY = value; } }
        public int LastY => _lastY;

        public abstract void GenerateTileMap();

        public TilemapData(int xSize, int ySize)
        {
            _xSize = xSize;
            _ySize = ySize;

            _tileData = new int[ySize, xSize];
            for (int i = 0; i < ySize; ++i)
            {
                for (int j = 0; j < xSize; ++j)
                {
                    _tileData[i, j] = 0;
                }
            }
        }

        public int GetData(int y, int x)
        {
            if (null == _tileData)
            {
                throw new System.Exception("Invalid Tiledata");
            }

            if (IsValidCoordinate(y, x, _ySize, _xSize))
                return _tileData[y, x];
            else
                return -1;
        }

        public void SetData(int y, int x, ETileType type)
        {
            if (null == _tileData)
            {
                throw new System.Exception("Invalid Tiledata");
            }

            if (IsValidCoordinate(y, x, _ySize, _xSize))
            {
                _tileData[y, x] = (int)type;
            }
        }

        public int GetGroundY(int x)
        {
            if (null == _tileData)
            {
                throw new System.Exception("Invalid Tiledata");
            }

            if (x < 0 || x >= XSize)
            {
                throw new System.Exception("Invalid Index");
            }

            for (int y = 0; y < YSize; ++y)
            {
                if (_tileData[y, x] == 0)
                {
                    return y;
                }
            }

            return -1;
        }

        protected static bool IsWithinBounds(int value, int size)
        {
            if (value >= size || value < 0)
                return false;

            return true;
        }

        protected static bool IsValidCoordinate(int y, int x, int ySize, int xSize)
        {
            if (x >= xSize || x < 0)
                return false;

            if (y >= ySize || y < 0)
                return false;

            return true;
        }
    }

    /// <summary>
    /// tile map data of dungeon entrance
    /// </summary>
    public class EntranceTileMapData : TilemapData
    {
        private readonly int _entranceWidth;

        public EntranceTileMapData(int xSize, int ySize, int startHeight, int entranceWidth) : base(xSize, ySize)
        {
            if (IsWithinBounds(entranceWidth, xSize) == false)
            {
                throw new System.Exception("Invalid parameters");
            }

            _startY = startHeight;
            _entranceWidth = entranceWidth;
        }

        public override void GenerateTileMap()
        {
            int height = _startY;

            for (int i = 0; i < _xSize; ++i)
            {
                //block end
                if (i == 0)
                {
                    height = _ySize - 1;
                }
                //make entrance flat
                else if (i <= _entranceWidth)
                {
                    height = _startY;
                }
                else if (i > _entranceWidth)
                {
                    int addValue = Random.Range(-1, 2);
                    height = Mathf.Clamp(height + addValue, 0, _ySize);
                }

                for (int j = height; j >= 0; --j)
                {
                    SetData(j, i, ETileType.Tile);
                }

                _lastY = height;
            }
        }
    }

    /// <summary>
    /// automatically generated dungeon tile map data
    /// </summary>
    public class HeightTerrainTileMapData : TilemapData
    {
        private readonly int _holePerTilemap;
        private readonly float _intensity;

        public HeightTerrainTileMapData(int xSize, int ySize, int holePerTilemap, float intensity) : base(xSize, ySize)
        {
            _holePerTilemap = holePerTilemap;
            _intensity = intensity;
        }

        public override void GenerateTileMap()
        {
            int[] holeXs = new int[_holePerTilemap];
            for (int i = 0; i < _holePerTilemap; ++i)
            {
                holeXs[i] = Random.Range(0, _xSize);
            }

            int height = _startY;
            for (int i = 0; i < _xSize; ++i)
            {
                if (holeXs.Contains(i))
                {
                    continue;
                }
                else
                {
                    if (Random.value < _intensity)
                    {
                        int addValue = Random.Range(-1, 2);
                        height = Mathf.Clamp(height + addValue, 0, _ySize);
                    }

                    for (int j = height; j >= 0; --j)
                    {
                        SetData(j, i, ETileType.Tile);
                    }

                    _lastY = height;
                }
            }
        }
    }

    /// <summary>
    /// tile map data of dungeon exit
    /// </summary>
    public class ExitTileMapData : TilemapData
    {
        private readonly int _exitWidth;

        public ExitTileMapData(int xSize, int ySize, int exitWidth) : base(xSize, ySize)
        {
            if (IsWithinBounds(_exitWidth, xSize) == false)
            {
                throw new System.Exception("Invalid parameters");
            }

            _exitWidth = exitWidth;
        }

        public override void GenerateTileMap()
        {
            int height = _startY;

            for (int i = 0; i < _xSize; ++i)
            {
                //block end
                if (i == _xSize - 1)
                {
                    height = _ySize - 1;
                }

                //make exit flat
                else if (i >= _exitWidth)
                {
                    height = _startY;
                    _lastY = height;
                }
                else if (i < _exitWidth)
                {
                    int addValue = Random.Range(-1, 2);
                    height = Mathf.Clamp(height + addValue, 0, _ySize);
                }

                for (int j = height; j >= 0; --j)
                {
                    SetData(j, i, ETileType.Tile);
                }
            }
        }
    }
}