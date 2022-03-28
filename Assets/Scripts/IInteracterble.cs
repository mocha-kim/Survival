using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    float Distance
    {
        get;
    }

    void Interact(PlayerController player);
    void StopInteract(PlayerController player);
}