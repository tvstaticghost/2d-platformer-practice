using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public float cubeSpawnTimer = 1f;
    public GameObject redSquare;
    public GameObject blueSquare;
    public GameObject greenSquare;
    public GameObject orangeSquare;
    public GameObject pinkSquare;
    public GameObject purpleSquare;

    private Vector3 newVec;
    private GameObject spawnedSquare;
    private List<GameObject> spawnedSquares = new();
    private List<GameObject> squareOptions = new();
    private System.Random random;
    public UIDocument mainMenu;
    private VisualElement mainMenuRoot;
    private Button playButton;
    private Button quitButton;

    void Start()
    {
        newVec = new Vector3(1.24f, -5.6f, 0);
        random = new();

        squareOptions.Add(redSquare);
        squareOptions.Add(blueSquare);
        squareOptions.Add(greenSquare);
        squareOptions.Add(orangeSquare);
        squareOptions.Add(pinkSquare);
        squareOptions.Add(purpleSquare);

        mainMenuRoot = mainMenu.rootVisualElement;
        Label highScoreLabel = mainMenuRoot.Q<Label>("HighScore");
        highScoreLabel.text = $"High Score: {0}";

        playButton = mainMenuRoot.Q<Button>("PlayButton");
        playButton.clickable.clicked += StartGame;

        quitButton = mainMenuRoot.Q<Button>("QuitButton");
        quitButton.clickable.clicked += QuitGame;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("WorldScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //Screen spans from -9.7 to 9.65
    private void SpawnCube()
    {
        float max = 9.65f;
        float min = -9.7f;

        float randomX = (float)(random.NextDouble() * (max - min) + min);
        newVec = new Vector3(randomX, -5.6f, 0);

        GameObject squareChoice = squareOptions[random.Next(squareOptions.Count)];

        spawnedSquare = Instantiate(squareChoice, newVec, Quaternion.identity);
        spawnedSquares.Add(spawnedSquare);
    }

    void Update()
    {
        cubeSpawnTimer -= Time.deltaTime;
        if (cubeSpawnTimer <= 0)
        {
            SpawnCube();
            cubeSpawnTimer = 1f;
        }

        for (int i = 0; i < spawnedSquares.Count; i++)
        {
            Vector3 pos = spawnedSquares[i].transform.position;
            pos.y += 2f * Time.deltaTime;
            spawnedSquares[i].transform.position = pos;

            if (pos.y > 5.75f)
            {
                Destroy(spawnedSquares[i]);
                spawnedSquares.Remove(spawnedSquares[i]);
            }
        }
    }
}

