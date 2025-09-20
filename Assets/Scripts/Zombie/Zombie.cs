using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float _wanderRangeMax = 6f;
    [SerializeField] private float _wanderRangeMin = 1.5f;

    private NavMeshAgent _agent;

    private Vector3 _spawnPoint;

    private ObjectPool<Zombie> _zombiePool;

    public void Reset(ObjectPool<Zombie> zombiePool, Vector3 spawnPoint)
    {
        _zombiePool = zombiePool;
        _agent = GetComponent<NavMeshAgent>();
        _spawnPoint = spawnPoint;
        _agent.Warp(_spawnPoint);
        Vector3 randomPoint;
        if (GetRandomPoint(_spawnPoint, _wanderRangeMax, out randomPoint))
        {
            _agent.SetDestination(randomPoint);
        }
        else
        {
            _agent.isStopped = true;
            _zombiePool.ReturnObject(this);
        }
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        //_spawnPoint = transform.position;
        //_agent.SetDestination(GetRandomPoint(_spawnPoint, _wanderRangeMin));
    }

    void Update()
    {
        if (_agent.remainingDistance <= 0.02f)
        {
            Vector3 randomPoint;
            if (GetRandomPoint(_spawnPoint, _wanderRangeMax, out randomPoint))
            {
                _agent.SetDestination(randomPoint);
            }
            else
            {
                _agent.isStopped = true;
                _zombiePool.ReturnObject(this);
            }
        }
    }

    bool GetRandomPoint(Vector3 center, float radius, out Vector3 randomPoint)
    {
        Vector3 randomDirection;

        do
        {
            randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                if (Vector3.Distance(transform.position, hit.position) > _wanderRangeMin)
                {
                    randomPoint = hit.position;
                    return true;
                }
            }

            if(!_agent.isOnNavMesh)
            {
                randomPoint = Vector3.zero;
                return false;
            }

        }while (true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            _agent.isStopped = true;
            _zombiePool.ReturnObject(this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("End Collision");
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground End Collision");
            _zombiePool.ReturnObject(this);
        }
    }
}
