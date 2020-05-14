namespace Bale007.EventBus
{
    public enum EventBusEventType
    {
        None = 0,
        
        UINotification_ShowHud,
        UINotification_HideHud,
        
        GameNotification_GameLoaded,
        
        GeneralNotification_ItemInteraction,
        
        RoomNotification_OnRoomChanged,
        
        DebugNotification_ShowDebugUI,
        DebugNotification_HideDebugUI
    }
}