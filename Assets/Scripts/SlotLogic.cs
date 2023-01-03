using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotLogic : MonoBehaviour, IPointerClickHandler
{
    public int slotID;
    public int inventoryID;
    public PlayerScript playerScript;
    public Slot thisSlot;
    public Inventory thisInventory;

    void Start()
    {
        playerScript = GameObject.Find("Main Camera").GetComponent<PlayerScript>();
        //thisInventory = playerScript.inventoryList[inventoryID];
        //thisSlot = thisInventory.slots[slotID];
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        /*Debug.Log("Click DETECTED ON SLOT ID: " + slotID);
        if (thisSlot.item == null)
        {
            thisInventory.AddItem(playerScript.sword);
            playerScript.DrawInventory(thisInventory);
        }*/

        if (eventData.pointerId == -2)
            playerScript.RightClickedOnSlot(slotID, inventoryID);
        else
            playerScript.ClickedOnSlot(slotID, inventoryID);
    }
}