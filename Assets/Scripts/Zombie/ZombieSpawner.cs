using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 0.2f;
    [SerializeField] private float _spawnRange = 15f;
    [SerializeField] private Zombie _zombiePrefab;
    [SerializeField] private ZombieDeathEffect _zombieDeathEffectPrefab;

    [SerializeField] private Transform _carTransform;
    public Transform CarTransfrom => _carTransform;

    private ObjectPool<Zombie> _zombiePool;
    private ObjectPool<ZombieDeathEffect> _effectPool;

    private float timer;

    void Start()
    {
        timer = _spawnRate;
        _zombiePool = new ObjectPool<Zombie>(_zombiePrefab);
        _effectPool = new ObjectPool<ZombieDeathEffect>(_zombieDeathEffectPrefab);
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
        Vector3 spawnPoint = new Vector3(Random.Range(-9, 9), 1.7f, _carTransform.position.z + _spawnRange);
        Zombie newZombie = _zombiePool.GetObject();
        newZombie.DeathEvent -= SpawnDeathEffect;
        newZombie.DeathEvent += SpawnDeathEffect;
        newZombie.Init(_zombiePool, this, spawnPoint);
    }

    private void SpawnDeathEffect(Vector3 position)
    {
        ZombieDeathEffect effect = _effectPool.GetObject();
        effect.gameObject.SetActive(true);
        effect.Spawn(position, _effectPool);
    }
}
