using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeSingleton : MonoBehaviour
{
    public static TreeSingleton Instance { get; private set; }


    private int _health;

    public void TakeDamage(int damage)
    {
        Debug.Log("Dealing damage: " + damage + ", _health: " + _health);
        _health -= damage;
        Debug.Log("New health: " + _health);

        if (_health < 1)
        {
            Debug.Log("Tree is dead!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

            _health = 10;
        }
    }
}
