
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
    private float _actualContactDistance;
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
        if (currentState == EnemyState.aggressive && Time.time - _timeOfLastAttack < 1f) { _wasFound = true; _transformationPercentage = 1; _isTransforming = false;
            currentState = EnemyState.idle;
        }
        if (_transformationPercentage < 0.5f) { _hasInstantContactDamage = false; }
        else { _hasInstantContactDamage = true; }
        base.Update();

        _animator.SetBool("isSearching", _transformationPercentage > 0.5f);
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
    public override void StartAttack()
    {

    }
    public override void PlayerTooClose()
    {
        if (!_isAttacking && Time.time - _timeOfLastAttack > _timeBetweenAttacks && PlayerMovement.instance.gameObject.TryGetComponent<HealthManager>(out HealthManager hm))
        {
            if (_hasInstantContactDamage)
            {
                hm.isPoisoned = true;
            }
            else
            {
                StartAttack();
            }
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
