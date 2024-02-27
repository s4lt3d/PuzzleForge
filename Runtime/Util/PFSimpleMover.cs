using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PuzzleForge
{
   public class PFSimpleMover : MonoBehaviour
   {
      [FormerlySerializedAs("localPositions")]
      [SerializeField]
      List<Transform> localTransforms = new List<Transform>(); // Change to localPositions
      [SerializeField]
      int currentPos = 0;
      public GameObject gameObject;
      public float speed = 1;

      // In CapturePosition, add the local position to the list instead of global position
      public void CapturePosition()
      {
         localTransforms.Add(gameObject.transform);
      }

      // In CyclePosition, do not change anything
      public void CyclePosition()
      {
         currentPos++;
         if (currentPos >= localTransforms.Count)
         {
            currentPos = 0;
         }
      }

      // In LerpingMove, we animate to the local position instead of the global position
      protected void LerpingMove()
      {
         Transform transform = gameObject.transform;
         transform.localPosition = Vector3.Lerp(transform.localPosition, localTransforms[currentPos].position, speed * Time.deltaTime);
         transform.localRotation = Quaternion.Lerp(transform.localRotation, localTransforms[currentPos].rotation, speed * Time.deltaTime);
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