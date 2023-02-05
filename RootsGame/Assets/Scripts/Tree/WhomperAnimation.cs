using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhomperAnimation : MonoBehaviour
{
    [SerializeField] private WhompingRoot whompingRoot = null;

    public void GrabEnemy()
    {
        whompingRoot.GrabEnemy();
    }

    public void ThrowEnemy()
    {
        whompingRoot.ThrowEnemy();
    }
}
