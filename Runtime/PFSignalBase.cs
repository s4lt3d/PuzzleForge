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

    public class PFSignalBase : PFBase
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
    public List<PFEventReactor> activationReactors;
    [HideInInspector]
    public List<PFEventReactor> deactivationReactors;
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

    protected void SendSignal(Component component, bool isActive)
    {
        // Included objectTags in the check
        if (!interactionTags.Contains(component.tag) && !objectTags.Contains(component.tag)) 
            return;

        if (isActive)
            HandleTriggerInteractions(component, activationTagCount, TriggerInteractions.DeactivationOnly);
        else
            HandleTriggerInteractions(component, deactivateTagCount, TriggerInteractions.ActivationOnly);

        if (triggerType == TriggerType.Latching && hasFired) return;
        hasFired = true;

        SetNextState(isActive);

        if (state && triggerInteractions != TriggerInteractions.DeactivationOnly)
            parentController.DispatchSignal(this, true);

        if (!state && triggerInteractions != TriggerInteractions.ActivationOnly)
            parentController.DispatchSignal(this, false);
    }

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

    protected void SetNextState(bool isActive)
    {
        state = triggerType == TriggerType.Toggle 
            ? !state 
            : triggerMode == TriggerMode.Normal ? isActive : !isActive;
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