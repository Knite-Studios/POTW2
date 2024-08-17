using System;
using UnityEngine;

namespace MiniGame
{
    /// <summary>
    /// UI element used in the hype mini-game to trigger the hype.
    /// </summary>
    public class HypeTrigger : MonoBehaviour
    {
        public Action OnHypeTriggerDestroyed;
        
        private void OnDestroy()
        {
            // Invoke the event first.
            OnHypeTriggerDestroyed?.Invoke();
            
            // Unsubscribe to all handlers.
            foreach (var d in OnHypeTriggerDestroyed!.GetInvocationList())
            {
                OnHypeTriggerDestroyed -= (Action) d;
            }
        }
    }
}