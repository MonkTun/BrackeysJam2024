using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    idle,
    defensive,
    aggressive
}
public class EnemyBase : MonoBehaviour
{
    //Classifications are not perfect particularly for runtime v. static values
    #region Reference Fields
    [Header("Reference Fields")]
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] Animator _animator;
    #endregion
    #region Runtime Values
    [Header("Runtime Values")]
    public EnemyState currentState;
    #region Vision Values
    [Header("Vision Values")]
    protected bool _canSeePlayer;
    [Tooltip("_startSeeingPlayerThreshold>_stopSeeingPlayerThreshold")]
    [SerializeField] protected float _startSeeingPlayerThreshold;
    [Tooltip("_startSeeingPlayerThreshold>_stopSeeingPlayerThreshold")]
    [SerializeField] protected float _stopSeeingPlayerThreshold;
    protected float _lastSeenPlayerTime = -1000;
    protected float _seenPlayerDuration = 0;
    protected const float _maxSeenPlayerDuration=30;
    [SerializeField]protected  float _forgetPlayerRate = 2;
    #endregion
    #region Attack Values
    [Header("Attack Values")]
    protected float _timeOfLastAttack=-1000;
    protected bool _isAttacking;
    [SerializeField] protected float _timeBetweenAttacks=1;
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
    [SerializeField] protected float _sightDistance;
    [SerializeField] protected bool _canBeDefensive;
    [SerializeField] protected float _minDefensiveDistance;
    #endregion
    #region Unity Functions
    private void Awake()
    {
        if (_navAgent == null) { _navAgent = GetComponent<NavMeshAgent>(); }
        if (_animator == null) { }
    }
    void Update()
    {
        //Update whether enemy can see player
        UpdateCanSeePlayer();
        //State Machine
        RunStateMachine();
    }
    #region Collision Functions
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

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
        if (_canSeePlayer&&_seenPlayerDuration < _stopSeeingPlayerThreshold)
        {
            _canSeePlayer = false;
        }
        else if(!_canSeePlayer && _seenPlayerDuration > _startSeeingPlayerThreshold)
        {
            _canSeePlayer = true;
        }

    }
    public virtual void RunStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                if (_isAttacking) IdleBehaviour();
                IdleTransitions();
                break;
            case EnemyState.defensive:
                if (_isAttacking) DefensiveBehaviour();
                DefensiveTransitions();
                break;
            case EnemyState.aggressive:
                if (_isAttacking) AggressiveBehaviour();
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
        _animator.SetTrigger("Attack");
        _isAttacking = true;
    }
    public virtual void DealAttack() //Checks hitbox and deals damage and kb
    {
        Collider2D[] cols=Physics2D.OverlapCircleAll(_attackOffset + (Vector2)transform.position, _attackRadius, _attackLayerMask);
        foreach(Collider2D col in cols)
        {
            //Get universal health script
            //Deal Damage
        }
    }
    public virtual void EndAttack() //Finishes attack animation and values
    {
        _isAttacking = false;
    }
    #endregion
    #endregion
    #region State Machine
    public virtual void IdleBehaviour()
    {
        //Choose target destination if it doesnt exist
        //Check for nearby food
        //Navigate to target destination
    }
    public virtual void IdleTransitions()
    {
        if (_canSeePlayer)
        {
            if (_canBeDefensive) currentState = EnemyState.defensive; 
            else currentState = EnemyState.aggressive; 
        }
    }
    public virtual void DefensiveBehaviour()
    {
        
    }
    public virtual void DefensiveTransitions()
    {
        Vector2 playerPos = Vector2.zero;
        if (!_canSeePlayer) currentState = EnemyState.idle;
        else if (Vector2.Distance(transform.position,playerPos)<_minDefensiveDistance) currentState = EnemyState.aggressive; 
    }
    public virtual void AggressiveBehaviour()
    {

    }
    public virtual void AggressiveTransitions() //Enemies will not naturally transition back to defensive unless overridden
    {
        if (!_canSeePlayer) currentState = EnemyState.idle;
    }
    #endregion
    #region Helper Functions
    public virtual bool CanSeePlayer()
    {
        Vector2 playerPos = Vector2.zero;
        Debug.LogError("UNFINISHED FUNCTION");
        if (Vector2.Distance(playerPos, transform.position) > _sightDistance) { return false; }
        RaycastHit2D rh = Physics2D.Raycast(transform.position, (playerPos - (Vector2)transform.position).normalized, _sightDistance,GlobalVariableHelper.instance.solidLayerMask);
        return rh.collider!=null&&rh.collider.gameObject!=null;
    }
    #endregion
}
