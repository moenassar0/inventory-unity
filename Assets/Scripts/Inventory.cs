using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public int inventoryID;
    public int slotCount;
    [SerializeField] public List<Slot> slots = new List<Slot>();
    [SerializeField] public List<GameObject> slotGameObjects = new List<GameObject>();
    
    public Inventory(int inventoryID, int slotCount)
    {
        this.inventoryID = inventoryID;
        this.slotCount = slotCount;
        this.slotGameObjects = null;
        for(int x = 0; x < slotCount; x++)
        {
            this.slots.Add(new Slot(x));
        }
    }

    public void setSlotGameObjects(List<GameObject> slotGameObjects)
    {
        this.slotGameObjects = slotGameObjects;
    }

    public void AddItem(ItemObject item, int stack)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                if(stack > item.maxStackSize)
                {
                    int newStack = stack - item.maxStackSize;
                    slots[i].currentStack = item.maxStackSize;
                    AddItem(item, newStack);
                }
                else
                    slots[i].currentStack = stack;
                break;
            }
        }
    }
}
