using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    public bool IsAlive { get; set; }

    void Update()
    {
        if (IsAlive)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
