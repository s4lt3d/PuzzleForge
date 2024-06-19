using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    public class PFSimpleMover : MonoBehaviour
    {
        [SerializeField]
        List<Vector3> localPositions = new List<Vector3>(); // List to store local positions
        [SerializeField]
        List<Quaternion> localRotations = new List<Quaternion>(); // List to store local rotations
        [SerializeField]
        int currentPos = 0;
        public float speed = 1;

        // Capture both the local position and local rotation
        public void CapturePosition()
        {
            localPositions.Add(transform.localPosition);
            localRotations.Add(transform.localRotation);
        }

        // Cycle to the next position and rotation
        public void CyclePosition(bool immediate = false)
        {
            currentPos++;
            if (currentPos >= localPositions.Count)
            {
                currentPos = 0;
            }
            
            if(immediate)
            {
                transform.localPosition = localPositions[currentPos];
                transform.localRotation = localRotations[currentPos];
            }
        }

        // Lerp to the local position and rotation
        protected void LerpingMove()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, localPositions[currentPos], speed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, localRotations[currentPos], speed * Time.deltaTime);
        }

        public void Update()
        {
            LerpingMove();
        }

        public void Reset(bool immediate = false)
        {
            currentPos = 0;
            if(immediate)
            {
                transform.localPosition = localPositions[currentPos];
                transform.localRotation = localRotations[currentPos];
            }
        }
    }
}