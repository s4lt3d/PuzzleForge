﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleForge
{
    /// <summary>
    /// The PFEventReactor class is a puzzle forge reactor that reacts to events based on specific conditions.
    /// </summary>
    public class PFEventReactor : PFReactorBase
    {
        public ReactorType reactorType;
        
        [Min(0)]
        public float ActivationDelay;
 
        [Min(0)]
        public float DeactivationDelay;

        [Min(1)]
        public int ActivationSignalCount = 1;
        
        [Min(0)]
        public int DeactivationSignalCount = 0;
        
        
        [SerializeField]
        private UnityEvent onActivated;

        [SerializeField]
        private UnityEvent onDeactivated;
        
        [SerializeField]
        private UnityEvent onReset;

        private bool hasFired;

        private bool state;
        private bool prevState;

        public HashSet<PFSignalEventBase> activationCount = new HashSet<PFSignalEventBase>();

        public override void React(bool ingress, PFSignalEventBase activator)
        {
            if (!enabled)
                return;
            if(activator != null)
                if (ingress)
                    activationCount.Add(activator);
                else
                    activationCount.Remove(activator);

            if (ingress && activationCount.Count < ActivationSignalCount)
                return; 
            
            if (!ingress && activationCount.Count > DeactivationSignalCount)
                return; 

            if (reactorType == ReactorType.Latching)
            {
                if (hasFired) return;
                hasFired = true;
            }

            SetNextState(ingress);

            HandleState(state);
        }

        protected override void HandleState(bool state)
        {
            if(prevState == state)
                return;
            prevState = state;
            
            if (state)
                StartCoroutine(ActivateCR());
            else
                StartCoroutine(DeactivateCR());
        }

        protected void SetNextState(bool ingress)
        {
            if (reactorType == ReactorType.Toggle)
                state = !state;
            else
                state = reactorMode == ReactorMode.Normal ? ingress : !ingress;
        }

        private IEnumerator ActivateCR()
        {
            if (ActivationDelay > 0)
                yield return new WaitForSeconds(ActivationDelay);

            onActivated?.Invoke();
        }

        private IEnumerator DeactivateCR()
        {
            if (DeactivationDelay > 0)
                yield return new WaitForSeconds(DeactivationDelay);

            onDeactivated?.Invoke();
        }
        
        public override void Reset()
        {
            if (!enabled)
                return;
            hasFired = false;
            state = false;
            activationCount.Clear();
            HandleState(state);
            try
            {
                onReset?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}