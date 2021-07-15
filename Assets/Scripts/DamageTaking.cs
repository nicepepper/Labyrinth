using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaking : MonoBehaviour
{
    [SerializeField] private int _hitPoint = 10;
    [SerializeField] private GameObject _destructionPrefab;
    [SerializeField] private bool _gameOverOnDestroyed = false;
    [SerializeField] private GameManager _game;

    public void TakeDamage(int amount)
    {
        _hitPoint -= amount;
        if (_hitPoint <= 0)
        {
            Debug.Log( "Destroy : "+ gameObject.name);
            Destroy(gameObject);
            if (_destructionPrefab != null)
            {
                Instantiate(_destructionPrefab, transform.position, transform.rotation);
            }

            if (_gameOverOnDestroyed == true)
            {
                Debug.Log("GameOver!");
                _game.GameOver();
            }
        }
    }

    public void SetGameManager(GameManager game)
    {
        _game = game;
    }
}
