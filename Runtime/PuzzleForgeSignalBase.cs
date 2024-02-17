using UnityEngine;
using System.Collections.Generic;

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
		ActivateAndDeactivate,
		ActivateOnly,
		DeactivateOnly
	}
	
	[IconAttribute(@"Packages/com.waltergordy.puzzleforge/Editor/Resources/PuzzleForgeTriggerSignal.png")]
	public class PuzzleForgeSignalBase : MonoBehaviour
	{
		public TriggerType triggerType;
		public TriggerMode triggerMode;
		public TriggerInteractions triggerInteractions;
		public List<string> tagRequired = new List<string> { "Player" };
		public List<Reactor> connectedReactors;
		
        [Range(0, 10)]
        public int activationTagCount = 1;
        [Range(0, 10)]
        public int deactivateTagCount = 0;
        
        public bool mouseClickDebug = false;

        public List<string> tagsActive = new List<string>();
		bool mouseState = false;
		bool hasFired = false;

		private bool state = false;

		protected void SendSignal(Component component, bool ingress)
		{
			if (ingress)
			{
				if (component != null)
				{
					tagsActive.Add(component.tag);
					if (tagsActive.Count < activationTagCount)
						return;
				}
				if (triggerInteractions == TriggerInteractions.DeactivateOnly)
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
				if (triggerInteractions == TriggerInteractions.ActivateOnly)
					return;
			}

			if (triggerType == TriggerType.Latching)
			{
				if (hasFired) return;
				hasFired = true;
			}
			
			SetNextState(ingress);
			
			foreach (var reactor in connectedReactors)
			{
				reactor.React(state);
			}
		}
		
		protected void SetNextState(bool ingress)
		{
			if (triggerType == TriggerType.Toggle)
			{
				state = !state;
			}
			else
			{
				state = triggerMode == TriggerMode.Normal ? ingress : !ingress;
			}
		}
		
		void OnMouseDown()
		{
			if (mouseClickDebug == true)
			{
				mouseState = !mouseState;
				if (mouseState == true)
				{
					Debug.Log("Debug Activate Event");
					SendSignal(null,true);
				}
				else
				{

					DebugActivate();
				}
			}
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
