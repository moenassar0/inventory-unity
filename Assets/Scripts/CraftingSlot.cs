using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : MonoBehaviour
{
    public Inventory craftingInventory;

    private void Start()
    {
        craftingInventory = GameObject.Find("Main Camera").GetComponent<PlayerScript>().craftingInventory;
    }
    void Update()
    {
        
    }
}
