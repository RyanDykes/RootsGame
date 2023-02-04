using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : MonoBehaviour
{

    public GameObject enemy;
    public int repeatRate = 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstantiateObject", 0, repeatRate);
    }

    void InstantiateObject()
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }
}
