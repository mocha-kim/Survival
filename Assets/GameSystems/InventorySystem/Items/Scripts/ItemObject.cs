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

    public string EffectsToString()
    {
        string result = "";

        foreach (ItemEffect effect in data.effects)
        {
            switch (effect.type)
            {
                case EffectType.Status:
                    result += effect.statusType.ToString() + " ";
                    result += effect.value >= 0 ? "+" : "-";
                    result += effect.value.ToString("n0");
                    break;
                case EffectType.Attribute:
                    result += effect.attributeType.ToString() + " ";
                    result += effect.value >= 0 ? "+" : "-";
                    result += effect.value.ToString("n0");
                    break;
                case EffectType.Condition:
                    result += effect.conditionType.ToString() + " " + effect.value.ToString() + "s";
                    break;
            }
            result += "\n";
        }
        result.Remove(result.Length - 1);

        return result;
    }
}
