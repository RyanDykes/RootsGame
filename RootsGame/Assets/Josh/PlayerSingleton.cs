using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    // UI
    public TextMeshProUGUI HealthUI;
    public TextMeshProUGUI ExperienceUI;
    public TextMeshProUGUI GameOverUI;

    public int StartHealth = 100;
    public int Experience { get; set; }
    public int Health { get; set; }
    public bool GamePaused { get; set; }
    public int SecondsBetweenGame { get; set; } = 10;

    private bool _inGameOver = false;
    private int _countDown = 0;

    public void RecieveExp(int exp)
    {
        if (_inGameOver || GamePaused) return;

        Experience += exp;
        ExperienceUI.text = "Exp: " + Experience;
    }

    public void TakeDamage(int damage)
    {
        if (_inGameOver || GamePaused) return;

        Health -= damage;
        HealthUI.text = "Health: " + Health;

        if (Health < 1)
        {
            GameOver();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            SetUp();
        }
    }

    private void SetUp()
    {
        _inGameOver = false;
        GamePaused = false;
        GameOverUI.enabled = false;
        Health = StartHealth;
        Experience = 0;
        _countDown = 0;
    }

    private void GameOver()
    {
        _inGameOver = true;
        _countDown = SecondsBetweenGame;
        GameOverUI.enabled = true;
        GameOverUI.text = "Game Over\n" + _countDown + "s";
        Invoke(nameof(CountDown), 1);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetUp();
    }

    private void CountDown()
    {
        _countDown--;
        if (_countDown < 1)
        {
            Invoke(nameof(Restart), 0);
        }
        else
        {
            GameOverUI.text = "Game Over\n" + _countDown + "s";
            Invoke(nameof(CountDown), 1);
        }
    }
}
