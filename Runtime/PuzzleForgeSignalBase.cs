using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

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
		bool isEnabled = false;

		bool triggerToggleState = false;

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

			switch (triggerType)
			{
				case TriggerType.Simple:
					if (triggerMode == TriggerMode.Normal)
						state = ingress;
					else
						state = !ingress;
					break;
				case TriggerType.Latching:
					if (hasFired)
						return;
					break;
				case TriggerType.Toggle:
					state = !state;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			hasFired = true;
			foreach (var reactor in connectedReactors)
			{
				reactor.React(state);
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