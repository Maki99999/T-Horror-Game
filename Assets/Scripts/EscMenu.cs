using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

namespace T
{
    public class EscMenu : MonoBehaviour
    {
        [SerializeField] private GameObject escMenuObj;
        private InputMaster controls;
        [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem;
        private bool inEscMenu = false;
        private VideoPlayer[] videoPlayers;
        private List<VideoPlayer> pausedVideoPlayers = new List<VideoPlayer>();

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource[] audioInEscMenu;
        [SerializeField] private AudioSource hoverSFX;
        [SerializeField] private AudioSource clickSFX;
        [SerializeField] private AudioSource sliderChangeSFX;

        [Space(10)]
        [SerializeField] private GameObject firstSelectedGameObject;
        [SerializeField] private Slider sliderSFX;
        [SerializeField] private Slider sliderBGM;


        private void Awake()
        {
            foreach (AudioSource audio in audioInEscMenu)
                audio.ignoreListenerPause = true;

            videoPlayers = GameObject.FindObjectsOfType<VideoPlayer>(true);

            sliderSFX.onValueChanged.AddListener(delegate { ChangeVolume(VolumeType.SFX, sliderSFX.value, true); });
            sliderBGM.onValueChanged.AddListener(delegate { ChangeVolume(VolumeType.BGM, sliderBGM.value, true); });
        }

        private void Start()
        {
            controls = GameController.Instance.player.controls;
            controls.Esc.Esc.performed += ctx => PressedEsc();
            controls.Menu.Disable();

            LoadSettings();
            escMenuObj.SetActive(inEscMenu);
        }

        private void PressedEsc()
        {
            ToggleEscMenu();
        }

        private void ToggleEscMenu()
        {
            inEscMenu = !inEscMenu;
            escMenuObj.SetActive(inEscMenu);

            if (inEscMenu)
            {
                Pause();
                eventSystem.SetSelectedGameObject(firstSelectedGameObject);
            }
            else
                Unpause();
        }

        private void Pause()
        {
            AudioListener.pause = true;
            Time.timeScale = 0f;
            controls.Player.Disable();
            controls.Minigame.Disable();
            controls.Menu.Enable();

            pausedVideoPlayers = new List<VideoPlayer>();
            foreach (VideoPlayer videoPlayer in videoPlayers)
                if (videoPlayer.isPlaying)
                {
                    pausedVideoPlayers.Add(videoPlayer);
                    videoPlayer.Pause();
                }
        }

        private void Unpause()
        {
            AudioListener.pause = false;
            Time.timeScale = 1f;
            controls.Player.Enable();
            controls.Minigame.Enable();
            controls.Menu.Disable();

            foreach (VideoPlayer videoPlayer in pausedVideoPlayers)
                videoPlayer.Play();
        }

        public void PlaySliderSFX()
        {
            if (!sliderChangeSFX.isPlaying)
                sliderChangeSFX.Play();
        }

        public void PlayHoverSFX(GameObject gameObject)
        {
            if (eventSystem.currentSelectedGameObject != gameObject)
                hoverSFX.Play();
        }

        public void ButtonContinue()
        {
            clickSFX.Play();
            ToggleEscMenu();
        }

        public void ButtonReset()
        {
            clickSFX.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ButtonExit()
        {
            clickSFX.Play();
            Application.Quit();
        }

        private void LoadSettings()
        {
            ChangeVolume(VolumeType.SFX, PlayerPrefs.GetFloat(VolumeTypeString(VolumeType.SFX), 1), false);
            ChangeVolume(VolumeType.BGM, PlayerPrefs.GetFloat(VolumeTypeString(VolumeType.BGM), 1), false);
        }

        private void ChangeVolume(VolumeType volumeType, float value, bool withSave = true)
        {
            mixer.SetFloat(VolumeTypeString(volumeType), Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
            VolumeTypeSlider(volumeType).SetValueWithoutNotify(value);

            if (withSave)
                PlayerPrefs.SetFloat(VolumeTypeString(volumeType), value);
        }

        private string VolumeTypeString(VolumeType volumeType)
        {
            return volumeType switch
            {
                VolumeType.SFX => "SFXVol",
                VolumeType.BGM => "BGMVol",
                _ => ""
            };
        }

        private Slider VolumeTypeSlider(VolumeType volumeType)
        {
            return volumeType switch
            {
                VolumeType.SFX => sliderSFX,
                VolumeType.BGM => sliderBGM,
                _ => null
            };
        }

        private enum VolumeType
        {
            SFX,
            BGM
        }
    }
}
