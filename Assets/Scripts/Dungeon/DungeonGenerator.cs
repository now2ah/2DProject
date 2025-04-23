using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class DungeonGenerator : MonoBehaviour
{
    public int GROUND_Y = 4;
    public int dungeonLength = 1;
    public TileBase ruleTile;

    GameObject _gridObject;
    Grid _grid;
    
    GameObject _tilemapObject;
    Tilemap _tileMap;

    TilemapCollider2D _tilemapCollider2d;
    CompositeCollider2D _compositeCollider2d;
    Rigidbody2D _rigidbody2d;

    MapDataGenerator _mapDataGenerator;
    List<TilemapData> _dungeonTilemapData;

    public void GenerateDungeon()
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

        _dungeonTilemapData.Add(_mapDataGenerator.GenerateTilemap(MapDataGenerator.EMapDataType.ENTRANCE, GROUND_Y));

        for (int i=0; i<dungeonLength; ++i)
        {
            _dungeonTilemapData.Add(_mapDataGenerator.GenerateTilemap(MapDataGenerator.EMapDataType.HEIGHT_TERRAIN, _mapDataGenerator.CurrentHeight));
        }

        _dungeonTilemapData.Add(_mapDataGenerator.GenerateTilemap(MapDataGenerator.EMapDataType.EXIT, _mapDataGenerator.CurrentHeight));

        for (int i=0; i<_dungeonTilemapData.Count; ++i)
        {
            _PaintTiles(_tileMap, _dungeonTilemapData[i], i, ruleTile);
        }

        _tilemapCollider2d = _tileMap.GetComponent<TilemapCollider2D>();
        _compositeCollider2d = _tileMap.AddComponent<CompositeCollider2D>();
        _rigidbody2d = _tileMap.GetComponent<Rigidbody2D>();

        _tilemapCollider2d.compositeOperation = Collider2D.CompositeOperation.Merge;
        _rigidbody2d.bodyType = RigidbodyType2D.Static;
    }

    void _Initialize()
    {
        _mapDataGenerator = GetComponent<MapDataGenerator>();
        if (null == _dungeonTilemapData)
            _dungeonTilemapData = new List<TilemapData>();

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
