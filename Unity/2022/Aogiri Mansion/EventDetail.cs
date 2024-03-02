using UnityEngine;
using UnityEngine.Events;

public class EventDetail : MonoBehaviour
{
    private UnityAction charaEvent;

    public UnityAction CharaEvent
    {
        get
        {
            return charaEvent;
        }
    }

    public void SetUpEvent(UnityAction unityEvent)
    {
        if (unityEvent != null)
        {
            charaEvent = new UnityAction(unityEvent);
        }
    }

    public void TriggerEvent()
    {
        charaEvent?.Invoke();
    }
}