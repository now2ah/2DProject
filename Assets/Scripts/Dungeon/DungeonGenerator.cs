using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.Mathematics;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] Vector3 _playerStartPosition;
    [SerializeField] Vector3 _exitPortalPosition;

    public PhysicsMaterial2D physicsMaterial;

    public Vector3 PlayerStartPosition => _playerStartPosition;

    public GameObject portalStonePrefab;
    GameObject _portalStone;

    List<GameObject> _enemyList;

    public int GROUND_Y = 10;
    public int dungeonLength = 1;
    public TileBase ruleTile;
    public GameObject enemyPrefab;
    public int enemyNum = 3;

    int _exitY;
    public int EXIT_Y => _exitY;

    GameObject _gridObject;
    Grid _grid;
    
    GameObject _tilemapObject;
    Tilemap _tileMap;

    TilemapCollider2D _tilemapCollider2d;
    CompositeCollider2D _compositeCollider2d;
    Rigidbody2D _rigidbody2d;

    MapDataGenerator _mapDataGenerator;
    List<TilemapData> _dungeonTilemapData;

    public void GenerateDungeon(Action callback = null)
    {
        _Initialize();

        //generate entrance tilemap
        //generate middle tilemap
        //generate exit tilemap
        //merge tilemaps
        //add entrance, exit
        //add monsters
        _tileMap.ClearAllTiles();
        _dungeonTilemapData.Clear();
        if (_portalStone != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(_portalStone);
#endif
            Destroy(_portalStone);
        }
        if (_enemyList.Count > 0)
        {
            foreach(GameObject enemy in _enemyList)
            {
#if UNITY_EDITOR
                DestroyImmediate(enemy);
#endif
                Destroy(enemy);
            }
            _enemyList.Clear();
        }


        _dungeonTilemapData.Add(_mapDataGenerator.GenerateTilemap(MapDataGenerator.EMapDataType.ENTRANCE, GROUND_Y));

        for (int i=0; i<dungeonLength; ++i)
        {
            _dungeonTilemapData.Add(_mapDataGenerator.GenerateTilemap(MapDataGenerator.EMapDataType.HEIGHT_TERRAIN, _mapDataGenerator.CurrentHeight));
        }

        _dungeonTilemapData.Add(_mapDataGenerator.GenerateTilemap(MapDataGenerator.EMapDataType.EXIT, _mapDataGenerator.CurrentHeight));

        _exitY = _mapDataGenerator.CurrentHeight;

        for (int i=0; i<_dungeonTilemapData.Count; ++i)
        {
            _PaintTiles(_tileMap, _dungeonTilemapData[i], i, ruleTile);
        }

        _tilemapCollider2d = _tileMap.GetComponent<TilemapCollider2D>();
        if (null == _compositeCollider2d)
            _compositeCollider2d = _tilemapObject.AddComponent<CompositeCollider2D>();
        _rigidbody2d = _tileMap.GetComponent<Rigidbody2D>();

        _tilemapCollider2d.compositeOperation = Collider2D.CompositeOperation.Merge;
        _rigidbody2d.bodyType = RigidbodyType2D.Static;

        _compositeCollider2d.sharedMaterial = physicsMaterial;
        _rigidbody2d.sharedMaterial = physicsMaterial;

        _playerStartPosition = new Vector3(3f, GROUND_Y + 1);
        _exitPortalPosition = new Vector3((_mapDataGenerator.xSize * 2) + (_mapDataGenerator.xSize * dungeonLength) - 3f, _exitY + 1);

        _portalStone = Instantiate(portalStonePrefab, _exitPortalPosition, Quaternion.identity);

        for (int i=1; i<_dungeonTilemapData.Count - 1; ++i)
        {
            HashSet<int> randNum = new HashSet<int>();
            for (int j=0; j<enemyNum; ++j)
            {
                int randX = UnityEngine.Random.Range(0, _dungeonTilemapData[i].XSize);
                randNum.Add(randX);
            }

            foreach(int randX in randNum)
            {
                //int groundY = _dungeonTilemapData[i].GetGroundY(randX);
                int groundY = _dungeonTilemapData[i].YSize;

                //if it's not a hole
                if (groundY > 0)
                {
                    GameObject enemy = Instantiate(enemyPrefab, new Vector3(randX + (_dungeonTilemapData[i].XSize * dungeonLength * i), groundY), Quaternion.identity);
                    _enemyList.Add(enemy);
                }
            }
        }

        callback?.Invoke();
    }

    void _Initialize()
    {
        _mapDataGenerator = GetComponent<MapDataGenerator>();
        if (null == _dungeonTilemapData)
            _dungeonTilemapData = new List<TilemapData>();

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
        for (int i = 0; i < tilemapData.YSize; ++i)
        {
            for (int j = 0; j < tilemapData.XSize; ++j)
            {
                if (tilemapData.GetData(i, j) == 1)
                    tilemap.SetTile(new Vector3Int(i + (tilemapData.XSize * offset), j), tile);
            }
        }
    }
}
