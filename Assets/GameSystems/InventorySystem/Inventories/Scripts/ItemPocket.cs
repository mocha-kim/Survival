using System.Collections;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ItemPocket : MonoBehaviour
{
    // Component
    public InventoryObject inventoryObject;

    // UI
    [SerializeField]
    private GameObject inventoryUIGO;
    [SerializeField]
    private GameObject itemPocketUIGO;
    [SerializeField]
    private ItemPocketUI itemPocketUI;

    private int destroyTime = 180;
    private bool isPlayerEnter = false;
    private bool isInventoryOpen = false;
    private bool isItemPocketOpen = false;

    private void Start()
    {
        inventoryUIGO = InterfaceManager.Instance.inventoryUI;
        itemPocketUIGO = InterfaceManager.Instance.itemPocketUI;
        itemPocketUI = itemPocketUIGO.GetComponent<ItemPocketUI>();

        StartCoroutine(DestroyItemPocket(destroyTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerEnter = false;

        InterfaceManager.Instance.activeUIs.Remove(inventoryUIGO);
        InterfaceManager.Instance.activeUIs.Remove(itemPocketUIGO);
        inventoryUIGO.SetActive(false);
        itemPocketUIGO.SetActive(false);
    }

    private void Update()
    {
        isInventoryOpen = inventoryUIGO.activeSelf;
        isItemPocketOpen = itemPocketUIGO.activeSelf;

        if (!isItemPocketOpen && inventoryObject.EmptySlotCount == inventoryObject.Slots.Length)
        {
            StartCoroutine(DestroyItemPocket(0f));
        }

        if (isPlayerEnter && Input.GetKeyDown(KeyCode.F))
        {
            itemPocketUI.SetInventoryObject(inventoryObject);

            if (isInventoryOpen && isItemPocketOpen)
            {
                InterfaceManager.Instance.activeUIs.Remove(inventoryUIGO);
                InterfaceManager.Instance.activeUIs.Remove(itemPocketUIGO);

                inventoryUIGO.SetActive(false);
                itemPocketUIGO.SetActive(false);
            }

            if (!isInventoryOpen)
            {
                inventoryUIGO.SetActive(true);
                InterfaceManager.Instance.activeUIs.Add(inventoryUIGO);

                inventoryUIGO.transform.SetAsLastSibling();
            }

            if (!isItemPocketOpen)
            {
                itemPocketUIGO.SetActive(true);
                InterfaceManager.Instance.activeUIs.Add(itemPocketUIGO);

                itemPocketUIGO.transform.SetAsLastSibling();
            }
        }
    }

    public void SetInventoryObject(InventoryObject inventory)
    {
        inventoryObject = inventory;
    }

    private IEnumerator DestroyItemPocket(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
        ItemPocketGenerator.Instance.DeleteItemPocket(inventoryObject);
    }
}
