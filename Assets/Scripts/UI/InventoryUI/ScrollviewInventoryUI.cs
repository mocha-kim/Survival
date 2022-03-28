using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollviewInventoryUI : DynamicInventoryUI
{
    [SerializeField]
    private GameObject viewport;
    [SerializeField]
    private ScrollRect scroll;

    protected float descriptionLength;

    protected override void OnEnable()
    {
        base.OnEnable();

        descriptionLength = description.GetComponent<RectTransform>().sizeDelta.y;

        ResizeContent();
        ResizeViewport();
    }

    private void ResizeViewport()
    {
        // calc lengthes to resize viewport area
        float viewportLength = 2 * Mathf.Abs(start.y) + (inventoryObject.Slots.Length / colNum) * (slotSize + space.y) - space.y;
        float tempLength = isDescriptionOpened ? viewportLength - (descriptionLength + 2 * Mathf.Abs(start.y)) : viewportLength;

        viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(0, tempLength);
    }

    private void ResizeContent()
    {
        // calc lengthes to resize content area
        float contentLength = Mathf.Abs(start.y) + (inventoryObject.Slots.Length / colNum) * (slotSize + space.y) - space.y;

        slotParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentLength);
    }

    protected override void EnableDescription()
    {
        base.EnableDescription();
        ResizeViewport();
    }

    protected override void DisableDescription()
    {
        base.DisableDescription();
        ResizeViewport();
    }
}
