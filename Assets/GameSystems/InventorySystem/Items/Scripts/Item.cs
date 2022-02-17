using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public string name;

    public ItemEffect[] effects;

    public Item()
    {
        id = -1;
        name = "";
    }
}
