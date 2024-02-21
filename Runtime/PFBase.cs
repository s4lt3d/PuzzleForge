using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    public abstract class PFBase : MonoBehaviour
    {
        public PFRoomController parentController;
        
        public List<string> objectTags = new List<string>();
        
        public void AddTag(string tag)
        {
            if (!objectTags.Contains(tag))
            {
                objectTags.Add(tag);
            }
        }
        public bool HasTag(string tag)
        {
            return objectTags.Contains(tag);
        }
    }
}