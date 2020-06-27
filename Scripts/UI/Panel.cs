using UnityEngine;

namespace Bale007.UI
{
    public abstract class Panel : MonoBehaviour
    {
        protected GameObject container;
        
        protected virtual void Awake()
        {        
            container = transform.Find("Container").gameObject;

            PanelAwake();
        }
    
        protected virtual void OnEnable()
        {
            UIManager.Instance.RegisterPanel(this);
        }

        protected virtual void OnDisable()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.DeRegisterPanel(this);
            }
        }

        public virtual void OpenPanel(params object[] param)
        {
            container.SetActive(true);
        
            OnOpened(param);
        }

        public virtual void ClosePanel()
        {
            container.SetActive(false);

            OnClosed();
        }
    
        protected virtual void PanelAwake()
        {

        }

        public virtual void ProcessClick(string clickName)
        {

        }

        protected virtual void OnOpened(params object[] param)
        {

        }

        protected virtual void OnClosed()
        {

        }
    }
}
