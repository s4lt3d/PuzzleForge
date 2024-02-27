using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    /// <summary>
    /// The base class for all PuzzleForge objects.
    /// </summary>
    public abstract class PFBase : MonoBehaviour
    {
       // public PFRoomController parentController;
        
        protected List<string> objectTags = new List<string>();

        public List<string> ObjectTags
        {
            get => objectTags;
            set => objectTags = value;
        }

        /// <summary>
        /// Adds a tag to the list of object tags.
        /// </summary>
        /// <param name="tag">The tag to add.</param>
        public void AddTag(string tag)
        {
            if (!ObjectTags.Contains(tag))
            {
                ObjectTags.Add(tag);
            }
        }

        /// <summary>
        /// Checks if the object has a specific tag.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <returns>True if the object has the tag, false otherwise.</returns>
        public bool HasTag(string tag)
        {
            return ObjectTags.Contains(tag);
        }

        public virtual void Reset()
        {
            // empty
        }
    }
}