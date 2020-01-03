using UnityEngine;
using UnityEngine.Networking;

public class NetPlayerSetup : NetworkBehaviour
{
    [SerializeField] MonoBehaviour[] _disableBehaviours;

    private void Awake()
    {
        if (!hasAuthority)
        {
            for (int i = 0; i < _disableBehaviours.Length; i++)
            {
                _disableBehaviours[i].enabled = false;
            }
        } 
    }

    public override void OnStartAuthority()
    {
        for (int i =0; i< _disableBehaviours.Length; i++)
        {
            _disableBehaviours[i].enabled = true;
        }
    }
}
