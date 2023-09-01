using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int level = 1;
    public float levelStartDelay = 0.5f;
    public float turnDelay = .05f;
    public static GameController instance = null;
    public int turnCounter = 0;

    public static int playerScore = 0;
    [HideInInspector] public bool playersTurn = true;
    private bool isPlayerHidden;

    private List<EnemyPathfinding> enemies;
    private BoardGenerator board;
    public Text levelText;

    [SerializeField] private GameObject gameOverPanel;
    public TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextGameOver;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [HideInInspector] public UnityEvent m_TurnEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        board = GetComponent<BoardGenerator>();
        InitGame();
        m_TurnEvent.AddListener(NextTurn);
    }

    public void AddEnemyToList(EnemyPathfinding enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemyToList(EnemyPathfinding enemy)
    {
        enemies.Remove(enemy);
    }

    public void SetPlayerHidden(bool hidden)
    {
        isPlayerHidden = hidden;
    }

    public void NextTurn()
    {
        turnCounter++;
        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        playersTurn = false;

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(0);
        }
        else
        {
            yield return new WaitForSeconds(turnDelay / 4);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (isPlayerHidden)
            {
                enemies[i].ChangeTarget(FindObjectOfType<Exit>().transform);
            }
            else
            {
                enemies[i].ChangeTarget(FindObjectOfType<PlayerMovePoint>().transform);
            }
            yield return new WaitForSeconds(enemies[i].moveTime);
            enemies[i].AttemptMove();
        }

        playersTurn = true;
    }

    public IEnumerator LoadNextLevel(float exitDelay)
    {
        playersTurn = false;
        yield return new WaitForSeconds(exitDelay);

        EnemyPathfinding[] numEnemies = FindObjectsOfType<EnemyPathfinding>();
        if (numEnemies.Length >= 1)
        {
            foreach (var enemy in numEnemies)
            {
                Destroy(enemy);
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        level++;
        levelText.text = "Level: " + level;
        InitGame();
    }

    private void InitGame()
    {
        playersTurn = false;
        UpdateScoreUI();
        board = FindObjectOfType<BoardGenerator>();
        enemies = new List<EnemyPathfinding>();
        board.SetupScene(level);
        levelText.text = "Level: " + level;
        Debug.Log("LOADING LEVEL " + level);
        Invoke("HideLevelImage", levelStartDelay);
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "Score:\n" + playerScore;
        scoreTextGameOver.text = "Score: " + playerScore;

        if (playerScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            PlayerPrefs.Save();
            highScoreText.text = "High Score:\n" + playerScore;
        }
        else
        {
            highScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("HighScore", 0);
        }
    }

    public void UpdateHealthUI(int health, int maxHealth)
    {
        playerHealthText.text = $"HP\n{health}/{maxHealth}";
    }

    public void GameOver()
    {
        UpdateScoreUI();
        playerScore = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartClicked()
    {
        gameOverPanel.SetActive(false);
        level = 1;
        InitGame();
    }

    private void HideLevelImage()
    {
        playersTurn = true;
    }
}