using UnityEngine;

public class ZombieDeathEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private ObjectPool<ZombieDeathEffect> _pool;

    public void Spawn(Vector3 position, ObjectPool<ZombieDeathEffect> pool)
    {
        transform.position = position;
        _pool = pool;
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        _pool.ReturnObject(this);
    }
}
