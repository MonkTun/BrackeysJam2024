
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GekkoEnemy : EnemyBase
{
    [Range(0,1)]
    private float _transformationPercentage;
    private bool _isTransforming;
    [Header("Gekko Specific Fields")]
    [SerializeField] private float _startTransformingDistance;
    [SerializeField] private float _stopTransformingDistance;
    [SerializeField] private float _resetHuntDistance;
    [SerializeField] private float _transformingRate;
    [SerializeField] private AnimationCurve _transformingPercentageToAlpha;
    [SerializeField] private AnimationCurve _transformingPercentageToSpeed;
    [SerializeField] private SpriteRenderer rend;
    private bool _isBeingLookedAt;
    private bool _wasFound;
    [SerializeField] private float _invisibleTransformationThreshold;
    protected override void Awake()
    {
        if (rend == null) { rend = GetComponent<SpriteRenderer>(); }
        base.Awake();
    }
    public override void IdleTransitions()
    {
        Debug.Log("Dist: " + CalculateDistSqr(PlayerMovement.instance.transform.position, transform.position));
        if (!_wasFound)
        {
            if (CalculateDistSqr(PlayerMovement.instance.transform.position, baseLocation.position) < _maxRoamingDistance * _maxRoamingDistance)
            {
                currentState = EnemyState.aggressive;
            }
        }
        else if(CalculateDistSqr(PlayerMovement.instance.transform.position, transform.position)> _resetHuntDistance* _resetHuntDistance)
        {
            _wasFound = false;
        }
    }
    public override void AggressiveBehaviour()
    {
        float distSqr = CalculateDistSqr(transform.position, PlayerMovement.instance.transform.position);
        Debug.Log("Can see player: " + CanSeePlayer() + " distance: " + distSqr);
        if (!_isTransforming)
        {
            if (CanSeePlayer() && distSqr < _startTransformingDistance * _startTransformingDistance)
            {
                _isTransforming = true;
            }
            else
            {
                _transformationPercentage = Mathf.Clamp01(_transformationPercentage - _transformingRate*Time.deltaTime);
            }
        }
        else if(!_isBeingLookedAt||_transformationPercentage>_invisibleTransformationThreshold)
        {
            if(distSqr> _stopTransformingDistance * _stopTransformingDistance)
            {
                _isTransforming = false;
            }
            else
            {
                _transformationPercentage = Mathf.Clamp01(_transformationPercentage + _transformingRate * Time.deltaTime);
            }
        }
        _navAgent.speed = _transformingPercentageToSpeed.Evaluate(_transformationPercentage) * _agentSpeed;

        base.AggressiveBehaviour();
    }
    protected override void Update()
    {
        rend.color = new Color(1, 1, 1, _transformingPercentageToAlpha.Evaluate(_transformationPercentage));
        base.Update();
    }
    public override void AggressiveTransitions()
    {

        //Deagresses if too far away from base
        if (CalculateDistSqr(transform.position, baseLocation.position) > _maxRoamingDistance * _maxRoamingDistance)
        {
            _navAgent.speed = _agentSpeed;
            _isTransforming = false;
            FindExplorationTarget(); currentState = EnemyState.idle;
        }
        else if (_isBeingLookedAt && _transformationPercentage < _invisibleTransformationThreshold)
        {
            _navAgent.speed = _agentSpeed;
            _isTransforming = false;
            currentState = EnemyState.idle;
            _wasFound = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Light")
        {
            _isBeingLookedAt = true;
        }
    }
    private void LateUpdate()
    {
        _isBeingLookedAt = false;
    }
}
