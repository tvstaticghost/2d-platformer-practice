using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public TileGenerator tileGenerator;
    public PlayerScript playerScript;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip scoreClip;
    void Start()
    {
        // Automatically find the TileGenerator on the "Grid" GameObject
        GameObject gridObject = GameObject.Find("Grid");

        if (gridObject != null)
        {
            tileGenerator = gridObject.GetComponent<TileGenerator>();
        }

        if (tileGenerator == null)
        {
            Debug.LogWarning("TileGenerator not found on Grid object!");
        }

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<PlayerScript>();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3 && other.gameObject.transform.position.y + other.GetComponent<SpriteRenderer>().bounds.size.y >= gameObject.transform.position.y)
        {
            Debug.Log("Player landed");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            tileGenerator.ClearAllTiles();

            if (scoreClip != null)
            {
                audioSource.PlayOneShot(scoreClip);
            }

            StartCoroutine(TileGenerator.Instance.GenerateTileSetCoroutine());
            playerScript.SetHasReachedPlatform();
            TileGenerator.Instance.ClearOldPlatforms();
            playerScript.IncrementPlayerScore();
        }
    }
}
