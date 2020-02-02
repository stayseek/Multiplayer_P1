using UnityEngine;

[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item
{
    public EquipmentSlotType equipSlot;

    public int damageModifier;
    public int armorModifier;
    public int speedModifier;

    public override void Use(Player player)
    {
        player.Inventory.RemoveItem(this);
        EquipmentItem oldItem = player.Equipment.EquipItem(this);
        if (oldItem != null) player.Inventory.AddItem(oldItem);
        base.Use(player);
    }
}
