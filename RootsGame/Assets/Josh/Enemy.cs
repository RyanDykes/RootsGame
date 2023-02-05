using UnityEngine;

public class Enemy : MonoBehaviour
{

    public const string treeName = "Tree";
    private const int _stopDistance = 5;
    private const float _attackCoolDown = 3.0f;

    private GameObject targetObject;
    
    private float _lastAttackAt = 0.0f;
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
        AssignTarget(GameObject.Find(treeName));
    }

    void Update()
    {
        if (IsDead || PlayerSingleton.Instance.GamePaused)
        {
            return;
        } 
        else if (targetObject == null)
        {
            AssignTarget(GameObject.Find(treeName));
        }

        var distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);
        var stopDistance = targetObject.name == treeName ? _stopDistance : 1.5;

        if (distanceToTarget > stopDistance)
        {
            MoveTowardsTarget();
        }
        else
        {
            if (targetObject.name == treeName)
            {
                AttackTree();
            } 
            else if (targetObject.CompareTag("Flower"))
            {
                AttackFlower();
            } 
            else if (targetObject.CompareTag("Wall"))
            {
                AttackWall();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Flower"))
        {
            AssignTarget(other.gameObject);
        }
    }

    public void HaveHitWall(GameObject wall)
    {
        AssignTarget(wall);
    }

    public void AssignTarget(GameObject newTarget)
    {
        targetObject = newTarget;
    }

    public void MoveTowardsTarget()
    {
        var movementSpeed = Random.Range(MinMovementSpeed, MaxMovementSpeed);
        var randomizedTarget = targetObject.transform.position;
        if (timer <= 0)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-offsetAmount, offsetAmount), targetObject.transform.position.y, Random.Range(-offsetAmount, offsetAmount));
            randomizedTarget += randomOffset;
            timer = changeInterval;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        transform.LookAt(targetObject.transform.position);
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
        var delta = Time.time - _lastAttackAt;
        if (delta >= _attackCoolDown)
        {
            _lastAttackAt = Time.time;
            DealDamageToFlower();
        }
    }

    private void AttackWall()
    {
        var delta = Time.time - _lastAttackAt;
        if (delta >= _attackCoolDown)
        {
            _lastAttackAt = Time.time;
            DealDamageToWall();
        }
    }

    public void DealDamageToTree()
    {
        if (targetObject.name != treeName) return;

        PlayerSingleton.Instance.TakeDamage(AttackPower);
    }

    public void DealDamageToFlower()
    {
        if (!targetObject.CompareTag("Flower")) return;

        Flower flower = targetObject.GetComponent<Flower>();
        flower.TakeDamage();
    }

    public void DealDamageToWall()
    {
        if (!targetObject.CompareTag("Wall")) return;

        WallRoot wall = targetObject.GetComponent<WallRoot>();
        wall.TakeDamage();
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
