﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace T
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        public static GameController Instance { get { return _instance; } }

        public AudioMixer mixer;
        [SerializeField] private MeterSlider susSlider;
        [SerializeField] private MeterSlider candySlider;
        [SerializeField] private PlayerController player;

        [Space(10), SerializeField] private CanvasGroup videoCanvasGroup;
        [SerializeField] private GameObject videoImage;
        [SerializeField] private RenderTexture videoRenderTexture;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private VideoClip introVideo;
        [SerializeField] private GameObject introAudio;
        [SerializeField] private Animator introDoorAnim;
        [SerializeField] private AudioSource introDoorAudio;
        [SerializeField] private Animator introWindowAnim;
        [SerializeField] private Transform playerStartPos;
        [SerializeField] private VideoClip deathVideo;
        [SerializeField] private GameObject deathAudio;

        [Space(10), SerializeField] private float candyDecreaseValue = 0.1f;
        private float susValue;
        private float candyValue;

        [Space(10)] public bool skipIntro;

        public bool gameActive { get; private set; } = false;
        private bool minigameTriggered = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;
        }

        IEnumerator Start()
        {
            susValue = 0f;
            candyValue = 0.5f;

            susSlider.SetColor(Color.gray);
            candySlider.SetColor(Color.gray);
            susSlider.SetValue(0);
            candySlider.SetValue(16);

            if (!skipIntro)
                yield return Intro();
            gameActive = true;
            StartWalkPhase();
        }

        IEnumerator Intro()
        {
            videoRenderTexture.Release();
            videoCanvasGroup.alpha = 1f;
            introAudio.SetActive(true);

            yield return new WaitForSeconds(4f);
            videoImage.SetActive(true);
            videoPlayer.clip = introVideo;
            yield return null;
            videoPlayer.Play();

            yield return new WaitForSeconds(1f); ;
            yield return new WaitWhile(() => videoPlayer.isPlaying);
            videoImage.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            introAudio.SetActive(false);
            player.transform.position = playerStartPos.position;
            videoCanvasGroup.alpha = 0f;

            introDoorAnim.SetBool("Open", true);
            introWindowAnim.SetTrigger("Start");
            introDoorAudio.PlayDelayed(1f);
        }

        public void MinigameMistake(int difficulty)
        {
            susValue += Mathf.Lerp(0.1f, 0.5f, difficulty / 10f);   //diff 10 -> 0.5; diff 0 -> 0.1
            susSlider.SetValue(Mathf.RoundToInt(susValue * 32));

            if (susValue >= 1f)
            {
                StartCoroutine(Death());
            }
        }

        public void HitObstacle()
        {
            MinigameMistake(4);
        }

        public void ResetSusValue()
        {
            susValue = 0.0f;
            susSlider.SetValue(0);
        }

        public void StartWalkPhase()
        {
            if (!gameActive)
                return;

            ResetSusValue();
            candyValue = 1f;
            candySlider.SetValue(32);
            StartCoroutine(CandyDecreaser());
        }

        private IEnumerator Death()
        {
            if (!gameActive)
                yield break;
            gameActive = false;

            videoRenderTexture.Release();
            videoCanvasGroup.alpha = 1f;

            yield return new WaitForSeconds(2f);
            videoImage.SetActive(true);
            videoPlayer.clip = deathVideo;
            videoPlayer.Play();
            deathAudio.SetActive(true);

            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => videoPlayer.isPlaying);
            videoImage.SetActive(false);

            yield return new WaitForSeconds(1f);
            deathAudio.SetActive(false);

            Application.Quit();
            StopAllCoroutines();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private IEnumerator CandyDecreaser()
        {
            while (!minigameTriggered && gameActive)
            {
                candyValue -= candyDecreaseValue;
                candySlider.SetValue(Mathf.RoundToInt(candyValue * 32));
                if (candyValue <= 0f)
                {
                    StartCoroutine(Death());
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void ChangeVolumeMusic(float value)
        {
            mixer.SetFloat("MusicVol", value);
        }

        public void ChangeVolumeSFX(float value)
        {
            mixer.SetFloat("SFXVol", value);
        }

        public static bool isControllerActive()
        {
            double gamepadTime = Gamepad.current == null ? -1d : Gamepad.current.lastUpdateTime;
            double keyboardTime = Keyboard.current == null ? -1d : Keyboard.current.lastUpdateTime;
            double mouseTime = Mouse.current == null ? -1d : Mouse.current.lastUpdateTime;

            double keyboardMouseTime = keyboardTime < mouseTime ? mouseTime : keyboardTime;

            return gamepadTime > keyboardMouseTime;
        }
    }
}