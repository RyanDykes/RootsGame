using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public const string targetName = "Tree";
    private GameObject targetObject;

    private Transform target;
    private const float speed = 1.0f;

    private bool isCurrentlyColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.Find(targetName);
        if (targetObject != null)
        {
            Debug.Log("Object found: " + targetObject.name);
            target = targetObject.transform;
        }
        else
        {
            Debug.Log("Object not found: " + targetName);
        }
    }

    void Update()
    {
        if (target == null)
        {
            Debug.Log("No target!");
            return;
        }

        Debug.Log("Moving towards: " + targetObject.name);

        if (!isCurrentlyColliding)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var name = collision.gameObject.name;
        Debug.Log("Entered collider with: " + name);
        isCurrentlyColliding = true;

        switch (name)
        {
            case "Tree":
                DealDamage();
                break;
            case "Spikes":
                break;
            case "Wall":
                break;
            default:
                return;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Leaving collider with: " + collision.gameObject.name);
        isCurrentlyColliding = false;
    }

    private void DealDamage()
    {
        TreeSingleton.Instance.TakeDamage(1);
    }

}
