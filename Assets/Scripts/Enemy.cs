﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
public class Enemy : Unit
{
    [Header("Movement")]
    [SerializeField] private float _moveRadius = 10f;
    [SerializeField] private float _minMoveDelay = 4f;
    [SerializeField] private float _maxMoveDelay = 12f;
    private Vector3 _startPosition;
    private Vector3 _curDistanation;
    private float _changePosTime;

    [Header("Behavior")]
    [SerializeField] private bool _aggressive;
    [SerializeField] private float _rewardExp;
    [SerializeField] private float _viewDistance = 5f;
    [SerializeField] private float _reviveDelay = 5f;

    private float _reviveTime;
    private List<Character> _enemies = new List<Character>();

    void Start()
    {
        _startPosition = transform.position;
        _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        _reviveTime = _reviveDelay;
    }
    void Update()
    {
        OnUpdate();
    }
    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();
        if (_reviveTime > 0)
        {
            _reviveTime -= Time.deltaTime;
        }
        else
        {
            _reviveTime = _reviveDelay;
            Revive();
        }
    }
    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();
        if (_focus == null)
        {
            Wandering(Time.deltaTime);
            if (_aggressive) FindEnemy();
        }
        else
        {
            float distance = Vector3.Distance(_focus.InteractionTransform.position,
            transform.position);
            if (distance > _viewDistance || !_focus.HasInteract)
            {
                RemoveFocus();
            }
            else if (distance <= _focus.Radius)
            {
                if (!_focus.Interact(gameObject))
                {
                    RemoveFocus();
                }
            }
        }
    }
    void Wandering(float deltaTime)
    {
        _changePosTime -= deltaTime;
        if (_changePosTime <= 0)
        {
            RandomMove();
            _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        }
    }
    void RandomMove()
    {
        _curDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) *
        new Vector3(_moveRadius, 0, 0) + _startPosition;
        _motor.MoveToPoint(_curDistanation);
    }
    protected override void Die()
    {
        base.Die();
        if (isServer)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Player.Progress.AddExp(_rewardExp / _enemies.Count);
            }
            _enemies.Clear();
        }
    }
    protected override void Revive()
    {
        transform.position = _startPosition;
        base.Revive();
        if (isServer)
        {
            _motor.MoveToPoint(_startPosition);
        }
    }
    void FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,_viewDistance, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++)
        {
            Interactable interactable = colliders[i].GetComponent<Interactable>();
            if (interactable != null && interactable.HasInteract)
            {
                SetFocus(interactable);
                break;
            }
        }
    }
    public override bool Interact(GameObject user)
    {
        if (base.Interact(user))
        {
            SetFocus(user.GetComponent<Interactable>());
            return true;
        }
        return false;
    }
    protected override void DamageWithCombat(GameObject user)
    {
        base.DamageWithCombat(user);
        Character character = user.GetComponent<Character>();
        if (character != null && !_enemies.Contains(character))
        {
            _enemies.Add(character);
        }
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);
    }

}
