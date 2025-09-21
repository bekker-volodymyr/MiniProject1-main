using UnityEngine;

public class TurretShooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _spawnRate = 1.5f;
    private float timer;

    private ObjectPool<Bullet> _pool;

    void Start()
    {
        timer = _spawnRate;
        _pool = new ObjectPool<Bullet>(_bulletPrefab);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnBullet();
            timer = _spawnRate;
        }
    }

    private void SpawnBullet()
    {
        Bullet newBullet = _pool.GetObject();
        newBullet.transform.position = _spawnPoint.position;
        newBullet.transform.rotation = _spawnPoint.rotation;
        newBullet.gameObject.SetActive(true);
        // Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
    }
}
