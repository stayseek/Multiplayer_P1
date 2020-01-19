using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour 
{

    private NavMeshAgent _agent;

    void Start () 
    {
        _agent = GetComponent<NavMeshAgent>();
	}
	
	public void MoveToPoint(Vector3 point) 
    {
        _agent.SetDestination(point);
    }
}
