using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class lookAt : MonoBehaviour
    {
        public Transform[] target;
        public int count = 0;

        public PathCreator[] pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float distanceTravelled = 0;
        public bool camera = false;
        public bool moving = true;

        void Update()
        {
            // Rotate the camera every frame so it keeps looking at the target
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                if (moving) transform.position = pathCreator[count].path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if (!camera) transform.rotation = pathCreator[count].path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
            transform.LookAt(target[count]);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Switch"))
            {
                distanceTravelled = 0;
                Debug.Log(other.tag);
                if (count == target.Length - 1)
                {
                    count = 0;
                }
                else
                {
                    count++;
                }
            }
            Debug.Log(count);
        }
    }
}