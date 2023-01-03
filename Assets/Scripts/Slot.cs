using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    public int SlotID;
    public ItemObject item;
    public int currentStack;

    public Slot(int slotID, ItemObject item = null, int currentStack = 0)
    {
        this.SlotID = slotID;
        this.item = item;
        this.currentStack = currentStack;
    }
}
