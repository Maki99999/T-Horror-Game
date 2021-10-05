using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class ObstaclePerson : MonoBehaviour
    {
        public AudioSource cryAudio;
        public GameObject particlesL;
        public GameObject particlesR;

        private bool runningAway = false;

        public void RunAway(bool fromLeft)
        {
            if (runningAway)
                return;
            runningAway = true;

            cryAudio.pitch = 1.2f + Random.value * 0.4f;
            cryAudio.Play();

            if (fromLeft)
                particlesL.SetActive(true);
            else
                particlesR.SetActive(true);

            GameController.Instance.HitObstacle();

            StartCoroutine(RunAndDisappear(fromLeft));
        }

        IEnumerator RunAndDisappear(bool fromLeft)
        {
            Vector3 oldPos = transform.position;
            Vector3 newPos = oldPos + (fromLeft ? Vector3.right : Vector3.left) * 20f;

            float rate = 1f / 2f;
            float fSmooth;
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
