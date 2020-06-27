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
        private readonly Dictionary<Type, Panel> registeredPanels = new Dictionary<Type, Panel>();
        private readonly Dictionary<Type, HUD> registeredHUDs = new Dictionary<Type, HUD>(); 
        private List<Panel> openedPanels = new List<Panel>();
    
        public void RegisterPanel(Panel panel)
        {
            registeredPanels.Add(panel.GetType(), panel);
        }
    
        public void DeRegisterPanel(Panel panel)
        {
            registeredPanels.Remove(panel.GetType());
        }
    
        public void RegisterHud(HUD hud)
        {
            registeredHUDs.Add(hud.GetType(), hud);
        }
    
        public void DeRegisterHud(HUD hud)
        {
            registeredHUDs.Remove(hud.GetType());
        }
    
        public void OpenPanel<T>(params object[] param) where T: Panel
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
    
        public void ClosePanel<T>() where T : Panel
        {
            foreach(Panel panel in openedPanels)
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
    
        public void RefreshHud<T>(params object[] param) where T: HUD
        {
            if (registeredHUDs.TryGetValue(typeof(T), out var hudFound))
            {
                hudFound.Refresh(param);
    
                return;
            }
    
            Debug.LogError("Refresh HUD Failed:" + typeof(T));
        }
        
        public void ShowHud<T>(params object[] param) where T: HUD
        {
            if (registeredHUDs.TryGetValue(typeof(T), out var hudFound))
            {
                hudFound.Show(param);
    
                return;
            }
    
            Debug.LogError("Show HUD Failed:" + typeof(T));
        }
        
        public void HideHud<T>() where T: HUD
        {    
            if (registeredHUDs.TryGetValue(typeof(T), out var hudFound))
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
