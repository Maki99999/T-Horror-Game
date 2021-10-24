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
        public bool triggerManually = false;

        private void Awake()
        {
            if (uiElement == null)
                uiElement.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!triggerManually)
                uiElement.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }

        public void Trigger()
        {
            uiElement.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }
}
