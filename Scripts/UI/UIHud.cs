using UnityEngine;

namespace Bale007.UI
{
    public abstract class UIHud : MonoBehaviour
    {
        private void Awake()
        {
            HudAwake();
        }

        private void OnEnable()
        {
            UIManager.Instance.RegisterHud(this);
        }

        private void OnDisable()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.DeRegisterHud(this);
            }
        }

        protected virtual void HudAwake()
        {

        }

        public virtual void Refresh(params object[] param) { }

        public virtual void Hide()
        {
        
        }

        public virtual void Show()
        {
        
        }
    }
}

