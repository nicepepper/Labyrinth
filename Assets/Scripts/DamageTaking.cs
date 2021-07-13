using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaking : MonoBehaviour
{
    [SerializeField] private int _hitPoint = 10;
    [SerializeField] private GameObject _destructionPrefab;
    [SerializeField] private bool _gameOverOnDestroyed = false;

    public void TakeDamage(int amount)
    {
        Debug.Log(gameObject.name + " damaged!");

        _hitPoint -= amount;
        
        if (_hitPoint <= 0)
        {
            //Debug.Log(gameObject.name + " destroyed!");
           
            Destroy(gameObject);
            
            if (_destructionPrefab != null)
            {
                Instantiate(_destructionPrefab, transform.position, transform.rotation);
            }

            if (_gameOverOnDestroyed == true)
            {
                //GameManager
            }
        }
    }
}
