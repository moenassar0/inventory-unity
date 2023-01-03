using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public List<Inventory> inventoryList;
    public ItemObject sword, axe, shield;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;

    //Dragging item logic variables
    public bool draggingItem = false;
    public ItemObject draggedItem = null;
    public int draggedFromSlotID;
    public int draggedFromInventoryID;
    public int draggedStack;
    public GameObject mousePointer;
    public Sprite mousePointerSprite;
    public RectTransform mousePointerTransform;

    //testing
    public Slot draggedSlot;

    void Start()
    {
        inventoryList = new List<Inventory>();

        //Initialize two inventories and add items
        CreateInventory(inventoryList.Count, 20);
        CreateInventory(inventoryList.Count, 20);
        inventoryList[0].AddItem(sword, 1);
        inventoryList[0].AddItem(axe, 2);
        inventoryList[0].AddItem(shield, 1);
        DrawInventory(inventoryList[0]);
        mousePointerSprite = mousePointer.GetComponent<Image>().sprite;
        mousePointerTransform = mousePointer.GetComponent<RectTransform>();
        mousePointer.GetComponent<RectTransform>().SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        if (draggingItem)
        {
            mousePointer.SetActive(true);
            mousePointer.GetComponent<Image>().sprite = draggedItem.itemIcon;
            Vector3 mousePos = Input.mousePosition;
            mousePointer.transform.position = Input.mousePosition;
        }
        else
            mousePointer.SetActive(false);
    }

    void CreateInventory(int inventoryID, int slotCount)
    {
        GameObject instantiatedPanel = Instantiate(inventoryPanel);
        instantiatedPanel.transform.SetParent(GameObject.Find("Canvas").transform);
        instantiatedPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10* inventoryList.Count - (inventoryList.Count * -330), -330);
        Inventory inventory = new Inventory(inventoryID, slotCount);
        inventoryList.Add(inventory);
        List<GameObject> slotGameObjects = new List<GameObject>();
        for (int i = 0; i < slotCount; i++)
        {
            //inventory.slots.Add(new Slot(i, new ItemObject()));
            GameObject instantiatedSlot = Instantiate(slotPrefab);
            SlotLogic slotLogic = instantiatedSlot.GetComponent<SlotLogic>();
            slotLogic.slotID = i;
            slotLogic.inventoryID = inventoryID;
            instantiatedSlot.transform.SetParent(instantiatedPanel.transform);
            slotGameObjects.Add(instantiatedSlot);
        }
        inventory.setSlotGameObjects(slotGameObjects);
    }

    public void DrawInventory(Inventory inventory)
    {
        for (int i = 0; i < inventory.slotCount; i++)
        {
            Image slotImage = inventory.slotGameObjects[i].transform.GetChild(0).GetComponent<Image>();
            Text stackImage = inventory.slotGameObjects[i].transform.GetChild(1).GetComponent<Text>();
            if (inventory.slots[i].item != null)
            {
                slotImage.sprite = inventory.slots[i].item.itemIcon;
                stackImage.text = inventory.slots[i].currentStack.ToString();
            }
            else
            {
                slotImage.sprite = null;
                stackImage.text = "";
            }
        }
    }

    public void ClickedOnSlot(int slotID, int inventoryID)
    {
        Inventory inventory = inventoryList[inventoryID];
        Slot slot = inventory.slots[slotID];
        
        if (!draggingItem && draggedItem == null)
        {
            if (slot.item != null)
            {
                draggedItem = slot.item;
                draggingItem = true;
                slot.item = null;
                draggedFromSlotID = slotID;
                draggedFromInventoryID = inventoryID;
                draggedStack = inventoryList[inventoryID].slots[slotID].currentStack;
                DrawInventory(inventory);
            }
        }
        else
        {
            if(slot.item == null)
            {
                slot.item = draggedItem;
                slot.currentStack = draggedStack;
                //inventory.slotGameObjects[slotID].GetComponent<SlotLogic>().inventoryID = draggedFromInventoryID;
                draggedItem = null;
                draggingItem = false;
                DrawInventory(inventory);
            }
            else
            {
                ItemObject tempItem = slot.item;
                int tempSlot = slot.currentStack;
                Slot draggedFromSlot = inventoryList[draggedFromInventoryID].slots[draggedFromSlotID];
                slot.item = draggedItem;
                slot.currentStack = draggedStack;
                draggedFromSlot.item = tempItem;
                draggedFromSlot.currentStack = tempSlot;


                draggedItem = null;
                draggingItem = false;
                DrawInventory(inventory);
                DrawInventory(inventoryList[draggedFromInventoryID]);
            }
        }
    }
    public void RightClickedOnSlot(int slotID, int inventoryID)
    {
        if(inventoryList[inventoryID].slots[slotID].item != null)
        {
            //split stack
            ItemObject thisItem = inventoryList[inventoryID].slots[slotID].item;
            if(inventoryList[inventoryID].slots[slotID].currentStack > 1)
            {
                int minusAmount = inventoryList[inventoryID].slots[slotID].currentStack / 2;
                inventoryList[inventoryID].slots[slotID].currentStack -= minusAmount;
                draggedItem = thisItem;
                draggingItem = true;
                draggedFromSlotID = slotID;
                draggedFromInventoryID = inventoryID;
                draggedStack = minusAmount;
            }
        }
    }
}