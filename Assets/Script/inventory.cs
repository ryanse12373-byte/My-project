using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int inventorySize = 10;
    public List<Item> items = new List<Item>(); 

    public void addItem(Item item)
    {
        if(items.Count >= inventorySize) return;
        items.Add(item);
    }
    public void removeItem(Item item)
    {
        if(!items.Contains(item)) return;
        items.Remove(item);
    }

    public Item equipedItem;

    public EquipementSlot[] equipementSlots;

    void Update()
    {
        
    }
}


