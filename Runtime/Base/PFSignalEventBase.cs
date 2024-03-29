using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    /// <summary>
    ///     Represents the type of trigger event.
    /// </summary>
    public enum TriggerType
    {
        Simple,
        Latching,
        Toggle
    }

    /// <summary>
    ///     Enum representing the trigger mode.
    /// </summary>
    public enum TriggerMode
    {
        Normal,
        Inverted
    }

    /// <summary>
    ///     Enumeration of trigger interaction options.
    /// </summary>
    public enum TriggerInteractions
    {
        ActivationAndDeactivation,
        ActivationOnly,
        DeactivationOnly
    }

    /// <summary>
    ///     Represents a signal event in the PuzzleForge game.
    /// </summary>
    public class PFSignalEventBase : PFSignalBase
    {
        public List<string> interactionTags = new() { "Player" };
        
        public TriggerType triggerType;

        public TriggerInteractions triggerInteractions;
        
        [Range(0, 10)]
        public int activationTagCount = 1;

        [Range(0, 10)]
        public int deactivateTagCount;



        [HideInInspector]
        public List<PFEventReactor> activationReactors;

        [HideInInspector]
        public List<PFEventReactor> deactivationReactors;

        private bool hasFired;

        private bool state;
        
        protected List<string> tagsActive = new();
        
        public List<PFReactorBase> OnActivationActivateHookups = new();
        public List<PFReactorBase> OnActivationDeactivateHookups = new();
        public List<PFReactorBase> OnActivationResetHookups = new();
        public List<PFReactorBase> OnDeactivationActivateHookups = new();
        public List<PFReactorBase> OnDeactivationDeactivateHookups = new();
        public List<PFReactorBase> OnDeactivationResetHookups = new();


        /// <summary>
        ///     Sends a signal to activate or deactivate the component based on the given parameters.
        /// </summary>
        /// <param name="component">The component to send the signal to.</param>
        /// <param name="isActive">A boolean value indicating whether to activate or deactivate the component.</param>
        protected void SendSignal(Component component, bool isActive)
        {
            if(!enabled)
                return;
            
            var pfBaseComponent = component.GetComponent<PFBase>();
            var componentTag = component.tag;

            if (!(interactionTags.Contains(componentTag) || pfBaseComponent.ObjectTags.Contains(componentTag)))
                return;

            if (isActive)
                HandleTriggerInteractions(component, activationTagCount, TriggerInteractions.DeactivationOnly);
            else
                HandleTriggerInteractions(component, deactivateTagCount, TriggerInteractions.ActivationOnly);

            if (triggerType == TriggerType.Latching && hasFired) return;
            hasFired = true;

            SetNextState(isActive);

            if (state && triggerInteractions != TriggerInteractions.DeactivationOnly)
                DispatchSignal(true);

            if (!state && triggerInteractions != TriggerInteractions.ActivationOnly)
                DispatchSignal(false);
        }

        private void DispatchSignal(bool ingress)
        {
            var hookups = ingress ? OnActivationActivateHookups : OnDeactivationDeactivateHookups;
            foreach (var reactor in hookups)
                reactor.React(ingress, this);

            hookups = !ingress ? OnActivationDeactivateHookups : OnDeactivationActivateHookups;
            foreach (var reactor in hookups)
                reactor.React(ingress, this);
            
            
            hookups = ingress ? OnActivationResetHookups : OnDeactivationResetHookups;
            foreach (var reactor in hookups)
                reactor.Reset();
        }

        /// <summary>
        ///     Handles the trigger interactions of a component.
        /// </summary>
        /// <param name="component">The component being triggered.</param>
        /// <param name="tagCount">The number of tags required for the interaction to take place.</param>
        /// <param name="interactions">Specifies the type of interactions to be handled.</param>
        protected void HandleTriggerInteractions(Component component, int tagCount, TriggerInteractions interactions)
        {
            if (component != null)
            {
                tagsActive.Add(component.tag);
                if (tagsActive.Count < tagCount)
                    return;
            }

            if (triggerInteractions == interactions)
                return;
        }

        /// <summary>
        ///     Sets the next state of the PFSignalEvent.
        /// </summary>
        /// <param name="isActive">Indicates whether the event is active or not.</param>
        protected void SetNextState(bool isActive)
        {
            state = triggerType == TriggerType.Toggle
                ? !state
                : triggerMode == TriggerMode.Normal
                    ? isActive
                    : !isActive;
        }
        
        
        public override void Reset()
        {
            if(!enabled)
                return; 
            hasFired = false;
            state = false;
            tagsActive.Clear();
        }

        public bool Contains(PFReactorBase reactor)
        {
            if(OnActivationActivateHookups.Contains(reactor)) return true;
            if(OnActivationDeactivateHookups.Contains(reactor)) return true;
            if(OnActivationResetHookups.Contains(reactor)) return true;
            if(OnDeactivationActivateHookups.Contains(reactor)) return true;
            if(OnDeactivationDeactivateHookups.Contains(reactor)) return true;
            if(OnDeactivationResetHookups.Contains(reactor)) return true;
            return false;
        }
        
        public List<PFReactorBase> GetAllReactors()
        {
            var list = new List<PFReactorBase>();
            list.AddRange(OnActivationActivateHookups);
            list.AddRange(OnActivationDeactivateHookups);
            list.AddRange(OnActivationResetHookups);
            list.AddRange(OnDeactivationActivateHookups);
            list.AddRange(OnDeactivationDeactivateHookups);
            list.AddRange(OnDeactivationResetHookups);
            return list;
        }
    }
}