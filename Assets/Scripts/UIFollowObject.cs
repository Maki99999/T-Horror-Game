using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class UIFollowObject : MonoBehaviour
    {
        public RectTransform uiElement;
        public Transform target;
        public Vector3 offset;

        private void Awake()
        {
            if (uiElement == null)
                uiElement.GetComponent<RectTransform>();
        }

        private void Update()
        {
            uiElement.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }
}
