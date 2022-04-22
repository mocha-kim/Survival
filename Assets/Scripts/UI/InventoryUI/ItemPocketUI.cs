using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPocketUI : DynamicInventoryUI
{
    private static ItemPocketUI instance;
    public static ItemPocketUI Instance => instance;

    // Component
    public GameObject player;
    public GameObject pocket;
    public GameObject inventoryUI;
    public GameObject itemPocketUI;
    public GameObject scrollView;

    protected float descriptionLength;
    private float distance;
    private float viewLength;

    protected override void OnEnable()
    {
        instance = this;
        base.OnEnable();

        descriptionLength = description.GetComponent<RectTransform>().sizeDelta.y;

        ResizeContent();
    }

    public void ResizeContent()
    {
        // calc lengthes to resize length area
        viewLength = Mathf.Abs(start.y) + (inventoryObject.Slots.Length / colNum) * (slotSize + space.y) + 2 * space.y;
    }

    protected override void EnableDescription()
    {
        base.EnableDescription();
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, viewLength + descriptionLength + space.y);
        scrollView.GetComponent<RectTransform>().position = new Vector2(transform.position.x, transform.position.y);
    }

    protected override void DisableDescription()
    {
        base.DisableDescription();
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, viewLength);
        scrollView.GetComponent<RectTransform>().position = new Vector2(transform.position.x, transform.position.y + 23.2812f);
    }

    public void TogglePocketUI()
    {
        distance = Vector3.Distance(pocket.transform.position, player.transform.position);
        if (distance <= 2.5f)
        {
            InterfaceManager.Instance.ToggleUI(inventoryUI);
            InterfaceManager.Instance.ToggleUI(itemPocketUI);
        }
    }
}
