using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace PuzzleForge
{
	public class TriggerActionMask : ScriptableObject
	{
		public int actionMask = 0;
	}

	[IconAttribute(@"Packages/com.waltergordy.puzzleforge/Editor/Resources/PuzzleForgeTriggerSignal.png")]
	public class PuzzleForgeTriggerSignal : MonoBehaviour
	{
		public TriggerMode triggerMode;
		public TriggerType triggerType;
		private TriggerInteractions triggerInteractions;
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

		private void SendSignal(Component component, bool ingress)
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

			switch (triggerMode)
			{
				case TriggerMode.Simple:
					if (triggerType == TriggerType.Normal)
						state = ingress;
					else
						state = !ingress;
					break;
				case TriggerMode.Latching:
					if (hasFired)
						return;
					break;
				case TriggerMode.Toggle:
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


		void OnTriggerEnter2D(Collider2D other)
		{
			SendSignal(other, true);
		}

		void OnTriggerEnter(Collider other)
		{
			SendSignal(other, true);
		}

		void OnTriggerExit(Collider other)
		{
			SendSignal(other, false);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			SendSignal(other, false);
		}
	}
}