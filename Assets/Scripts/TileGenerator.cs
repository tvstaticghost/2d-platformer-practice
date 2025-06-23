using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap redTileMap;
    public Tilemap blueTileMap;
    public Tilemap greenTileMap;
    public GameObject platform;
    public TileBase redTile;
    public TileBase blueTile;
    public TileBase greenTile;
    public TileBase groundTile;
    private Vector3Int targetPos;
    public GameObject player;
    private UnityEngine.Vector3 playerPos;

    [SerializeField] AudioClip kickClip;
    [SerializeField] AudioSource audioSource;

    public static TileGenerator Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
       Instance = this;
    }
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
        //targetPos = new Vector3Int(7, -5, 0);
        //redTileMap.SetTile(targetPos, redTile);

        StartCoroutine(GenerateTileSetCoroutine());
    }

    public IEnumerator GenerateTileSetCoroutine()
    {
        yield return new WaitForSeconds(1f);

        playerPos = player.transform.position; //Assign the player's transform position as the playerPos

        int startingX = (int) playerPos.x;
        int startingY = (int) playerPos.y;

        Tilemap[] maps = { redTileMap, blueTileMap, greenTileMap };
        System.Random random = new();

        int numberOfTiles = random.Next(10, 30);
        int tilesRendered = 0;
        bool goingRight = true;

        while (tilesRendered < numberOfTiles)
        {
            Tilemap mapSelection = maps[random.Next(0, maps.Length)];
            TileBase tileSelection;

            if (mapSelection == redTileMap)
                tileSelection = redTile;
            else if (mapSelection == blueTileMap)
                tileSelection = blueTile;
            else
                tileSelection = greenTile;

            // Randomly switch direction
            int randomChance = random.Next(0, 11);
            if (randomChance == 2)
            {
                goingRight = !goingRight;
                startingY++;
            }

            if (goingRight)
                startingX += random.Next(1, 3);
            else
                startingX -= random.Next(1, 3);

            Vector3Int tilePosition = new(startingX, random.Next(startingY - 2, startingY + 3), 0);
            mapSelection.SetTile(tilePosition, tileSelection);

            startingX++;
            startingY++;
            tilesRendered++;

            if (kickClip != null)
            {
                audioSource.PlayOneShot(kickClip);
            }

            // ⏳ Wait a bit before placing the next tile
            yield return new WaitForSeconds(0.3f); // <-- adjust delay time here
        }

        Instantiate(platform, new UnityEngine.Vector3(startingX + 3, startingY + 1, 0), UnityEngine.Quaternion.identity);
    }

    //NOW I NEED TO REGENERATE THE TILES AFTER LANDING ON THE PLATFORM AND INCREASE AND KEEP SCORE

    public void ClearAllTiles()
    {
        redTileMap.ClearAllTiles();
        blueTileMap.ClearAllTiles();
        greenTileMap.ClearAllTiles();
    }
    
}
