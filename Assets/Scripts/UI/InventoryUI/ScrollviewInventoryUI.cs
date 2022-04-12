using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollviewInventoryUI : DynamicInventoryUI, IVerticalScrollable
{
    [SerializeField]
    private GameObject viewport;
    [SerializeField]
    private Scrollbar scroll;

    protected float descriptionLength;

    protected override void OnEnable()
    {
        base.OnEnable();

        descriptionLength = description.GetComponent<RectTransform>().sizeDelta.y;

        ResizeContent();
        ResizeViewport();
    }

    public void ResizeViewport()
    {
        float viewportLength = 2 * Mathf.Abs(start.y) + (inventoryObject.Slots.Length / colNum) * (slotSize + space.y) - space.y;
        float tempLength = isDescriptionOpened ? viewportLength - (descriptionLength + 2 * Mathf.Abs(start.y)) : viewportLength;

        viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(0, tempLength);
    }

    public void ResizeContent()
    {
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
