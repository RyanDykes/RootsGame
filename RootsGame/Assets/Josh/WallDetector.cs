using UnityEngine;

public class WallDetector : MonoBehaviour
{

    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            enemy.HaveHitWall(other.gameObject);
        }
    }

}
