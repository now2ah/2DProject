using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileBase;

    private void Start()
    {
        tilemap.SetTile(new Vector3Int(1, 1, 0), tileBase);
    }
}
