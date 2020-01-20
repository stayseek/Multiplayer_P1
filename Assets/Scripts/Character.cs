using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    protected Vector3 _startPosition;
    [SerializeField] protected float _reviveDelay = 5f;
    protected float _reviveTime;

    [SerializeField] protected GameObject gfx;

    void Start()
    {
        _startPosition = transform.position;
        _reviveTime = _reviveDelay;
    }

    void Update()
    {
        OnUpdate();
    }

    protected override void Die()
    {
        base.Die();
        gfx.SetActive(false);
    }
    protected override void Revive()
    {
        base.Revive();
        transform.position = _startPosition;
        gfx.SetActive(true);
        if (isServer)
        {
            _motor.MoveToPoint(_startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!_isDead)
        {
            _motor.MoveToPoint(point);
        }
    }
}
