using Bale007.EventBus;
using UnityEngine;

namespace Bale007.UI
{
    public abstract class HUD : MonoBehaviour
    {
        protected virtual void Awake()
        {

        }

        protected virtual void OnEnable()
        {
            EventBus.EventBus.Subscribe(EventBusEventType.UINotification_ShowHud, Show);
            EventBus.EventBus.Subscribe(EventBusEventType.UINotification_HideHud, Hide);
            
            UIManager.Instance.RegisterHud(this);
        }

        protected virtual  void OnDisable()
        {
            EventBus.EventBus.UnSubscribe(EventBusEventType.UINotification_ShowHud, Show);
            EventBus.EventBus.UnSubscribe(EventBusEventType.UINotification_HideHud, Hide);
            
            UIManager.Instance?.DeRegisterHud(this);
        }

        public virtual void Refresh(params object[] param)
        {
            
        }

        public virtual void Hide(params object[] param)
        {
            transform.localScale = Vector3.zero;
        }

        public virtual void Show(params object[] param)
        {
            transform.localScale = Vector3.one;
        }
    }
}

