using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    public int StartHealth = 100;
    public int Experience { get; set; }
    public int Health { get; set; }

    public void RecieveExp(int exp)
    {
        Debug.Log("Recieving exp: " + exp + ", current Experience: " + Experience);
        Experience += exp;
        Debug.Log("New _exp: " + Experience);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Dealing damage: " + damage + ", _health: " + Health);
        Health -= damage;
        Debug.Log("New health: " + Health);

        if (Health < 1)
        {
            Debug.Log("Tree is dead!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SetUp();
        }
    }

    private void Awake()
    {
        Debug.Log("I am awake!");
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
