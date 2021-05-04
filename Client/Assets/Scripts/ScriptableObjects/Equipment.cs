using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlotEnum equipSlot;

    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();

        //equip the item
        EquipmentManager.instance.Equip(this);

        //remove from inventory
        RemoveFromInventory();
    }

}

/// <summary>
/// Equipment Slot and your respective items
/// </summary>
public enum EquipmentSlotEnum
{
    Mount,
    Medal,
    Earring1,
    Earring2,
    Ring1,
    Ring2,
    Head,
    Chest,
    Legs,
    Hands,
    Feet,
    Weapon,
    Shield,
    Cape,
    Amulet1,
    Amulet2,
    Guild
}
