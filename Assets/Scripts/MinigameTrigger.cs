using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

namespace T
{
    public class MinigameTrigger : MonoBehaviour
    {
        public GameObject controllerInput;
        public GameObject keyboardInput;
        public Animator doorAnim;
        public SpriteRenderer dudeSprite;
        public ParticleSystem candyParticles;

        public Sprite[] dudes;

        [Space(10)]

        public AudioSource audioSource;
        public AudioClip sfxDoorBell;
        public AudioClip sfxDoorOpen;
        public AudioClip sfxDoorClose;

        [Space(10)]

        public int buttonCount = 12;
        public int difficulty = 5;

        MinigameController controller;
        bool inRange = false;
        bool triggered = false;

        IEnumerator Start()
        {
            yield return null;
            controller = GameObject.FindObjectOfType<MinigameController>();
            controller.controls.Player.Confirm.performed += ctx => TryStartingMinigame();
            ShowingInput(false);
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
            if (other.CompareTag("Player") && !triggered)
            {
                inRange = true;
                ShowingInput(true);
            }
        }

        void TryStartingMinigame()
        {
            if (!triggered && inRange)
            {
                triggered = true;
                ShowingInput(false);
                controller.player.Freeze();
                StartCoroutine(OpenDoor());
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !triggered)
            {
                inRange = false;
                ShowingInput(false);
            }
        }

        IEnumerator OpenDoor()
        {
            audioSource.clip = sfxDoorBell;
            audioSource.Play();
            yield return new WaitForSeconds(1f);

            dudeSprite.sprite = dudes[Random.Range(0, dudes.Length)];

            audioSource.clip = sfxDoorOpen;
            audioSource.Play();
            doorAnim.SetBool("Open", true);
            yield return new WaitForSeconds(1f);

            yield return controller.Minigame(buttonCount, difficulty);

            candyParticles.Play();
            audioSource.clip = sfxDoorClose;
            audioSource.Play();
            doorAnim.SetBool("Open", false);
        }
    }
}
