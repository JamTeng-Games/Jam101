using Jam.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jam.Runtime.UI_
{

    public static class UIUtil
    {
        public static void AddEventListener(this UIBehaviour control,
                                            EventTriggerType eventType,
                                            System.Action<BaseEventData> action)
        {
            EventTrigger trigger = control.gameObject.GetOrAddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener((data) => action(data));
            trigger.triggers.Add(entry);
        }

        public static void RemoveAllEventListeners(this UIBehaviour control)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.triggers.Clear();
            }
        }

        public static void DisableEventListeners(this UIBehaviour control)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.enabled = false;
            }
        }

        public static void RemoveEventListenerComp(this UIBehaviour control)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                Object.Destroy(trigger);
            }
        }
    }

}