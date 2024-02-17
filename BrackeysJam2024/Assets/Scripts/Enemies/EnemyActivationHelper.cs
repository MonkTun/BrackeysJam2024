using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivationHelper : MonoBehaviour
{
    //Only activates enemy once player is close enough
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _activationDistance;
    private void Start()
    {
        if (_enemy == null) { _enemy = transform.GetChild(0).gameObject; }
        _enemy.SetActive(false);
    }
    private void Update()
    {
        if (CalculateDistSqr(_enemy.transform.position, PlayerMovement.instance.transform.position) < _activationDistance * _activationDistance)
        {
            _enemy.SetActive(true);
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }

    public static float CalculateDistSqr(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }
}
