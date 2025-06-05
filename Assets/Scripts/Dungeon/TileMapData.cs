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

        protected int lastY;

        public int XSize => _xSize;
        public int YSize => _ySize;
        public int[,] TileData => _tileData;

        public int LastY => lastY;

        public abstract void GenerateTileMap();

        public TilemapData(int xSize, int ySize)
        {
            _xSize = xSize;
            _ySize = ySize;

            _tileData = new int[ySize, xSize];
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

        protected static bool IsValidCoordinate(int value, int size)
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

    public class EntranceTileMapData : TilemapData
    {
        private readonly int _startHeight;
        private readonly int _entranceWidth;

        public EntranceTileMapData(int xSize, int ySize, int startHeight, int entranceWidth) : base(xSize, ySize)
        {
            if (IsValidCoordinate(startHeight, ySize) == false ||
                IsValidCoordinate(entranceWidth, xSize) == false)
            {
                throw new System.Exception("Invalid parameters");
            }

            _startHeight = startHeight;
            _entranceWidth = entranceWidth;
        }

        public override void GenerateTileMap()
        {
            int height = _startHeight;

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
                    height = _startHeight;
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
            }
        }
    }
}