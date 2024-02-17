using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
public enum EnemyState
{
    idle,
    defensive,
    aggressive
}
public enum IdleState
{
    exploring,
    searching,
    returning
}
public class EnemyBase : MonoBehaviour
{
    //Classifications are not perfect particularly for runtime v. static values
    #region Reference Fields
    [Header("Reference Fields")]
    [SerializeField] protected NavMeshAgent _navAgent;
    [SerializeField] protected Animator _animator;
    #region Audio Clips
    [Header("Audio Clips")]
    [SerializeField] protected AudioClip _aggroAudioClip;
    [SerializeField] protected AudioClip _defenseAudioClip;
    [SerializeField] protected AudioClip _deathAudioClip;
    #endregion
    #endregion
    #region Runtime Values
    [Header("Runtime Values")]
    public EnemyState currentState;
    public IdleState currentIdleState;
    protected float timeOfLastSwitchIdleState = -1000;
    #region Movement Values
    [Header("Movement values")]
    public Transform baseLocation;
    public Vector2 currentExplorationTarget;
    #region Idle State
    protected float _timeOfLastSearch = 0;
    [SerializeField] protected float _timeBetweenSearches = 5;
    [SerializeField] protected float _timeBetweenSearchesVariance = 2;
    [SerializeField] protected float _searchDuration = 3;
    [SerializeField] protected float _searchDurationVariance = 3;
    protected float _currentSearchDuration;
    protected float _currentTimeBetweenSearches;
    #endregion
    #endregion
    #region Vision Values
    [Header("Vision Values")]
    protected bool _canSeePlayer;
    [Tooltip("_startSeeingPlayerThreshold>_stopSeeingPlayerThreshold")]
    [SerializeField] protected float _startSeeingPlayerThreshold;
    [Tooltip("_startSeeingPlayerThreshold>_stopSeeingPlayerThreshold")]
    [SerializeField] protected float _stopSeeingPlayerThreshold;
    protected float _lastSeenPlayerTime = -1000;
    protected float _seenPlayerDuration = 0;
    protected const float _maxSeenPlayerDuration = 10;
    [SerializeField] protected float _forgetPlayerRate = 2;
    #endregion
    #region Attack Values
    [Header("Attack Values")]
    protected float _timeOfLastAttack = -1000;
    protected bool _isAttacking;
    [SerializeField] protected float _timeBetweenAttacks = 1;
    [SerializeField] protected float _attackDamage = 10;
    [SerializeField] protected float _attackKnockback = 10;
    #region Hitbox Values
    [Header("Hitbox Values")]
    [SerializeField] protected Vector2 _attackOffset;
    [SerializeField] protected float _attackRadius;
    [SerializeField] protected LayerMask _attackLayerMask;
    #endregion
    #endregion
    #endregion 
    #region Static Values
    [Header("Static Values")]
    [SerializeField] protected float _sightDistance; //Sight distance when aggresive, searching, or defensive
    [SerializeField] protected float _idleSightDistance;//Sight distance when exploring/retreating in idle state
    [SerializeField] protected bool _canBeDefensive;
    [SerializeField] protected float _minDefensiveDistance;
    [SerializeField] protected float _startAttackRange;
    [SerializeField] protected float _agentSpeed;
    [SerializeField] protected float _maxRoamingDistance = 20;
    [SerializeField] protected float _maxRoamingDistanceVariance = 2;
    protected float _currentMaxRoamingDistance;
    [Tooltip("How much food the player is worth")]
    [SerializeField] protected float _playerFoodValue;
    [Tooltip("Minimum distance before enemy switches from idle back to exploring")]
    [SerializeField] protected float _minRetreatingBaseDistance;
    [SerializeField] protected bool _hasInstantContactDamage = true;
    #endregion
    #region Unity Functions
    protected virtual void Awake()
    {
        if (_navAgent == null) { _navAgent = GetComponent<NavMeshAgent>(); }
        _navAgent.speed = _agentSpeed;
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
        if (_animator == null) { _animator = GetComponent<Animator>(); }
        UpdateRuntimeValues();
    }
    protected virtual void Start()
    {
        //Set new exploration target
        FindExplorationTarget();
    }
    protected virtual void Update()
    {
        if (Random.Range(0f,1f)<0.1f&&_navAgent.isPathStale||_navAgent.pathStatus!=NavMeshPathStatus.PathComplete) { FindExplorationTarget(); }
        _animator.SetBool("isSearching", currentState == EnemyState.idle && currentIdleState == IdleState.searching);
        //Orient towards navmesh motion
        if(!_isAttacking)FaceTarget();
        UpdateCanSeePlayer();
        //State Machine
        RunStateMachine();

    }
    private void OnDestroy()
    {
        if (_deathAudioClip != null) { PlaySFXClip(_deathAudioClip); }
    }
    void FaceTarget()
    {
        Vector3 direction = _navAgent.velocity;
        if (direction.magnitude <= Mathf.Epsilon) { return; }
        transform.rotation = Quaternion.Euler(0,0,90+((direction.x<0)?0:180)+Mathf.Atan(direction.y/direction.x)*180/Mathf.PI);
    }
    void FacePlayer()
    {
        Vector3 direction =  PlayerMovement.instance.transform.position-transform.position;
        if (direction.magnitude <= Mathf.Epsilon) { return; }
        transform.rotation = Quaternion.Euler(0, 0, 90 + ((direction.x < 0) ? 0 : 180) + Mathf.Atan2(direction.x,direction.y) * 180 / Mathf.PI);
    }
    #region Collision Functions
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!_isAttacking&&Time.time-_timeOfLastAttack>_timeBetweenAttacks&& collision.gameObject.TryGetComponent<HealthManager>(out HealthManager hm) )
            {
                if (_hasInstantContactDamage)
                {
                    _timeOfLastAttack = Time.time;
                    Debug.Log("Damaged enemy: " + collision.gameObject.name);
                    hm.changeHealth(_attackDamage);
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position).normalized * _attackKnockback);

                }
                else
                {
                    StartAttack();
                }
            }
        }
    }
    #endregion
    #endregion
    #region Primary Functions
    public virtual void UpdateCanSeePlayer()
    {
        if (CanSeePlayer() && _seenPlayerDuration < _maxSeenPlayerDuration)
        {
            _lastSeenPlayerTime = Time.time;
            _seenPlayerDuration += Time.deltaTime;
            if (_seenPlayerDuration > _maxSeenPlayerDuration) _seenPlayerDuration = _maxSeenPlayerDuration;
        }
        else if (_seenPlayerDuration > 0)
        {
            _seenPlayerDuration -= Time.deltaTime * _forgetPlayerRate;
            if (_seenPlayerDuration < 0) _seenPlayerDuration = 0;
        }
        if (_canSeePlayer && _seenPlayerDuration < _stopSeeingPlayerThreshold)
        {
            _canSeePlayer = false;
        }
        else if (!_canSeePlayer && _seenPlayerDuration > _startSeeingPlayerThreshold)
        {
            _canSeePlayer = true;
        }

    }
    public virtual void RunStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                if (!_isAttacking) IdleBehaviour();
                IdleTransitions();
                break;
            case EnemyState.defensive:
                if (!_isAttacking) DefensiveBehaviour();
                DefensiveTransitions();
                break;
            case EnemyState.aggressive:
                if (!_isAttacking) AggressiveBehaviour();
                AggressiveTransitions();
                break;
            default:
                Debug.LogError("UNKNOWN STATE FOR " + gameObject.name);
                break;
        }
    }
    #region Attack Functions
    public virtual void StartAttack() //Begins attack windup
    {
        //Face player
        FacePlayer();
        _animator.SetTrigger("Attack");
        _isAttacking = true;
        _navAgent.speed = 0;
    }
    public virtual void DealAttack() //Checks hitbox and deals damage and kb
    {
        Debug.Log("Attacking");
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2)transform.TransformPoint(_attackOffset), _attackRadius, _attackLayerMask);
        foreach (Collider2D col in cols)
        {
            Debug.Log("Hit enemy: " + col.name);
            if (col.gameObject != gameObject && col.TryGetComponent<HealthManager>(out HealthManager hm))
            {

                Debug.Log("Damaged enemy: " + col.name);
                hm.changeHealth(_attackDamage);
                col.GetComponent<Rigidbody2D>().AddForce((col.gameObject.transform.position - transform.position).normalized * _attackKnockback);
            }
        }
    }
    public virtual void EndAttack() //Finishes attack animation and values
    {
        _isAttacking = false;
        _navAgent.speed = _agentSpeed;
    }
    #endregion
    #endregion
    #region State Machine
    public virtual void IdleBehaviour()
    {
        switch (currentIdleState)
        {
            case IdleState.exploring:
                ExploringBehaviour();
                ExploringTransitions();
                break;
            case IdleState.returning:
                ReturningBehaviour();
                ReturningTransitions();
                break;
            case IdleState.searching:
                SearchingBehaviour();
                SearchingTransitions();
                break;
            default:
                break;
        }
    }
    public virtual void IdleTransitions()
    {
        if (_canSeePlayer)
        {
            if (_canBeDefensive) { currentState = EnemyState.defensive; if (_defenseAudioClip != null) { PlaySFXClip(_defenseAudioClip); } }
            else { currentState = EnemyState.aggressive; if (_aggroAudioClip != null) { PlaySFXClip(_aggroAudioClip); } }
        }
    }
    public virtual void DefensiveBehaviour()
    {
        //Stays still for now add more complex behaviour afterwards
        _navAgent.SetDestination((Vector2)transform.position);
    }
    public virtual void DefensiveTransitions()
    {
        Vector2 playerPos = PlayerMovement.instance.transform.position;
        if (!_canSeePlayer) { FindExplorationTarget(); _navAgent.speed = _agentSpeed; currentState = EnemyState.idle; }
        else if (Vector2.Distance(transform.position, playerPos) < _minDefensiveDistance) { _navAgent.speed = _agentSpeed; currentState = EnemyState.aggressive; if (_aggroAudioClip != null) { PlaySFXClip(_aggroAudioClip); } }
    }
    public virtual void AggressiveBehaviour()
    {
        _navAgent.SetDestination((Vector2)PlayerMovement.instance.transform.position);
        Vector2 playerPos = PlayerMovement.instance.transform.position;

        if (!_isAttacking&&Time.time-_timeOfLastAttack>_timeBetweenAttacks&&Vector2.Distance(transform.position, playerPos) < _attackRadius && CanSeePlayer())
        {
            
            StartAttack();
        }

    }
    public virtual void AggressiveTransitions() //Enemies will not naturally transition back to defensive unless overridden
    {
        if (!_canSeePlayer){ FindExplorationTarget(); currentState = EnemyState.idle; }
    }
    #region Idle State Machine
    public virtual void ExploringBehaviour()
    {
        //Just navmesh agent stuff, does not need to update
    }
    public virtual void ExploringTransitions()
    {
        if (Vector2.Distance(transform.position, baseLocation.position) > _currentMaxRoamingDistance||(Vector2.Distance(currentExplorationTarget,transform.position)<_idleSightDistance))
        {
            _currentMaxRoamingDistance = _maxRoamingDistance + Random.Range(-_maxRoamingDistanceVariance, _maxRoamingDistanceVariance);
            timeOfLastSwitchIdleState = Time.time;
            currentIdleState = IdleState.returning;
        }
        else if (Time.time - _timeOfLastSearch > _currentTimeBetweenSearches)
        {
            _currentTimeBetweenSearches = _timeBetweenSearches + Random.Range(-_timeBetweenSearchesVariance, _timeBetweenSearchesVariance);
            timeOfLastSwitchIdleState = Time.time;
            currentIdleState = IdleState.searching;
            _navAgent.speed = 0;
            Transform target = FindBestSorroundingTarget();
            if (target != null)
            {
                _navAgent.SetDestination((Vector2)target.position);
            }
        }
    }
    public virtual void ReturningBehaviour()
    {
        if ((Vector2)_navAgent.destination != (Vector2)baseLocation.position) _navAgent.SetDestination((Vector2)baseLocation.position);
    }
    public virtual void ReturningTransitions()
    {
        float distanceFromBase = Vector2.Distance(transform.position, (Vector2)baseLocation.position);
        if (distanceFromBase < _minRetreatingBaseDistance && CanSeeTarget(baseLocation.position))
        {
            FindExplorationTarget();
            currentIdleState = IdleState.exploring;
        }

    }
    public virtual void SearchingBehaviour()
    {
        //Stand still, checks at start and end of search, intentionally left blank
    }
    public virtual void SearchingTransitions()
    {
        Transform target = FindBestSorroundingTarget();
        if (target != null)
        {
            _navAgent.SetDestination((Vector2)target.position);
        }
        if (Time.time - timeOfLastSwitchIdleState > _currentSearchDuration)
        {
            _timeOfLastSearch = Time.time;
            _navAgent.speed = _agentSpeed;
            timeOfLastSwitchIdleState = Time.time;
            currentIdleState = IdleState.exploring;
            _currentSearchDuration = _searchDuration + Random.Range(-_searchDurationVariance, _searchDurationVariance);
        }
    }

    #endregion
    #endregion
    #region Helper Functions
    public virtual bool CanSeePlayer()
    {
        Vector2 playerPos = (Vector2)PlayerMovement.instance.transform.position;
        if (Vector2.Distance(playerPos, transform.position) > ((currentState!=EnemyState.idle||currentIdleState!=IdleState.searching)?_sightDistance:_idleSightDistance)) { return false; }
        return CanSeeTarget(playerPos);
    }
    public virtual bool CanSeeTarget(Vector2 target)
    {
        float dist = Vector2.Distance(target,transform.position);
        if(dist> ((currentState != EnemyState.idle || currentIdleState != IdleState.searching) ? _sightDistance : _idleSightDistance)) { return false; }
        RaycastHit2D rh = Physics2D.Raycast(transform.position, (target - (Vector2)transform.position).normalized, 
            dist, 
            GlobalVariableHelper.instance.solidLayerMask);
        return rh.collider == null || rh.collider.gameObject == null; //Only considers walls/obstacles
    }
    public virtual void UpdateRuntimeValues()
    {
        _currentMaxRoamingDistance = _maxRoamingDistance + Random.Range(-_maxRoamingDistanceVariance, _maxRoamingDistanceVariance);
        _currentTimeBetweenSearches = _timeBetweenSearches + Random.Range(-_timeBetweenSearchesVariance, _timeBetweenSearchesVariance);
        _currentSearchDuration = _searchDuration + Random.Range(-_searchDurationVariance, _searchDurationVariance);
    }
    public virtual Transform FindBestSorroundingTarget()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _sightDistance, GlobalVariableHelper.instance.lootLayerMask);
        float currentMaxValue = 0;
        Transform currentMaxTransform = null;
        foreach (Collider2D col in cols)
        {
            if (col.TryGetComponent<PickupItem>(out PickupItem pi))
            {
                if (pi._item.isEdible && pi._item._foodValue > currentMaxValue)
                {
                    currentMaxValue =pi._item._foodValue;
                    currentMaxTransform = col.transform;
                }
            }
            else if (col.gameObject.tag == "Player")
            {
                if (_playerFoodValue > currentMaxValue)
                {
                    currentMaxValue = _playerFoodValue;
                    currentMaxTransform = col.transform;
                }
            }
        }
        return currentMaxTransform;
    }
    public virtual bool FindExplorationTarget()
    {
        for (int i = 0; i < 10; i++) {
            if(NavMesh.SamplePosition(Random.insideUnitCircle * (_maxRoamingDistance + _maxRoamingDistanceVariance), out NavMeshHit hit, 2, NavMesh.AllAreas))
            {
                currentExplorationTarget = hit.position;
                _navAgent.SetDestination(currentExplorationTarget);
                
                return true;
            }
        }
        Debug.LogError("UNABLE TO FIND EXPLORATION TARGET POINT");
        return false;
    }

    public static float CalculateDistSqr(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }

    public void PlaySFXClip(AudioClip clip)
    {
        MMSoundManagerSoundPlayEvent.Trigger(clip, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
    }
    #endregion
}
