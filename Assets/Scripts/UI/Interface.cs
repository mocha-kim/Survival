using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Interface : MonoBehaviour
{
    public GameObject dragArea;

    private Vector3 interfaceStart;
    private Vector3 mouseStart;

    protected virtual void Awake()
    {
        // interface related events
        AddEvent(gameObject, EventTriggerType.PointerClick, delegate { OnClickInterface(); });

        // interface window drag related events
        if (dragArea)
        {
            AddEvent(dragArea, EventTriggerType.BeginDrag, delegate { OnStartDrag(); });
            AddEvent(dragArea, EventTriggerType.Drag, delegate { OnDrag(); });
        }
    }

    protected virtual void AddEvent(GameObject go, EventTriggerType triggerType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger)
        {
            Debug.LogWarning(go.name + "has no EventTrigger component found");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = triggerType };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnterInterface()
    {
        MouseData.mouseHoveredInterface = gameObject;
    }

    public void OnExitInterface()
    {
        MouseData.mouseHoveredInterface = null;
    }

    public void OnClickInterface()
    {
        InterfaceManager.Instance.OnFocus(gameObject);
    }

    public void OnStartDrag()
    {
        interfaceStart = gameObject.GetComponent<RectTransform>().position;
        mouseStart = Input.mousePosition;
    }

    public void OnDrag()
    {
        gameObject.GetComponent<RectTransform>().position = interfaceStart + Input.mousePosition - mouseStart;
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
