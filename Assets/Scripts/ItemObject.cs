using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemObject", order = 1)]
public class ItemObject : ScriptableObject
{
    public int itemID;
    public string itemDescription;
    public string itemName;
    public Sprite itemIcon;
    public int maxStackSize;
}