using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace T
{
    public class GameController : MonoBehaviour
    {
        public AudioMixer mixer;

        public float susValue;
        public float candyValue;

        public MeterSlider susSlider;
        public MeterSlider candySlider;

        public float candyDecreaseValue = 0.1f;

        bool minigameTriggered = false;

        void Start()
        {
            susValue = 0f;
            candyValue = 0.5f;

            susSlider.SetColor(Color.gray);
            candySlider.SetColor(Color.gray);
            susSlider.SetValue(0);
            candySlider.SetValue(16);
            //intro
            StartWalkPhase();
        }

        public void MinigameMistake(int difficulty)
        {
            susValue += Mathf.Lerp(0.1f, 0.5f, difficulty / 10f);   //diff 10 - 0.5; diff 1  - 0.1
            susSlider.SetValue(Mathf.RoundToInt(susValue * 32));

            if (susValue >= 1f)
            {
                Debug.Log("Game Over");
            }
        }

        public void ResetSusValue()
        {
            susValue = 0.0f;
            susSlider.SetValue(0);
        }

        public void StartWalkPhase()
        {
            ResetSusValue();
            candyValue = 1f;
            candySlider.SetValue(32);
            StartCoroutine(CandyDecreaser());
        }

        IEnumerator CandyDecreaser()
        {
            while (!minigameTriggered)
            {
                candyValue -= candyDecreaseValue;
                candySlider.SetValue(Mathf.RoundToInt(candyValue * 32));
                if (candyValue <= 0f)
                {
                    Debug.Log("Game Over"); //TODO:
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void MinigameStarted()
        {

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