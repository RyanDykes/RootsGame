using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    private const int _maxHealth = 10;

    private int _exp;
    private int _health;

    public void RecieveExp(int exp)
    {
        Debug.Log("Recieving exp: " + exp + ", current _exp: " + _exp);
        _exp += exp;
        Debug.Log("New _exp: " + _exp);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Dealing damage: " + damage + ", _health: " + _health);
        _health -= damage;
        Debug.Log("New health: " + _health);

        if (_health < 1)
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
        _health = _maxHealth;
    }
}
