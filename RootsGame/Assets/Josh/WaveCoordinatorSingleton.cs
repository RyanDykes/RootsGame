using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveCoordinatorSingleton : MonoBehaviour
{

    public static WaveCoordinatorSingleton Instance { get; private set; }

    // UI
    public TextMeshProUGUI WaveCounter;
    public TextMeshProUGUI NextWaveIn;

    // Enemies
    public GameObject Woodcutter;
    public GameObject Chainsaw;
    public GameObject Flamethrower;
    public GameObject Poison;

    public int EnemiesPerWave { get; set; } = 10;
    public int SecondsBetweenWave { get; set; } = 5;

    private int _countDown = 0;
    private int _wave = 0;
    private bool _inWave = false;

    private readonly object _activeLock = new object();
    private int _totalSpawners = 0;
    private int _activeSpawners = 0;

    private readonly object _spawnLock = new object();
    private int _enemiesToSpawn = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            StartWave();
        }
    }

    public void Update()
    {
        if (!_inWave) return;
        if (_activeSpawners > 0) return;

        var _activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (_activeEnemies < 1)
        {
            FinishWave();
        }
    }

    public void FinishSpawner()
    {
        lock(_activeLock)
        {
            _activeSpawners--;
        }
    }

    public GameObject GetEnemy()
    {
        if (_enemiesToSpawn < 1) return null;

        lock (_spawnLock)
        {
            _enemiesToSpawn--;
        }

        var availableEnemies = new List<GameObject> { Woodcutter };
        if (_wave > 1)
        {
            availableEnemies.Add(Chainsaw);
        }       
        if (_wave > 2)
        {
            availableEnemies.Add(Flamethrower);
        }
        if (_wave > 3)
        {
            availableEnemies.Add(Poison);
        }

        return availableEnemies[Random.Range(0, (availableEnemies.Count))];
    }

    private void StartWave()
    {
        Debug.Log("Starting Wave: " + _wave);
        _wave++;
        _inWave = true;
        WaveCounter.text = "Wave " + _wave;
        NextWaveIn.enabled = false;

        _totalSpawners = GameObject.FindGameObjectsWithTag("Respawn").Length;
        _activeSpawners = _totalSpawners;
        _enemiesToSpawn += EnemiesPerWave * _wave;

        var spawners = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (var spawner in spawners)
        {
            Hut hut = (Hut)spawner.GetComponent(typeof(Hut));
            hut.StartSpawning();
        }

        Debug.Log("_totalSpawners: " + _totalSpawners + ", _activeSpawners: " + _activeSpawners + ", enemiesToSpawn: " + _enemiesToSpawn);
    }

    private void CountDown()
    {
        if (PlayerSingleton.Instance.GamePaused)
        {
            if (NextWaveIn.enabled)
            {
                NextWaveIn.enabled = false;
            }
            Invoke(nameof(CountDown), 1);
            return;
        } 
        else if (!NextWaveIn.enabled)
        {
            NextWaveIn.enabled = true;
        }

        _countDown--;
        if (_countDown < 1)
        {
            Invoke(nameof(StartWave), 0);
        }
        else
        {
            NextWaveIn.text = "Next wave in\n" + _countDown + "s";
            Invoke(nameof(CountDown), 1);
        }
    }

    private void FinishWave()
    {
        Debug.Log("Wave Finished: " + _wave);

        _inWave = false;
        _countDown = SecondsBetweenWave;
        NextWaveIn.enabled = true;
        NextWaveIn.text = "Next wave in\n" + SecondsBetweenWave + "s";
        Invoke(nameof(CountDown), 1);

        Debug.Log("New wave in: " + SecondsBetweenWave);
    }

}
