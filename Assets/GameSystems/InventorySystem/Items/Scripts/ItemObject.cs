using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/New Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool isStackable;

    public Sprite icon;
    public Item data = new();

    [TextArea(15, 20)]
    public string description;
}
