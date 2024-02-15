using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace PuzzleForge
{
	public class PuzzleForgeReactor : MonoBehaviour, IPuzzleForgeActivatable
	{

		// Get animators for all children
		// IEnumerator for deactivation

		public bool inverted = false;
		bool isEnabled;
		public bool overrideSpeed = false;
		public float animationSpeed = 1;

		public bool oneShot = false;
		bool hasLatched = false;
		public bool anyActivator = false;

		[SerializeField]
		public ulong enableMask = 0;
		[SerializeField]
		public ulong disableMask = 0;

		public float AnimationEnableDelay = 0.0f;
		public float AnimationDisableDelay = 0.0f;

		string animatorControllerName = "";
		public Animator anim;

		protected ulong enableMaskRegister = 0;
		protected ulong disableMaskRegister = 0;

		public bool toggle = false;

		[SerializeField]
		private UnityEvent onActivated;

		[SerializeField]
		private UnityEvent onDeactivated;

		// Use this for initialization
		void Awake()
		{
			if (anim == null) // incase one hasn't been specified 
				anim = gameObject.GetComponent<Animator>();

			if (animatorControllerName.Length > 0)
			{
				RuntimeAnimatorController runtimeAnimController = (RuntimeAnimatorController)Resources.Load(animatorControllerName);
				anim.runtimeAnimatorController = runtimeAnimController;
			}
			if (anim != null)
				anim.SetBool("Enabled", inverted);
		}

		void Start()
		{


			Deactivate(0);
		}

		public bool isActivated()
		{
			return isEnabled;

		}

		bool serviceEnableMask(ulong incomingMask)
		{
			enableMaskRegister |= incomingMask;
			disableMaskRegister &= ~incomingMask;

			if (incomingMask == 0) // special use
				return true;

			if ((incomingMask & enableMask) == 0)
				return false;

			return true;
		}

		bool serviceDisableMask(ulong incomingMask)
		{
			disableMaskRegister |= incomingMask;
			enableMaskRegister &= ~incomingMask;

			if (incomingMask == 0) // special use
				return true;

			if ((incomingMask & disableMask) == 0)
				return false;

			return true;
		}


		IEnumerator ActivateCR()
		{
			if (AnimationEnableDelay > 0)
				yield return new WaitForSeconds(AnimationEnableDelay);

			//if((enableMask & enableMaskRegister) != enableMask)
			//	yield break;

			disableMaskRegister = 0;

			isEnabled = !inverted;

			ActivationAction();

			yield break;
		}

		IEnumerator DeactivateCR()
		{
			if (AnimationDisableDelay > 0)
				yield return new WaitForSeconds(AnimationDisableDelay);

			//if((disableMask & disableMaskRegister) != disableMask)
			//	yield break;

			enableMaskRegister = 0;

			isEnabled = inverted;

			DeactivateAction();

			yield break;
		}

		protected virtual void ActivationAction()
		{
			if (onActivated != null)
				onActivated.Invoke();

			if (anim == null)
				return;

			if (overrideSpeed == true)
			{
				if (inverted)
					anim.SetFloat("Speed", 0);
				else
					anim.SetFloat("Speed", animationSpeed);
			}
			else
			{
				anim.SetBool("Enabled", !inverted);
			}
		}

		protected virtual void DeactivateAction()
		{
			if (onDeactivated != null)
				onDeactivated.Invoke();

			if (anim == null)
				return;

			if (overrideSpeed == true)
			{
				if (inverted)
					anim.SetFloat("Speed", animationSpeed);
				else
					anim.SetFloat("Speed", 0);
			}
			else
			{
				if (anim != null)
					anim.SetBool("Enabled", inverted);
			}
		}


		public void Activate(ulong incomingMask)
		{
			if (oneShot && hasLatched)
				return;

			if (serviceEnableMask(incomingMask) == false)
				return;

			if (anyActivator == true || incomingMask == 0)
				enableMaskRegister = enableMask;

			if (toggle == true)
				inverted = !inverted;

			if ((enableMask & enableMaskRegister) == enableMask)
			{
				hasLatched = true;
				StartCoroutine(ActivateCR());
			}
		}

		public void Deactivate(ulong incomingMask)
		{
			if (oneShot && hasLatched)
				return;

			if (serviceDisableMask(incomingMask) == false)
				return;

			if (anyActivator == true || incomingMask == 0)
				disableMaskRegister = disableMask;

			if (toggle == true)
			{
				Activate(incomingMask);
				return;
			}

			if ((disableMask & disableMaskRegister) == disableMask)
			{
				StartCoroutine(DeactivateCR());
			}
		}
	}
}