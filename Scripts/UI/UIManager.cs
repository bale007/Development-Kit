using System;
using System.Collections;
using System.Collections.Generic;
using Bale007.EventBus;
using Bale007.Singleton;
using UnityEngine;

namespace Bale007.UI
{
    public class UIManager : SingletonBase<UIManager>
    {      
        private Dictionary<Type, UIPanel> registeredPanels = new Dictionary<Type, UIPanel>();
        private Dictionary<Type, UIHud> registeredHuds = new Dictionary<Type, UIHud>(); 
        private List<UIPanel> openedPanels = new List<UIPanel>();
    
        public void RegisterPanel(UIPanel panel)
        {
            registeredPanels.Add(panel.GetType(), panel);
        }
    
        public void DeRegisterPanel(UIPanel panel)
        {
            registeredPanels.Remove(panel.GetType());
        }
    
        public void RegisterHud(UIHud hud)
        {
            registeredHuds.Add(hud.GetType(), hud);
        }
    
        public void DeRegisterHud(UIHud hud)
        {
            registeredHuds.Remove(hud.GetType());
        }
    
        public void OpenPanel<T>(params object[] param) where T: UIPanel
        {
            if(registeredPanels.TryGetValue(typeof(T), out var windowFound))
            {
                if (!openedPanels.Contains(windowFound))
                {
                    openedPanels.Add(windowFound);
    
                    windowFound.OpenPanel(param);

                    HandleOpenPanelChanged();
    
                    return;
                }
            }
    
            Debug.LogError("Opened Panel Failed:" + typeof(T));
        }
    
        public void ClosePanel<T>() where T : UIPanel
        {
            foreach(UIPanel panel in openedPanels)
            {
                if(panel.GetType() == typeof(T))
                {
                    openedPanels.Remove(panel);
    
                    panel.ClosePanel();
                    
                    HandleOpenPanelChanged();
                    
                    return;
                }
            }
    
            Debug.LogError("Close Panel Failed:" + typeof(T));
        }
    
        public void RefreshHud<T>(params object[] param) where T: UIHud
        {
            if (registeredHuds.TryGetValue(typeof(T), out var hudFound))
            {
                hudFound.Refresh(param);
    
                return;
            }
    
            Debug.LogError("Refresh HUD Failed:" + typeof(T));
        }
        
        public void ShowHud<T>(params object[] param) where T: UIHud
        {
            if (registeredHuds.TryGetValue(typeof(T), out var hudFound))
            {
                hudFound.Show(param);
    
                return;
            }
    
            Debug.LogError("Show HUD Failed:" + typeof(T));
        }
        
        public void HideHud<T>() where T: UIHud
        {    
            if (registeredHuds.TryGetValue(typeof(T), out var hudFound))
            {
                hudFound.Hide();
    
                return;
            }
    
            Debug.LogError("Hide HUD Failed:" + typeof(T));
        }

        public bool HasActivePanel
        {
            get { return openedPanels.Count > 0; }
        }

        private void HandleOpenPanelChanged()
        {
            if (HasActivePanel)
            {
                EventBus.EventBus.Publish(EventBusEventType.UINotification_HideHud);
            }
            else
            {
                EventBus.EventBus.Publish(EventBusEventType.UINotification_ShowHud);
            }
        }
    }
}
