using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class CameraController : MonoBehaviour
    {
        public Transform playerTransform;
        public Transform camMin;
        public Transform camMax;

        void Start() {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }

        void LateUpdate()
        {
            Vector2 resolutionHalf = new Vector2(20, 11.25f) / 2;

            float newX = Mathf.Clamp(playerTransform.position.x, camMin.position.x + resolutionHalf.x, camMax.position.x - resolutionHalf.x);
            float newY = Mathf.Clamp(playerTransform.position.y, camMin.position.y + resolutionHalf.y, camMax.position.y - resolutionHalf.y);

            Vector3 newPos = new Vector3(newX, newY, transform.position.z);

            transform.position = newPos;
        }
    }
}