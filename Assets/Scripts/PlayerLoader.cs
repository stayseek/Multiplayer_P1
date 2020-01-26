using UnityEngine;
using UnityEngine.Networking;

public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private PlayerController _controller;
    [SyncVar(hook = "HookUnitIdentity") ] private NetworkIdentity _unitIdentity;

    public Character CreateCharacter()
    {
        GameObject unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity, transform);
        NetworkServer.Spawn(unit);
        _unitIdentity = unit.GetComponent<NetworkIdentity>();
        unit.GetComponent<Character>().SetInventory(GetComponent<Inventory>());
        return unit.GetComponent<Character>();
    }

    public override void OnStartAuthority()
    {
        if (isServer)
        {
            Character character = CreateCharacter();
            _controller.SetCharacter(character, true);
            InventoryUI.Instance.SetInventory(character.Inventory);
        }
        else
        {
            CmdCreatePlayer();
        }
    }
    [Command]
    public void CmdCreatePlayer()
    {
        Character character = CreateCharacter();
        _controller.SetCharacter(character, false);
    }

    [ClientCallback]
    void HookUnitIdentity(NetworkIdentity unit)
    {
        if (isLocalPlayer)
        {
            _unitIdentity = unit;
            Character character = unit.GetComponent<Character>();
            _controller.SetCharacter(unit.GetComponent<Character>(), true);
            character.SetInventory(GetComponent<Inventory>());
            InventoryUI.Instance.SetInventory(character.Inventory);
        }
    }
    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }
}
