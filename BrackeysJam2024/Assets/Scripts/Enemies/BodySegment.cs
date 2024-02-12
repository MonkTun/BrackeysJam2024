using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    public Transform followTarget;
    [SerializeField] private float _jointLength = 1;
    [SerializeField] private float _movementSpeed=1;
    private void Update()
    {
        if (CalculateDistSqr(transform.position, followTarget.position) > _jointLength * _jointLength)
        {
            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, _movementSpeed*Time.deltaTime);
        }
    }
    public static float CalculateDistSqr(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }
}
