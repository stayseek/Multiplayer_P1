using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Unit : NetworkBehaviour
{
    [SerializeField] protected UnitMotor _motor;
    [SerializeField] protected UnitStats _myStats;
    protected bool _isDead;

    void Update()
    {
        OnUpdate();
    }
    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }
    protected void OnUpdate()
    {
        if (isServer)
        {
            if (!_isDead)
            {
                if (_myStats.CurHealth == 0) Die();
                else OnAliveUpdate();
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    [ClientRpc] void RpcDie()
    {
        if (!isServer) Die();
    }
    [ClientRpc] void RpcRevive()
    {
        if (!isServer) Revive();
    }

    protected virtual void Die()
    {
        _isDead = true;
        if (isServer)
        {
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }
    protected virtual void Revive()
    {
        _isDead = false;
        if (isServer)
        {
            _myStats.SetHealthRate(1);
            RpcRevive();
        }
    }
}
