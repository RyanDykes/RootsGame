using UnityEngine;

public class Enemy : MonoBehaviour
{

    public const string targetName = "Tree";
    private const int _stopDistance = 3;
    private const float _attackCoolDown = 3.0f;

    private GameObject targetObject;
    private Transform target;
        
    private bool isCurrentlyColliding = false;

    private float _lastAttackAt = 0.0f;

    public bool IsDead { get; set; } = false;
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public float MinMovementSpeed { get; set; }
    public float MaxMovementSpeed { get; set; }
    public int ExpReward { get; set; }

    public Enemy()
    {
 
    }

    // Start is called before the first frame update
    void Start()
    {
        var type = gameObject.name.ToLower();
        if (type.StartsWith("woodcutter"))
        {
            Health = 1;
            AttackPower = 1;
            MinMovementSpeed = 0.5f;
            MaxMovementSpeed = 1.0f;
            ExpReward = 10;
        }
        else if (type.StartsWith("chainsaw"))
        {
            Health = 2;
            AttackPower = 1;
            MinMovementSpeed = 0.5f;
            MaxMovementSpeed = 1.0f;
            ExpReward = 10;
        }
        else if (type.StartsWith("flamethrower"))
        {
            Health = 3;
            AttackPower = 1;
            MinMovementSpeed = 0.5f;
            MaxMovementSpeed = 1.0f;
            ExpReward = 10;
        }
        else if (type.StartsWith("poison"))
        {
            Health = 4;
            AttackPower = 1;
            MinMovementSpeed = 0.5f;
            MaxMovementSpeed = 1.0f;
            ExpReward = 10;
        }

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
        if (IsDead)
        {
            return;
        }

        if (!isCurrentlyColliding)
        {
            var distanceToTree = Vector3.Distance(transform.position, target.position);
            if (distanceToTree > _stopDistance)
            {
                MoveTowardsTree();
            }
            else
            {
                var delta = Time.time - _lastAttackAt;
                if (delta >= _attackCoolDown)
                {
                    _lastAttackAt = Time.time;
                    DealDamage();
                }
            }
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

    public void MoveTowardsTree()
    {
        Debug.Log("Moving towards: " + targetObject.name);
        var movementSpeed = Random.Range(MinMovementSpeed, MaxMovementSpeed);
        transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
    }

    public void DealDamage()
    {
        PlayerSingleton.Instance.TakeDamage(AttackPower);
    }

    public void GiveExperience()
    {
        PlayerSingleton.Instance.RecieveExp(ExpReward);
    }

    public void Die(float time)
    {
        Destroy(gameObject, time);
    }
}
