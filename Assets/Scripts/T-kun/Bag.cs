using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class Bag : MonoBehaviour
    {
        public Collider2D bagCollider;
        public Transform playerBagTransform;
        public Collider2D playerCollider;
        public LineRenderer lineRenderer;

        private Vector3 lastDistance = Vector3.zero;

        private void Start()
        {
            Physics2D.IgnoreCollision(playerCollider, bagCollider);
        }

        private void OnEnable()
        {
            StartCoroutine(CheckDistance());
        }

        private void Update()
        {
            lastDistance = transform.position + transform.up * 0.5f - playerBagTransform.position;
            lineRenderer.SetPosition(1, lastDistance);
        }

        private IEnumerator CheckDistance()
        {
            while (isActiveAndEnabled)
            {
                if (lastDistance.sqrMagnitude > 16f)
                {
                    bagCollider.enabled = false;
                    yield return new WaitForSeconds(2f);;
                    bagCollider.enabled = true;
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
