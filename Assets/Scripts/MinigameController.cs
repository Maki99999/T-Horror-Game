using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace T
{
    public class MinigameController : MonoBehaviour
    {
        public PlayerController player;
        public InputMaster controls;
        public MinigameButtons buttons;
        public GameObject buttonsBackground;
        public MinigameText text;
        public MeterSlider timeSlider;

        public AudioClip[] voice;
        public AudioSource voiceAudioSource;
        public AudioClip sfxWrong;
        public AudioClip sfxFinish;
        public AudioSource buttonAudioSource;

        public GameController gameController;

        public float maxTime;
        public float minTime;

        string[] buttonStrings = new string[] { "up", "down", "left", "right", "primary", "secondary" };
        string currentButton = "";
        string lastButtonPressed = "";

        int spookyTextLetterCount = 0;

        bool timerTriggered = false;

        void Start()
        {
            controls = player.controls;
            controls.Minigame.Primary.performed += ctx => ClickedButton("primary");
            controls.Minigame.Secondary.performed += ctx => ClickedButton("secondary");
            controls.Minigame.Up.performed += ctx => ClickedButton("up");
            controls.Minigame.Down.performed += ctx => ClickedButton("down");
            controls.Minigame.Left.performed += ctx => ClickedButton("left");
            controls.Minigame.Right.performed += ctx => ClickedButton("right");
            controls.Minigame.Disable();

            timeSlider.SetColor(Color.gray);
            timeSlider.gameObject.SetActive(false);
        }

        void ClickedButton(string button)
        {
            lastButtonPressed = button;
        }

        public IEnumerator Minigame(int buttonCount, int difficulty)
        {
            spookyTextLetterCount = -1;
            controls.Minigame.Enable();
            buttonsBackground.SetActive(true);

            timeSlider.SetValue(1);
            timeSlider.gameObject.SetActive(true);

            for (int i = 0; i < buttonCount; i++)
            {
                currentButton = buttonStrings[Random.Range(0, buttonStrings.Length)];

                buttons.ChangeButton(GameController.isControllerActive(), currentButton);

                lastButtonPressed = "";
                while (!lastButtonPressed.Equals(currentButton))
                {
                    lastButtonPressed = "";
                    timerTriggered = false;
                    Coroutine timer = StartCoroutine(ButtonTimer(maxTime - (difficulty / 10f) * (maxTime - minTime)));
                    while (lastButtonPressed.Equals("") && !timerTriggered)
                        yield return null;
                    string pressedButton = lastButtonPressed;
                    StopCoroutine(timer);

                    if (!lastButtonPressed.Equals(currentButton))
                    {
                        gameController.MinigameMistake(difficulty);
                        buttonAudioSource.clip = sfxWrong;
                        buttonAudioSource.Play();
                    }
                    yield return null;
                }
                yield return ChangeSpookyText(i, buttonCount);
            }
            buttonAudioSource.clip = sfxFinish;
            buttonAudioSource.Play();

            gameController.StartWalkPhase();
            player.Unfreeze();
            buttons.ChangeButton(false, "stop");
            text.ResetText();
            buttonsBackground.SetActive(false);
            timeSlider.gameObject.SetActive(false);
        }

        IEnumerator ButtonTimer(float time)
        {
            float startTime = Time.time;
            while (Time.time < startTime + time)
            {
                timeSlider.SetValue(Mathf.RoundToInt(((1 - (Time.time - startTime) / time)) * 32));
                yield return null;
            }
            timerTriggered = true;
        }

        IEnumerator ChangeSpookyText(int currentButtonInt, int ButtonCount)
        {
            float currentButton12 = ((float)currentButtonInt / (float)ButtonCount) * 12f;
            for (int i = 0; i < 12; i++)
            {
                if (i <= currentButton12 && i > spookyTextLetterCount)
                {
                    spookyTextLetterCount++;
                    voiceAudioSource.clip = voice[i];
                    voiceAudioSource.Play();
                    text.SetLetters(i + 1);
                    player.WrapAnim.SetTrigger("Jitter");
                    yield return new WaitWhile(() => voiceAudioSource.isPlaying);
                }
            }
        }
    }
}
