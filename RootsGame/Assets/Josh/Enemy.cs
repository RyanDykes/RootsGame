using UnityEngine;

public class Enemy : MonoBehaviour
{

    public const string targetName = "Tree";
    private const int _stopDistance = 3;
    private const float _attackCoolDown = 3.0f;

    private GameObject targetObject;
    private Vector3 target;
        
    private bool isCurrentlyColliding = false;

    private float _lastAttackAt = 0.0f;
    private GameObject _flower;

    public bool IsDead { get; set; } = false;

    public int Health = 1;
    public int AttackPower = 1;
    public float MinMovementSpeed = 0.5f;
    public float MaxMovementSpeed = 1.0f;
    public int ExpReward = 10;

    public float changeInterval = 1.0f;
    public float offsetAmount = 1.0f;
    private float timer = 0;

    public Enemy()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.Find(targetName);
        if (targetObject != null)
        {
            target = targetObject.transform.position;
        }
        else
        {
            Debug.Log("Object not found: " + targetName);
        }
    }

    void Update()
    {
        if (target == null || IsDead || PlayerSingleton.Instance.GamePaused || isCurrentlyColliding)
        {
            return;
        }

        var distanceToTree = Vector3.Distance(transform.position, target);
        if (distanceToTree > _stopDistance)
        {
            MoveTowardsTree();
        }
        else
        {
            AttackTree();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Flower"))
        {
            isCurrentlyColliding = true;
            _flower = other.gameObject;
            Invoke(nameof(AttackFlower), 3.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCurrentlyColliding = false;
    }

    public void MoveTowardsTree()
    {
        var movementSpeed = Random.Range(MinMovementSpeed, MaxMovementSpeed);
        var randomizedTarget = target;
        if (timer <= 0)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-offsetAmount, offsetAmount), target.y, Random.Range(-offsetAmount, offsetAmount));
            randomizedTarget += randomOffset;
            timer = changeInterval;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        transform.position = Vector3.MoveTowards(transform.position, randomizedTarget, movementSpeed * Time.deltaTime);
    }

    private void AttackTree()
    {
        var delta = Time.time - _lastAttackAt;
        if (delta >= _attackCoolDown)
        {
            _lastAttackAt = Time.time;
            DealDamageToTree();
        }
    }

    private void AttackFlower()
    {
        DealDamageToFlower();
    }

    public void DealDamageToFlower()
    {
        Destroy(_flower);
        isCurrentlyColliding = false;
    }

    public void DealDamageToTree()
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
