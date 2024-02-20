using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleForge
{
    public class PuzzleForgeEventReactor : PuzzleForgeReactorBase
    {
        [Min(0)]
        public float ActivationDelay;

        [Min(0)]
        public float DeactivationDelay;

        [SerializeField]
        private UnityEvent onActivated;

        [SerializeField]
        private UnityEvent onDeactivated;

        private bool hasFired;

        private bool state;

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
                state = !state;
            else
                state = reactorMode == ReactorMode.Normal ? ingress : !ingress;
        }

        private IEnumerator ActivateCR()
        {
            if (ActivationDelay > 0)
                yield return new WaitForSeconds(ActivationDelay);

            onActivated.Invoke();
        }

        private IEnumerator DeactivateCR()
        {
            if (DeactivationDelay > 0)
                yield return new WaitForSeconds(DeactivationDelay);

            onDeactivated.Invoke();
        }
    }
}