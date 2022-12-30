using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    public int SlotID;
    public ItemObject item;

    public Slot(int slotID, ItemObject item = null)
    {
        this.SlotID = slotID;
        this.item = item;
    }
}
