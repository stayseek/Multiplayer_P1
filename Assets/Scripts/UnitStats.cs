using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SyncVar] protected int _curHealth;
    public Stat damage;

    public override void OnStartServer()
    {
        _curHealth = _maxHealth;
    }

    public int CurHealth 
    { 
        get 
        { 
            return _curHealth;
        } 
    }
    public void SetHealthRate(float rate)
    {
        _curHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
}
