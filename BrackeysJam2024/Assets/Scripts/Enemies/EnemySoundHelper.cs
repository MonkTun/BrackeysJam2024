using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EnemySoundHelper : MonoBehaviour
{
    [SerializeField] private MMF_Player _mmfPlayer;
    [SerializeField] private float _minDistanceBetweenFootsteps;
    [SerializeField] private float _maxDistanceBetweenFootsteps;
    private float _currentDistanceBetweenFootsteps;
    private Vector2 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        if (_mmfPlayer == null) { _mmfPlayer = GetComponent<MMF_Player>(); }
        _currentDistanceBetweenFootsteps = Random.Range(_minDistanceBetweenFootsteps, _maxDistanceBetweenFootsteps);
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (CalculateDistSqr(lastPos, transform.position) > _currentDistanceBetweenFootsteps * _currentDistanceBetweenFootsteps)
        {
            _currentDistanceBetweenFootsteps = Random.Range(_minDistanceBetweenFootsteps, _maxDistanceBetweenFootsteps);
            _mmfPlayer.PlayFeedbacks();
            lastPos = transform.position;
        }
    }
    public static float CalculateDistSqr(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }
}
