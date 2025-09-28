using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ZombieDetector _car;
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private GameObject _gameOverScreen;

    private void Start()
    {
        _car.DeathEvent += OnGameOver;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_carMovement.IsAlive)
            Restart();
    }

    private void OnGameOver()
    {
        _gameOverScreen.SetActive(true);
        _carMovement.IsAlive = false;
    }

    private void Restart()
    {
        _car.Restart();
        _carMovement.IsAlive = true;
        _gameOverScreen.SetActive(false);
    }

}
