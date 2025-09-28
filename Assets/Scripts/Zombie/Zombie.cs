using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    // enum - enumeration - перелічення
    enum ZombieState
    {
        Idle, Agro
    }
    private ZombieState state;

    [SerializeField] private Image _healthBar;
    float maxHealth = 1;
    float currentHealth;

    [SerializeField] private float _wanderRangeMax = 6f;
    [SerializeField] private float _wanderRangeMin = 1.5f;

    [SerializeField] private float _agroDistance = 10f;

    [SerializeField]
    private NavMeshAgent _agent;

    private Vector3 _spawnPoint;

    private ObjectPool<Zombie> _zombiePool;
    private ZombieSpawner _spawner;

    private bool _isAlive = false;

    void Update()
    {
        if (!_isAlive)
            return;

        if (Vector3.Distance(_spawner.CarTransfrom.position, transform.position) >= 35)
        {
            Death();
            return;
        }

        if (state == ZombieState.Idle)
        {
            _agent.speed = 3.5f;
            if (!_agent.pathPending && _agent.remainingDistance <= 0.02f)
            {
                Wander();
            }
            if(Vector3.Distance(_spawner.CarTransfrom.position, transform.position) <= _agroDistance)
            {
                state = ZombieState.Agro;
            }
        }
        else if(state == ZombieState.Agro)
        {
            _agent.speed = 5f;
            _agent.SetDestination(_spawner.CarTransfrom.position);
        }
    }

    // Метод який встановлює початкові налаштування зомбі
    public void Init(ObjectPool<Zombie> zombiePool, ZombieSpawner spawner, Vector3 spawnPoint)
    {
        _zombiePool = zombiePool;
        _spawnPoint = spawnPoint;
        _spawner = spawner;

        state = ZombieState.Idle;

        currentHealth = maxHealth;
        _healthBar.fillAmount = 1;

        transform.position = _spawnPoint;

        gameObject.SetActive(true);

        _agent.enabled = true;

        _agent.Warp(_spawnPoint);

        _isAlive = true;

        Wander();
    }

    private void Wander()
    {
        Vector3 randomPoint;
        if (GetRandomPoint(_spawnPoint, _wanderRangeMax, out randomPoint))
        {
            _agent.SetDestination(randomPoint);
        }
        else
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("Death");
        _isAlive = false;
        _agent.enabled = false;
        _zombiePool.ReturnObject(this);
        //gameObject.SetActive(false);
    }

    bool GetRandomPoint(Vector3 center, float radius, out Vector3 randomPoint)
    {
        Vector3 randomDirection;

        for (int i = 0; i < 10; i++)
        {
            randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, hit.position) > _wanderRangeMin)
                {
                    randomPoint = hit.position;
                    return true;
                }
            }

        }

        randomPoint = Vector3.zero;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Death();
        }

        if(other.CompareTag("Bullet"))
        {
            currentHealth -= 0.25f;
            _healthBar.fillAmount = currentHealth;

            if(currentHealth <= 0)
            {
                Death();
            }
        }
    }
}
