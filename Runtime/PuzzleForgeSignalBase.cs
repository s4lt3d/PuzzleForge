using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    public enum TriggerType
    {
        Simple,
        Latching,
        Toggle
    }

    public enum TriggerMode
    {
        Normal,
        Inverted
    }

    public enum TriggerInteractions
    {
        ActivationAndDeactivation,
        ActivationOnly,
        DeactivationOnly
    }

    public class PuzzleForgeSignalBase : PuzzleForgeBase
    {
        public TriggerType triggerType;
        public TriggerMode triggerMode;
        public TriggerInteractions triggerInteractions;

        [Range(0, 10)]
        public int activationTagCount = 1;

        [Range(0, 10)]
        public int deactivateTagCount;

        public List<string> interactionTags = new() { "Player" };

        [HideInInspector]
        public List<PuzzleForgeEventReactor> activationReactors;

        [HideInInspector]
        public List<PuzzleForgeEventReactor> deactivationReactors;

        public bool mouseClickDebug;
        private bool hasFired;
        private bool mouseState;

        private bool state;

        protected List<string> tagsActive = new();

        private void OnMouseDown()
        {
            if (mouseClickDebug)
            {
                mouseState = !mouseState;
                if (mouseState)
                {
                    Debug.Log("Debug Activate Event");
                    SendSignal(null, true);
                }
                else
                {
                    DebugActivate();
                }
            }
        }

        protected void SendSignal(Component component, bool ingress)
        {
            if (!interactionTags.Contains(component.tag))
                return;

            if (ingress)
            {
                if (component != null)
                {
                    tagsActive.Add(component.tag);
                    if (tagsActive.Count < activationTagCount)
                        return;
                }

                if (triggerInteractions == TriggerInteractions.DeactivationOnly)
                    return;
            }
            else
            {
                if (component != null)
                {
                    tagsActive.Remove(component.tag);
                    if (tagsActive.Count > deactivateTagCount)
                        return;
                }

                if (triggerInteractions == TriggerInteractions.ActivationOnly)
                    return;
            }

            if (triggerType == TriggerType.Latching)
            {
                if (hasFired) return;
                hasFired = true;
            }

            SetNextState(ingress);

            if (state && triggerInteractions != TriggerInteractions.DeactivationOnly)
                parentController.SendSignal(this, true);

            if (state == false && triggerInteractions != TriggerInteractions.ActivationOnly)
                parentController.SendSignal(this, false);
        }

        protected void SetNextState(bool ingress)
        {
            if (triggerType == TriggerType.Toggle)
                state = !state;
            else
                state = triggerMode == TriggerMode.Normal ? ingress : !ingress;
        }

        public void DebugActivate()
        {
            Debug.Log("Debug Activate Event");
            SendSignal(null, true);
        }

        public void DebugDeactivate()
        {
            Debug.Log("Debug Deactivate Event");
            SendSignal(null, false);
        }
    }
}