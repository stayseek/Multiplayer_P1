using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    private static readonly int Moving = Animator.StringToHash("Moving");

    void FixedUpdate ()
    {
        animator.SetBool(Moving, agent.hasPath);
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
