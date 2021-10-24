using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class ObstacleRat : MonoBehaviour
    {
        public AudioSource cryAudio;
        public GameObject candy;
        public SpriteRenderer spriteRenderer;
        public Animator animator;

        private bool runningAway = false;

        public void RunAway(bool fromLeft)
        {
            if (runningAway)
                return;
            runningAway = true;

            cryAudio.pitch = 1.2f + Random.value * 0.4f;
            cryAudio.Play();

            animator.enabled = false;
            spriteRenderer.flipX = !fromLeft;
            if (!fromLeft)
                candy.transform.localPosition = new Vector3(candy.transform.localPosition.x * -1, candy.transform.localPosition.y, candy.transform.localPosition.z);

            StartCoroutine(RunAndDisappear(!fromLeft));
        }

        IEnumerator RunAndDisappear(bool fromLeft)
        {
            Vector3 oldPos = transform.position;
            Vector3 newPos = GameController.Instance.playerBag.transform.position;

            float rate = 1f / 0.3f;
            float fSmooth;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.Lerp(0f, 1f, f);
                transform.position = Vector3.Lerp(oldPos, newPos, fSmooth);

                yield return null;
            }

            candy.SetActive(true);
            GameController.Instance.HitRat();

            oldPos = transform.position;
            newPos = oldPos + (fromLeft ? Vector3.right : Vector3.left) * 20f;

            rate = 1f / 2f;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.Lerp(0f, 1f, f);
                transform.position = Vector3.Lerp(oldPos, newPos, fSmooth);

                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
