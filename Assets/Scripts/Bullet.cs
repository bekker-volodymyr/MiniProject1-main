using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 25f;

    private ObjectPool<Bullet> _pool;

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(IsOffScreen())
        {
            _pool.ReturnObject(this);
        }
    }

    bool IsOffScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if(viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombie"))
        {
            _pool.ReturnObject(this);
        }
    }
}
