using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
   public class PFSimpleMover : MonoBehaviour
   {
      [SerializeField]
      List<Vector3> localPositions = new List<Vector3>(); // Change to localPositions
      [SerializeField]
      int currentPos = 0;
      public GameObject gameObject;
      public float speed = 1;

      // In CapturePosition, add the local position to the list instead of global position
      public void CapturePosition()
      {
         localPositions.Add(gameObject.transform.localPosition);
      }

      // In CyclePosition, do not change anything
      public void CyclePosition()
      {
         currentPos++;
         if (currentPos >= localPositions.Count)
         {
            currentPos = 0;
         }
      }

      // In LerpingMove, we animate to the local position instead of the global position
      protected void LerpingMove()
      {
         Transform transform = gameObject.transform;
         transform.localPosition = Vector3.Lerp(transform.localPosition, localPositions[currentPos], speed * Time.deltaTime);
      }

      public void Update()
      {
         LerpingMove();
      }
    
      public void Reset()
      {
         currentPos = 0;
      }
   }
}