using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : MonoBehaviour
{
    public Inventory craftingInventory;
    public List<int[]> database = new List<int[]>();
    public bool problem;
    public int[] craftingRecipe = new int[9]; 
    void Start()
    {
        database.Add(new int[9] { 4, 4, 4,  -1, 5, -1,  -1, 5, -1 });
        //int[,] array2D = new int[,] { { 1, 2, 3 }, { 3, 4, 3 }, { 5, 6, 6 }, { 7, 8, 9 } };
        craftingInventory = gameObject.GetComponent<PlayerScript>().craftingInventory;
        Debug.Log(database[0][0]);
    }
    void Update()
    {
        for (int i = 0; i < craftingInventory.slots.Count - 1; i++)
        {
            if (craftingInventory.slots[i].item != null)
            {
                craftingRecipe[i] = craftingInventory.slots[i].item.itemID;
            }
            else
            {
                craftingRecipe[i] = -1;
            }
        }
        //int[,] currentRecipe = new int[3,3] { { craftingInventory.slots[0].item.itemID, craftingInventory.slots[1].item.itemID, craftingInventory.slots[2].item.itemID }, { craftingInventory.slots[3].item.itemID, craftingInventory.slots[4].item.itemID, craftingInventory.slots[5].item.itemID }, { craftingInventory.slots[6].item.itemID, craftingInventory.slots[7].item.itemID, craftingInventory.slots[8].item.itemID } };

        if (ArrayEquality(craftingRecipe, database[0]))
        {
            Debug.Log("GOT DAMN DATS A RECIPE BOIIII");
        }
    }

    public bool ArrayEquality(int[] array1, int[] array2)
    {
        for (int i = 0; i < 9; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }
        return true;
    }
}
