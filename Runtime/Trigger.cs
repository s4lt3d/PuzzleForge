using System;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{

    public enum TriggerMode
    {
        Simple, 
        Latching, 
        Toggle
    }

    public enum TriggerType
    {
        Normal,
        Inverted 
    }

    public enum TriggerInteractions
    {
        ActivateAndDeactivate,
        ActivateOnly,
        DeactivateOnly
    }
    
    public class Trigger : MonoBehaviour
    {
        public List<Reactor> connectedReactors;
        public TriggerMode triggerMode;
        public TriggerType triggerType;
        private bool currentState = false; // true is active, false is inactive

        private void OnTriggerEnter(Collider other)
        {
            Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            Deactivate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Activate();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Deactivate();
        }

        public void Activate()
        {
            if (triggerMode == TriggerMode.Latching && currentState) return; // Prevent reactivation if latched
            currentState = true;
            SendSignal(true);
        }

        public void Deactivate()
        {
            if (triggerMode == TriggerMode.Latching && !currentState) return; // Prevent deactivation if latched
            currentState = false;
            SendSignal(false);
        }

        public void Toggle()
        {
            currentState = !currentState;
            SendSignal(currentState);
        }

        private void SendSignal(bool state)
        {
            if (triggerType == TriggerType.Inverted) state = !state;
            foreach (var reactor in connectedReactors)
            {
                reactor.React(state);
            }
            if (triggerMode == TriggerMode.Latching) this.enabled = false; // Optionally disable trigger if latched
        }

        // Method to connect to reactors, ensuring they can be dynamically added
        public void ConnectReactor(Reactor reactor)
        {
            if (!connectedReactors.Contains(reactor))
            {
                connectedReactors.Add(reactor);
            }
        }
    }
}