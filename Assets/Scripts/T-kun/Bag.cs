using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Experimental.Rendering.Universal;

namespace T
{
    public class Bag : MonoBehaviour
    {
        public Collider2D bagCollider;
        public Transform playerBagTransform;
        public Collider2D playerCollider;
        public Transform lineTransform;

        private float lastDistance = 0;
        private Vector3 lastDistanceVector = Vector3.zero;

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
            Debug.DrawRay(playerBagTransform.position, Vector3.up);
            lastDistanceVector = transform.position + transform.up * 0.5f - playerBagTransform.position;
            lastDistance = lastDistanceVector.magnitude;
            lineTransform.localScale = new Vector3(2, 2 + lastDistance * 16, 2);

            float angle = Mathf.Atan2(lastDistanceVector.y, lastDistanceVector.x) * Mathf.Rad2Deg;
            lineTransform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        }

        private IEnumerator CheckDistance()
        {
            while (isActiveAndEnabled)
            {
                if (lastDistance > 4f)
                {
                    bagCollider.enabled = false;
                    yield return new WaitForSeconds(2f);
                    bagCollider.enabled = true;
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
