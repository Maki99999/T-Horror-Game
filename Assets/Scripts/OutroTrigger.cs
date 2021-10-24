using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace T
{
    public class OutroTrigger : MonoBehaviour
    {
        public GameObject controllerInput;
        public GameObject keyboardInput;
        public Animator doorAnim;

        public GameObject speechBubble;
        public UIFollowObject speechBubbleFollow;
        public Text speechBubbleText;

        [Space(10)]

        public AudioSource audioSource;
        public AudioClip sfxDoorBell;
        public AudioClip sfxDoorOpen;
        public AudioClip sfxDoorOutro;

        public Transform playerOutroPos;

        bool inRange = false;
        int triggered = 0;

        private void Start()
        {
            doorAnim.SetBool("Open", true);
            GameController.Instance.player.controls.Player.Confirm.performed += ctx => Ring();
            ShowingInput(false);
        }

        void Ring()
        {
            if (!inRange)
                return;

            triggered++;
            switch (triggered)
            {
                case 1:
                    audioSource.clip = sfxDoorBell;
                    audioSource.Play();
                    break;
                case 2:
                    audioSource.clip = sfxDoorBell;
                    audioSource.Play();
                    break;
                case 3:
                    StartCoroutine(OutroAnim());
                    break;
                default:
                    break;
            }
        }

        private IEnumerator OutroAnim()
        {
            ShowingInput(false);
            GameController.Instance.player.Freeze();
            audioSource.clip = sfxDoorOutro;
            audioSource.Play();
            yield return new WaitForSeconds(2.8f);
            doorAnim.SetBool("Open", false);
            audioSource.clip = sfxDoorOpen;
            audioSource.Play();
            speechBubble.SetActive(true);
            speechBubbleText.gameObject.SetActive(true);
            speechBubbleFollow.Trigger();

            speechBubbleText.text = "";
            string[] texts = { "go away", "no candy", "no\nhalloween", "go", "i'll call\nthe police" };
            for (int i = 0; i < 2; i++)
            {
                foreach (string text in texts)
                {
                    speechBubbleText.text = text;
                    yield return new WaitForSeconds(0.3f);
                }
            }
            StartCoroutine(OutroTextAnim());
            yield return GameController.Instance.player.MoveTo(playerOutroPos.position, true);
            yield return GameController.Instance.Outro();
        }

        private IEnumerator OutroTextAnim()
        {
            string[] texts = { "stop", " stop", "\nstop   ", "stop     ", "\nstop" };
            while (isActiveAndEnabled)
            {
                foreach (string text in texts)
                {
                    speechBubbleText.text = text;
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }

        void ShowingInput(bool on)
        {
            if (on)
            {
                if (GameController.isControllerActive())
                    controllerInput.SetActive(true);
                else
                    keyboardInput.SetActive(true);
            }
            else
            {
                controllerInput.SetActive(false);
                keyboardInput.SetActive(false);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && triggered < 3)
            {
                inRange = true;
                ShowingInput(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && triggered < 3)
            {
                inRange = false;
                ShowingInput(false);
            }
        }
    }
}
