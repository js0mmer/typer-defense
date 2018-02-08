using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float spawnDelay = -1;
    public GameObject enemy;
    public Vector3 spawnLocation;

    static int health = 100;
    float timeBefore = 0;
    bool gameEnded = false;
    [HideInInspector]
    public bool hasActiveEnemy;
    private Enemy activeEnemy;
    private bool isPaused = true;

    public GameObject gameOver;
    public GameObject difficultyMenu;
    public GameObject pauseMenu;
    public GameObject overlayCanvas;
    public Text gameOverScore;
    public Text score;
    public Text highscore;
    public EventSystem es;
    public GameObject restartButton;
    public GameObject quitButton;
    public Image healthBar;
    [HideInInspector]
    public ArrayList enemies;
    private static string[] words = { "sidewalk", "robin", "three", "protect", "periodic",
    "somber", "majestic", "jump", "pretty", "wound", "jazzy",
    "memory", "join", "crack", "grade", "boot", "cloudy", "sick",
    "mug", "hot", "tart", "dangerous", "mother", "rustic", "economical",
    "weird", "cut", "parallel", "wood", "encouraging", "interrupt",
    "guide", "long", "chief", "mom", "signal", "rely", "abortive",
    "hair", "representative", "earth", "grate", "proud", "feel",
    "hilarious", "addition", "silent", "play", "floor", "numerous",
    "friend", "pizzas", "building", "organic", "past", "mute", "unusual",
    "mellow", "analytical", "crate", "homely", "protest", "hungry",
    "society", "head", "female", "eager", "heap", "dramatic", "present",
    "sin", "box", "pies", "awesome", "root", "available", "sleet", "wax",
    "boring", "smash", "anger", "tasty", "spare", "tray", "daffy", "scarce",
    "account", "spot", "thought", "distinct", "nimble", "practice", "cream",
    "ablaze", "thoughtless", "love", "verdict", "giant", "entrepreneur",
    "awesome", "undulating", "arid", "pungent", "supercalifragilisticexpialidocious",
    "excellent", "extraordinary", "ordinary", "immediate", "generator", "input",
    "meticulous", "rigorous", "extravagant", "preposterous", "illustrious",
    "unreasonable", "quadrilateral", "trigonometry", "pythagoras", "mathematician",
    "biotechnology", "philosophy", "psychology", "pharmaceutical", "technology",
    "engineering", "unavailable", "astrophysics", "recommendation", "procedure",
    "graduation", "concurrent", "enrollment", "appreciation", "production",
    "development", "international", "cooperation", "conceptual", "assignment",
    "expectation", "requirement", "education", "performance", "publication",
    "geography", "pavement", "boulevard", "skyscraper", "beginner",
    "intermediate", "advanced", "diversion", "counseling", "exemption",
    "independence", "responding", "presentation" };

    public static GameManager instance;

    void Start()
    {
        instance = this;
        enemies = new ArrayList();
        score.text = "0";
        isPaused = true;
    }

    public void StartGame(float spawnDelay)
    {
        this.spawnDelay = spawnDelay;
        difficultyMenu.SetActive(false);
        overlayCanvas.SetActive(true);
        timeBefore = Time.time;
        Time.timeScale = 1f;
        isPaused = false;
        gameEnded = false;
        StartCoroutine(WaitSpawn());
    }

    IEnumerator WaitSpawn()
    {
        while (!gameEnded)
        {
            Instantiate(enemy, spawnLocation, enemy.transform.rotation);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void Update()
    {
        if (spawnDelay != -1f && !gameEnded)
        {
            score.text = (Time.time - timeBefore).ToString("0");

            foreach (char letter in Input.inputString)
            {
                TypeLetter(letter);
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPaused && pauseMenu.activeInHierarchy)
                {
                    Resume();
                }
                else if(!isPaused)
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    isPaused = true;
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            gameOverScore.text = "Time Lasted: " + score.text + " seconds";

            if (int.Parse(score.text) > PlayerPrefs.GetInt("highscore" + spawnDelay, 0))
            {
                highscore.text = "Record: " + score.text + " seconds";
                PlayerPrefs.SetInt("highscore" + spawnDelay, int.Parse(score.text));
            }
            else
            {
                highscore.text = "Record: " + PlayerPrefs.GetInt("highscore" + spawnDelay);
            }

            gameOver.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RemoveHealth(int amount)
    {
        if(amount >= health)
        {
            health = 0;
            EndGame();
        } else
        {
            health -= amount;
        }

        healthBar.fillAmount = health / 100.0f;
    }

    public void TypeLetter(char letter)
    {
        if (hasActiveEnemy)
        {
            if (activeEnemy.GetNextLetter() == letter)
            {
                activeEnemy.TypeLetter();
            }
        }
        else
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetNextLetter() == letter)
                {
                    activeEnemy = enemy;
                    hasActiveEnemy = true;
                    enemy.TypeLetter();
                    break;
                }
            }
        }

        if (hasActiveEnemy && activeEnemy.WordTyped())
        {
            hasActiveEnemy = false;
            enemies.Remove(activeEnemy);
            Destroy(activeEnemy.gameObject);
        }
    }

    public bool IsActiveEnemy(Enemy enemy)
    {
        return hasActiveEnemy && activeEnemy == enemy;
    }

    public string RandomWord()
    {
        return words[Random.Range(0, words.Length)];
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

}
