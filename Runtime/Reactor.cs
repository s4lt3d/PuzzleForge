using UnityEngine;
using UnityEngine.Events;

namespace PuzzleForge
{

    public abstract class ReactorBase : MonoBehaviour
    {
        public abstract void React(bool state);
    }
    
    public class Reactor : ReactorBase
    {
        [SerializeField]
        private UnityEvent onActivated;

        [SerializeField]
        private UnityEvent onDeactivated;
        
        public override void React(bool state)
        {
            if (state) 
                onActivated.Invoke();
            else 
                onDeactivated.Invoke();
        }

    }
}