using UnityEngine;
using System.Collections.Generic;

namespace PuzzleForge
{
	public class ActivatorActionLayer : ScriptableObject
	{
		public int actionLayer = 0;
	}

	public class PuzzleForgeActivator : MonoBehaviour
	{
		PuzzleForgeController switchController;

		[HideInInspector]
		public int actionLayer = 0;
		public bool latching = false;
		public bool invert = false;
		public bool mouseClickDebug = false;
		public bool keyboardDebug = false;
		bool mouseState = false;
		bool keyboardState = false;
		bool latched = false;
		bool isEnabled = false;
		public int unitsRequiredToActivate = 1;
		public int unitsRequiredToDeactivate = 0;
		public List<string> tagRequired = new List<string> { "Player" };
		public bool toggle = false;
		bool triggerToggleState = false;


		List<GameObject> triggeredObjects = new List<GameObject>();

		// Use this for initialization
		void Start()
		{

			switchController = FindSwitchController(transform.parent);
		}
		int depthCount = 0;

		PuzzleForgeController FindSwitchController(Transform obj)
		{

			PuzzleForgeController sc;

			if (obj == null)
				return null;

			depthCount++;
			if (depthCount > 10)
				return null;
			sc = obj.GetComponent<PuzzleForgeController>();
			if (sc == null)
				sc = FindSwitchController(obj.parent);

			return sc;
		}

		void Update()
		{
			if (keyboardDebug == true)
			{
				if (switchController != null && mouseClickDebug == true)
				{
					if (Input.GetButtonDown("Fire2"))
					{
						keyboardState = !keyboardState;
						if (keyboardState == true)
							Enable();
						else
							Disable();
					}
				}
			}
		}

		void OnMouseDown()
		{

			if (switchController != null && mouseClickDebug == true)
			{
				mouseState = !mouseState;
				if (mouseState == true)
				{
					Debug.Log("Mouse Enable Event");
					Enable();
				}
				else
				{
					Debug.Log("Mouse Disable Event");
					Disable();
				}
			}
		}

		public void Enable()
		{
			if (switchController == null)
				return;
			if (isEnabled == true)
				return;
			isEnabled = true;

			if (latched && latching)
				return;
			latched = true;

			if (invert == false)
				switchController.Enable(actionLayer);
			else
				switchController.Disable(actionLayer);
		}

		public void Disable()
		{
			if (switchController == null)
				return;
			if (isEnabled == false)
				return;
			isEnabled = false;

			if (latched && latching)
				return;

			if (invert == true)
				switchController.Enable(actionLayer);
			else
				switchController.Disable(actionLayer);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			TrigerEnter(other);
		}


		void OnTriggerEnter(Collider other)
		{
			TrigerEnter(other);

		}

		void TrigerEnter(Component other)
		{
			if (tagRequired.Contains(other.tag))
			{
				if (triggeredObjects.Contains(other.gameObject))
					return;
				triggeredObjects.Add(other.gameObject);
				if (triggeredObjects.Count >= unitsRequiredToActivate)
				{
					if (toggle == false)
						Enable();

					if (toggle == true)
					{
						if (triggerToggleState == false)
						{
							Enable();
						}
						else
						{
							Disable();
						}
					}
					triggerToggleState = !triggerToggleState;
				}
			}
		}

		void OnTriggerExit(Collider other)
		{
			TriggerExit(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			TriggerExit(other);
		}

		void TriggerExit(Component other)
		{
			if (tagRequired.Contains(other.tag))
			{
				triggeredObjects.Remove(other.gameObject);
				if (triggeredObjects.Count <= unitsRequiredToDeactivate)
				{
					if (toggle == false)
						Disable();
					// Don't do anything if toggle is enabled. All toggle action happens in trigger enter
				}
			}
		}
	}
}