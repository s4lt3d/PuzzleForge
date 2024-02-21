using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;


namespace PuzzleForge
{
    [Serializable]
    public class PFSpineAnimationParameters
    {
        public string eventName;  // Used to trigger the animation
        public bool loop = false; // Determines if the animation should loop
    }

    /// <summary>
    /// Helps play Spine animations with specific settings from Unity events in the Inspector without code.
    /// </summary>
    public class PFSpineAnimatorHelper : MonoBehaviour
    {
        public List<PFSpineAnimationParameters> animationParameters;

        private SkeletonAnimation skeletonAnimation;

        private void Awake()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
        }

        public void SetSpineAnimation(string eventName)
        {
            var param = animationParameters.Find(p => p.eventName == eventName);
            if (param == null)
            {
                Debug.LogWarning("Animation event name not found: " + eventName);
                return;
            }

            PlaySpineAnimation(eventName, param.loop);
        }

        private void PlaySpineAnimation(string animationName, bool loop)
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
                
            }
        }
    }
}