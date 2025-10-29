using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Menu : MonoBehaviour
{
    public GameObject gemDropInstructionsInfinite;
    public GameObject gemDropInstructionsTimed;
    public GameObject gemDropInstructionsLives;
    public GameObject menu;
    public TMP_Text scoreText;
    public TMP_Text endText;
    public TMP_Text leaderboardText;
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject ball;
    public GameObject bomb;
    public GameObject trophy;
    private bool menuOn;
    private bool instructionsOn;
    private int gamePlay;
    private GameObject instructions;
    private int score;
    private int lives;
    private float timeLeft;
    private List<List<int>> highScores;

    public bool MenuOn
    {
        get { return menuOn; }
        set { menuOn = value; }
    }

    public bool InstructionsOn
    {
        get { return instructionsOn; }
        set { instructionsOn = value; }
    }

    public int GamePlay {
        get {return gamePlay;}
        set {gamePlay = value;}
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int Lives {
        get {return lives;}
        set {lives = value;}
    }

    public float TimeLeft {
        get {return timeLeft;}
        set {timeLeft = value;}
    }

    public List<List<int>> HighScores {
        get {return highScores;}
        set {highScores = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuOn = false;
        InstructionsOn = false;
        Score = 0;
        Lives = 3;
        TimeLeft = 60.0f;
        startPanel.SetActive(true);
        GamePlay = 0;
        endPanel.SetActive(false);
        HighScores = new List<List<int>>();

        for (int i = 0; i < 3; i++) {
            List<int> scores = new List<int>();

            for (int j = 0; j < 3; j++)
            {
                string key = i + " HighScore " + j;
                if (PlayerPrefs.HasKey(key))
                {
                    int score = PlayerPrefs.GetInt(key);
                    if (!scores.Contains(score)) // Prevent duplicates
                    {
                        scores.Add(score);
                    }
                }
            }

            HighScores.Add(scores);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GamePlay > 0)
        {
            MenuOn = !MenuOn;
        }

        menu.SetActive(MenuOn);
        Time.timeScale = !MenuOn ? 1 : 0;

        if (!MenuOn && InstructionsOn)
        {
            Destroy(instructions);
            InstructionsOn = false;
        }

        scoreText.text = "Score: " + Score;

        if (GamePlay == 1) {
            endText.text = "Time Left: " + TimeLeft;
        }
        if (GamePlay == 3) {
            endText.text = "Lives: " + Lives;
        }

        if (Lives == 0 || TimeLeft == 0) {
            End();
        }
    }

    public void Quit()
    {
        AddHighScore();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //editor runs
#else
        Application.Quit(); //build
#endif
    }

    public void Restart()
    {
        SceneManager.LoadScene("GemDrop");
    }

    public void ShowInstructions()
    {
        InstructionsOn = !InstructionsOn;

        if (InstructionsOn)
        {
            if (GamePlay == 1) {
                instructions = Instantiate(gemDropInstructionsTimed);
            }
            else if (GamePlay == 2) {
                instructions = Instantiate(gemDropInstructionsInfinite);
            }
            else if (GamePlay == 3) {
                instructions = Instantiate(gemDropInstructionsLives);
            }
            
        }
        else
        {
            Destroy(instructions);
        }
    }

    public void ChooseTimed() {
        GamePlay = 1;
        startPanel.SetActive(false);
        StartSpawning();
    }

    public void ChooseInfinite() {
        GamePlay = 2;
        startPanel.SetActive(false);
        StartSpawning();
    }

    public void ChooseLives() {
        GamePlay = 3;
        startPanel.SetActive(false);
        StartSpawning();
    }

    public void StartSpawning() {
        CancelInvoke("SpawnBall");
        CancelInvoke("SpawnBomb");
        CancelInvoke("SpawnTrophy");
        
        if (GamePlay > 0) {
            InvokeRepeating("SpawnBall", 1, 5);
            InvokeRepeating("SpawnBomb", 3, 7);
            InvokeRepeating("SpawnTrophy", 5, 10);
            InvokeRepeating("UpdateTimeLeft", 0, 1);
        }
    }

    public void End() {
        Time.timeScale = 0;

        AddHighScore();

        string leaderboard = "LEADERBOARD";
        for (int i = 0; i < HighScores[GamePlay - 1].Count; i++)
        {
            leaderboard += "\n " + (i + 1) + ". " + HighScores[GamePlay - 1][i];
        }

        leaderboardText.text = leaderboard;

        endPanel.SetActive(true);
    }

    public void SpawnBall()
    {
        float z = Random.Range(2, 10);
        float x = Random.Range(-z, z);
        GameObject clone = Instantiate(ball, new Vector3(x, 40, z), new Quaternion());
        Destroy(clone, 5.0f);
    }

    public void SpawnBomb()
    {
        float z = Random.Range(2, 10);
        float x = Random.Range(-z, z);
        GameObject clone = Instantiate(bomb, new Vector3(x, 40, z), new Quaternion());
        Destroy(clone, 5.0f);
    }

    public void SpawnTrophy()
    {
        float z = Random.Range(2, 10);
        float x = Random.Range(-z, z);
        GameObject clone = Instantiate(trophy, new Vector3(x, 40, z), new Quaternion());
        Destroy(clone, 5.0f);
    }

    public void AddScore(int curr)
    {
        Score += curr;
    }

    public void LostLife() {
        Lives--;
    }

    public void UpdateTimeLeft() {
        TimeLeft--;
    }

    public int GetGamePlay() {
        return GamePlay;
    }

    public void AddHighScore() {
        if (!HighScores[GamePlay - 1].Contains(Score))
        {
            HighScores[GamePlay - 1].Add(Score);
        }

        HighScores[GamePlay - 1].Sort((x, y) => y.CompareTo(x));

        while (HighScores[GamePlay - 1].Count > 3)
        {
            HighScores[GamePlay - 1].RemoveAt(HighScores[GamePlay - 1].Count - 1);
        }

        for (int i = 0; i < HighScores[GamePlay - 1].Count; i++)
        {
            PlayerPrefs.SetInt((GamePlay - 1) + " HighScore " + i, HighScores[GamePlay - 1][i]);
        }
        for (int i = HighScores[GamePlay - 1].Count; i < 3; i++)
        {
            PlayerPrefs.DeleteKey((GamePlay - 1) + " HighScore " + i);
        }
        PlayerPrefs.Save();
    }
}
