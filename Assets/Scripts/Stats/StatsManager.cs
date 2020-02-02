using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{

    [SyncVar] public int Damage;
    [SyncVar] public int Armour;
    [SyncVar] public int MoveSpeed;
}
