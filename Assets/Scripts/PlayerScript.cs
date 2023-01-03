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
    /*public ItemObject draggedItem = null;
    public int draggedFromSlotID;
    public int draggedFromInventoryID;
    public int draggedStack;*/
    public GameObject mousePointer;
    public Sprite mousePointerSprite;
    public RectTransform mousePointerTransform;

    //testing
    public DraggedItem draggedItem;

    void Start()
    {
        inventoryList = new List<Inventory>();
        draggedItem = new DraggedItem();
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
            mousePointer.GetComponent<Image>().sprite = draggedItem.item.itemIcon;
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
        
        if (!draggingItem && draggedItem.item == null)
        {
            if (slot.item != null)
            {
                draggedItem = new DraggedItem(slotID, inventoryID, slot.currentStack, slot.item);
                //draggedItem = slot.item;
                draggingItem = true;
                slot.item = null;
                //draggedFromSlotID = slotID;
                //draggedFromInventoryID = inventoryID;
                //draggedStack = inventoryList[inventoryID].slots[slotID].currentStack;
                DrawInventory(inventory);
            }
        }
        else
        {
            if(slot.item == null)
            {
                slot.item = draggedItem.item;
                slot.currentStack = draggedItem.currentStack;
                //inventory.slotGameObjects[slotID].GetComponent<SlotLogic>().inventoryID = draggedFromInventoryID;
                draggedItem = new DraggedItem();
                draggingItem = false;
                DrawInventory(inventory);
            }
            else
            {
                ItemObject tempItem = slot.item;
                int tempSlot = slot.currentStack;
                Slot draggedFromSlot = inventoryList[draggedItem.inventoryID].slots[draggedItem.slotID];
                slot.item = draggedItem.item;
                slot.currentStack = draggedItem.currentStack;
                draggedFromSlot.item = tempItem;
                draggedFromSlot.currentStack = tempSlot;


                draggedItem = new DraggedItem();
                draggingItem = false;
                DrawInventory(inventory);
                DrawInventory(inventoryList[draggedItem.inventoryID]);
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
       
                draggingItem = true;
                draggedItem = new DraggedItem(slotID, inventoryID, minusAmount, thisItem);
            }
        }
    }
}
public class DraggedItem
{
    public int slotID;
    public int inventoryID;
    public int currentStack;
    public ItemObject item;
    public DraggedItem(int slotID, int inventoryID, int currentStack, ItemObject item)
    {
        this.slotID = slotID;
        this.inventoryID = inventoryID;
        this.currentStack = currentStack;
        this.item = item;
    }

    public DraggedItem() { }
}

