using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace twoDProject.Dungeon
{
    /// <summary>
    /// total dungeon tile map data of multiple tile map datas
    /// </summary>
    public class DungeonTilemapData
    {
        private List<TilemapData> _tilemapDataList;

        public DungeonTilemapData()
        {
            _tilemapDataList = new List<TilemapData>();
        }

        public void Add(TilemapData tilemapData)
        {
            _tilemapDataList.Add(tilemapData);
        }

        public void Clear()
        {
            _tilemapDataList.Clear();
        }

        public IReadOnlyList<TilemapData> GetList()
        {
            return _tilemapDataList.AsReadOnly();
        }

        public void GenerateDungeonTilemapData()
        {
            for (int i = 0; i < _tilemapDataList.Count; ++i)
            {
                _tilemapDataList[i].GenerateTileMap();

                //make it Y position connected
                if (i < _tilemapDataList.Count - 1)
                {
                    _tilemapDataList[i + 1].StartY = _tilemapDataList[i].LastY;
                }
            }
        }
    }

    /// <summary>
    /// generate dungeon with tile map, start position, exit portal stone, monsters
    /// </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private int dungeonLength;
        [SerializeField] private int enemyAmount;

        [Header("Entrance Map Tile")]
        [SerializeField] private int entranceTileXSize;
        [SerializeField] private int entranceTileYSize;
        [SerializeField] private int entranceStartY;
        [SerializeField] private int entranceWidthX;

        [Header("Height Terrain Map Tile")]
        [SerializeField] private int heightTerrainTileXSize;
        [SerializeField] private int heightTerrainTileYSize;
        [SerializeField] private int holePerTilemap;
        [SerializeField] private float intensity;

        [Header("Exit Map Tile")]
        [SerializeField] private int exitTileXSize;
        [SerializeField] private int exitTileYSize;
        [SerializeField] private int exitWidthX;

        [Header("Others")]
        [SerializeField] PhysicsMaterial2D physicsMaterial;
        [SerializeField] TileBase ruleTile;
        [SerializeField] GameObject portalStonePrefab;
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] Vector3 _playerStartPosition;
        [SerializeField] Vector3 _exitPortalPosition;
        
        public Vector3 PlayerStartPosition => _playerStartPosition;

        private GameObject _portalStone;
        private List<GameObject> _enemyList;
        private int _exitY;
        private GameObject _gridObject;
        private Grid _grid;
        private GameObject _tilemapObject;
        private Tilemap _tileMap;
        private TilemapCollider2D _tilemapCollider2d;
        private CompositeCollider2D _compositeCollider2d;
        private Rigidbody2D _rigidbody2d;
        private DungeonTilemapData _dungeonTilemapData;

        private void Awake()
        {
            Initialize();
        }

        public void GenerateDungeon(Action callback = null)
        {
            Initialize();

            //clear datas
            _tileMap.ClearAllTiles();
            _dungeonTilemapData.Clear();

            if (_portalStone != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(_portalStone);
#else
                Destroy(_portalStone);
#endif
            }

            if (_enemyList.Count > 0)
            {
                foreach (GameObject enemy in _enemyList)
                {
#if UNITY_EDITOR
                    DestroyImmediate(enemy);
#else
                    Destroy(enemy);
#endif
                }
                _enemyList.Clear();
            }

            //entrance tile map
            EntranceTileMapData entranceTilemap = new EntranceTileMapData(entranceTileXSize, entranceTileYSize, entranceStartY, entranceWidthX);
            _dungeonTilemapData.Add(entranceTilemap);

            //height terrain tile map
            for (int i = 0; i < dungeonLength; ++i)
            {
                HeightTerrainTileMapData heightTerrainTilemap = new HeightTerrainTileMapData(heightTerrainTileXSize, heightTerrainTileYSize,
                    holePerTilemap, intensity);
                _dungeonTilemapData.Add(heightTerrainTilemap);
            }

            //exit tile map
            ExitTileMapData exitTilemap = new ExitTileMapData(exitTileXSize, exitTileYSize, exitWidthX);
            _dungeonTilemapData.Add(exitTilemap);

            _dungeonTilemapData.GenerateDungeonTilemapData();

            for (int i = 0; i < _dungeonTilemapData.GetList().Count; ++i)
            {
                _PaintTiles(_tileMap, _dungeonTilemapData.GetList()[i], i, ruleTile);
            }

            _tilemapCollider2d = _tileMap.GetComponent<TilemapCollider2D>();
            if (null == _compositeCollider2d)
                _compositeCollider2d = _tilemapObject.AddComponent<CompositeCollider2D>();
            _rigidbody2d = _tileMap.GetComponent<Rigidbody2D>();

            _tilemapCollider2d.compositeOperation = Collider2D.CompositeOperation.Merge;
            _rigidbody2d.bodyType = RigidbodyType2D.Static;

            _compositeCollider2d.sharedMaterial = physicsMaterial;
            _rigidbody2d.sharedMaterial = physicsMaterial;

            //set player start position, portal position
            _playerStartPosition = new Vector3(3f, entranceTilemap.StartY + 1);
            _exitPortalPosition = new Vector3(entranceTileXSize + exitTileXSize + (heightTerrainTileXSize * dungeonLength) - 3f, exitTilemap.LastY + 1);

            _portalStone = Instantiate(portalStonePrefab, _exitPortalPosition, Quaternion.identity);

            //spawn enemies
            for (int i = 1; i < _dungeonTilemapData.GetList().Count - 1; ++i)
            {
                HashSet<int> randNum = new HashSet<int>();
                for (int j = 0; j < enemyAmount; ++j)
                {
                    int randX = UnityEngine.Random.Range(0, _dungeonTilemapData.GetList()[i].XSize);
                    randNum.Add(randX);
                }

                foreach (int randX in randNum)
                {
                    int groundY = _dungeonTilemapData.GetList()[i].YSize;

                    //if it's not a hole
                    if (groundY > 0)
                    {
                        GameObject enemy = Instantiate(enemyPrefab, new Vector3(randX + (_dungeonTilemapData.GetList()[i].XSize * dungeonLength * i), groundY), Quaternion.identity);
                        _enemyList.Add(enemy);
                    }
                }
            }

            callback?.Invoke();
        }

        private void Initialize()
        {
            _dungeonTilemapData = new DungeonTilemapData();
            _enemyList = new List<GameObject>();
            _GenerateGrid();
            _GenerateTilemap();
        }

        void _GenerateGrid()
        {
            if (null == _gridObject)
            {
                _gridObject = new GameObject();
                _gridObject.name = "Grid";
                _grid = _gridObject.AddComponent<Grid>();
            }
        }

        void _GenerateTilemap()
        {
            if (null == _tilemapObject)
            {
                _tilemapObject = new GameObject();
                _tilemapObject.name = "Tilemap";
                _tileMap = _tilemapObject.AddComponent<Tilemap>();
                _tilemapObject.AddComponent<TilemapRenderer>();
                _tilemapObject.AddComponent<TilemapCollider2D>();
                _tilemapObject.transform.SetParent(_gridObject.transform);
                _tilemapObject.layer = LayerMask.NameToLayer("Ground");
            }
        }

        void _PaintTiles(Tilemap tilemap, TilemapData tilemapData, int offset, TileBase tile)
        {
            for (int i = 0; i < tilemapData.XSize; ++i)
            {
                for (int j = 0; j < tilemapData.YSize; ++j)
                {
                    if (tilemapData.GetData(j, i) == 1)
                        tilemap.SetTile(new Vector3Int(i + (tilemapData.XSize * offset), j), tile);
                }
            }
        }
    }
}