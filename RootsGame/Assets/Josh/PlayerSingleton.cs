using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    // UI
    public TextMeshProUGUI HealthUI;
    public TextMeshProUGUI ExperienceUI;

    public int StartHealth = 100;
    public int Experience { get; set; }
    public int Health { get; set; }
    public bool GamePaused { get; set; }

    public void RecieveExp(int exp)
    {
        Experience += exp;
        ExperienceUI.text = "Exp: " + Experience;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        HealthUI.text = "Health: " + Health;

        if (Health < 1)
        {
            Debug.Log("Tree is dead!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SetUp();
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
        Health = StartHealth;
        Experience = 0;
    }
}
