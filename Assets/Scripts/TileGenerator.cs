using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap redTileMap;
    public TileBase redTile;
    private Vector3Int targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                Debug.Log($"Tile found at grid position: {pos}");
            }
        }

        //Draw tile logic - I DID IT!
        targetPos = new Vector3Int(7, -5, 0);
        redTileMap.SetTile(targetPos, redTile);
    }

    void GenerateTileSet()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
