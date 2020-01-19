using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    [SerializeField] protected Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
    private static readonly int Moving = Animator.StringToHash("Move");

    void FixedUpdate ()
    {
        _animator.SetBool(Moving, _agent.hasPath);
    }

    //Placeholder functions for Animation events
    void Hit() 
    {
    }

    void FootR() 
    {
    }

    void FootL() 
    {
    }
}
