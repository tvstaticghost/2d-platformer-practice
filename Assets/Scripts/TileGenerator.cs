using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap redTileMap;
    public Tilemap blueTileMap;
    public Tilemap greenTileMap;
    public TileBase redTile;
    public TileBase blueTile;
    public TileBase greenTile;
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

        GenerateTileSet();
    }

    void GenerateTileSet()
    {
        Tilemap[] maps = { redTileMap, blueTileMap, greenTileMap }; //All 3 tile maps in the game
        System.Random random = new();

        //Need to increase y to go up
        int startingX = 7;
        int startingY = -5; //coordinates of the first tile after the black ground platform

        int numberOfTiles = random.Next(10, 30);
        bool goingRight = true;

        for (int i = 0; i < numberOfTiles; i++)
        {
            //randomly select a tile map and set the tile color to the cooresponding map
            Tilemap mapSelection = maps[random.Next(0, maps.Length)];
            TileBase tileSelection;

            if (mapSelection == redTileMap)
            {
                tileSelection = redTile;
            }
            else if (mapSelection == blueTileMap)
            {
                tileSelection = blueTile;
            }
            else
            {
                tileSelection = greenTile;
            }

            //fix logic to space out map in a branching pattern
            int randomChance = random.Next(0, 11);
            if (randomChance == 2)
            {
                //switch directions
                goingRight = !goingRight;
                startingY++;
            }

            if (goingRight)
            {
                startingX += random.Next(1, 3);
            }
            else
            {
                startingX -= random.Next(1, 3);
            }

            mapSelection.SetTile(new Vector3Int(startingX, random.Next(startingY - 2, startingY + 3), 0), tileSelection);
            startingX++;
            startingY++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
