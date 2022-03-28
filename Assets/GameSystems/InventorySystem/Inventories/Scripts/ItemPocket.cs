using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPocket : MonoBehaviour
{
    // Component
    public GameObject inventoryUI;
    public GameObject itemPocketUI;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            InterfaceManager.Instance.ToggleUI(inventoryUI);
            InterfaceManager.Instance.ToggleUI(itemPocketUI);
        }
    }

    public void OnTriggerExit()
    {
        InterfaceManager.Instance.activeUIs.Remove(inventoryUI);
        inventoryUI.SetActive(false);

        InterfaceManager.Instance.activeUIs.Remove(itemPocketUI);
        itemPocketUI.SetActive(false);
    }

    public void Interact(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void StopInteract(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}