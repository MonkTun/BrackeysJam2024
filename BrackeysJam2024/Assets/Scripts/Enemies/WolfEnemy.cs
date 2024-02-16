using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEnemy : EnemyBase
{
    protected WolfEnemy _currentTriggerWolf;
    public WolfBase wolfBase;
    [SerializeField] protected float _packDistance;
    [SerializeField] protected float _minWolvesForPack;
    private float _timeOfLastHowl;
    [SerializeField] protected float _timeBetweenHowls;
    public override void IdleTransitions()
    {
        if (_canSeePlayer)
        {
            _currentTriggerWolf = null;
            wolfBase.SetPackTarget(this, PlayerMovement.instance.transform.position);
            if (CountNearbyWolves() >= _minWolvesForPack)
            {
                currentState = EnemyState.aggressive;
                return;
            }
        }
        base.IdleTransitions();
    }
    public override void DefensiveTransitions()
    {
        if(CountNearbyWolves() >= _minWolvesForPack)
        {
            currentState = EnemyState.aggressive;
            return;
        }
        if (!_canSeePlayer) _currentTriggerWolf = null;
        base.DefensiveTransitions();
    }
    public override void AggressiveBehaviour()
    {
        if (Time.time - _timeOfLastHowl > _timeBetweenHowls)
        {
            _animator.SetTrigger("Howl");
            wolfBase.SetPackTarget(this, PlayerMovement.instance.transform.position);
            _timeOfLastHowl = Time.time;
        }
        base.AggressiveBehaviour();
    }
    public override void AggressiveTransitions()
    {
        if(!_canSeePlayer)_currentTriggerWolf = null;
        base.AggressiveTransitions();
    }
    public void ReceivePackTarget(WolfEnemy trigger, Vector2 target)
    {
        _timeOfLastHowl = Time.time;
        currentExplorationTarget = target;
        _navAgent.SetDestination(target);
    }
    private void OnDestroy()
    {
        wolfBase.wolves.Remove(this);
    }
    
    private int CountNearbyWolves()
    {
        int total = 0;
        foreach(WolfEnemy w in wolfBase.wolves)
        {
            if (CalculateDistSqr(w.transform.position, transform.position)< _packDistance* _packDistance) 
            {
                total++;
            }
        }
        return total;
    }
}
