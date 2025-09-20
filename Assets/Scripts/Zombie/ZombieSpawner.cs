using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 0.2f;
    [SerializeField] private float _spawnRange = 15f;
    [SerializeField] private Zombie _zombiePrefab;

    [SerializeField] private Transform _carTransform;
    public Transform CarTransfrom => _carTransform;

    private ObjectPool<Zombie> _zombiePool;

    private float timer;

    void Start()
    {
        timer = _spawnRate;
        _zombiePool = new ObjectPool<Zombie>(_zombiePrefab);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnZombie();
            timer = _spawnRate;
        }
    }

    private void SpawnZombie()
    {
        Vector3 spawnPoint = new Vector3(Random.Range(-9, 9), 0.09f, _carTransform.position.z + _spawnRange);
        Zombie newZombie = _zombiePool.GetObject();
        newZombie.Init(_zombiePool, this, spawnPoint);
    }
}
