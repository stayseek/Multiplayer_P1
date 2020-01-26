﻿using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{

    public Transform DropPoint;
    public int Space = 20;
    public event SyncList<Item>.SyncListChanged OnItemChanged;

    public SyncListItem Items = new SyncListItem();

    public override void OnStartLocalPlayer()
    {
        Items.Callback += ItemChanged;
    }

    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        OnItemChanged(op, itemIndex);
    }

    public bool Add(Item item)
    {
        if (Items.Count < Space)
        {
            Items.Add(item);
            return true;
        }
        else return false;
    }

    public void Remove(Item item)
    {
        CmdRemoveItem(Items.IndexOf(item));
    }

    [Command]
    void CmdRemoveItem(int index)
    {
        if (Items[index] != null)
        {
            Drop(Items[index]);
            Items.RemoveAt(index);
        }
    }

    private void Drop(Item item)
    {
        PickUpItem pickupItem = Instantiate(item.pickUpPrefab, DropPoint.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
        pickupItem.Item = item;
        NetworkServer.Spawn(pickupItem.gameObject);
    }
}
