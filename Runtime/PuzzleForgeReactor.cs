using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleForge
{
    public abstract class ReactorBase : MonoBehaviour
    {
        public abstract void React(bool state);
    } 
    
    public enum ReactorType
    {
        Simple, 
        Latching, 
        Toggle
    }

    public enum ReactorMode
    {
        Normal,
        Inverted 
    }
    
    public class PuzzleForgeReactor : ReactorBase
    {
        public ReactorType reactorType;
        public ReactorMode reactorMode;
        
        [Min(0)]
        public float ActivationDelay = 0.0f;
        [Min(0)]
        public float DeactivationDelay = 0.0f;
        

        [SerializeField]
        private UnityEvent onActivated;

        [SerializeField]
        private UnityEvent onDeactivated;
        
        private bool state;
        
        bool hasFired = false;
        
        public override void React(bool ingress)
        {
            if (reactorType == ReactorType.Latching)
            {
                if (hasFired) return;
                hasFired = true;
            }
			
            SetNextState(ingress);
            
            if (state) 
                StartCoroutine(ActivateCR());
            else 
                StartCoroutine(DeactivateCR());
        }
        
        protected void SetNextState(bool ingress)
        {
            if (reactorType == ReactorType.Toggle)
            {
                state = !state;
            }
            else
            {
                state = reactorMode == ReactorMode.Normal ? ingress : !ingress;
            }
        }
        
        IEnumerator ActivateCR()
        {
            if (ActivationDelay > 0)
                yield return new WaitForSeconds(ActivationDelay);

            onActivated.Invoke();

            yield break;
        }

        IEnumerator DeactivateCR()
        {
            if (DeactivationDelay > 0)
                yield return new WaitForSeconds(DeactivationDelay);
           
            onDeactivated.Invoke();

            yield break;
        }
    }
}