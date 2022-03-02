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

    protected override void Start()
    {
        descriptionLength = description.GetComponent<RectTransform>().sizeDelta.y;

        ResizeContent();
        ResizeViewport();
        base.Start();
    }

    private void ResizeViewport()
    {
        float temp = scroll.verticalNormalizedPosition;
        // calc lengthes to resize content area
        float viewportLength = 2 * Mathf.Abs(start.y) + (inventoryObject.Slots.Length / colNum) * (slotSize + space.y) - space.y;
        float tempLength = isDescriptionOpened ? viewportLength - (descriptionLength + 2 * Mathf.Abs(start.y)) : viewportLength;

        viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(0, tempLength);
        scroll.verticalNormalizedPosition = temp;
    }

    public void ResizeContent()
    {
        // calc lengthes to resize content area
        float contentLength = Mathf.Abs(start.y) + (inventoryObject.Slots.Length / colNum) * (slotSize + space.y) - space.y;

        slotParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentLength);
    }

    public override void EnableDescription()
    {
        base.EnableDescription();
        ResizeViewport();
    }

    public override void DisableDescription()
    {
        base.DisableDescription();
        ResizeViewport();
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
