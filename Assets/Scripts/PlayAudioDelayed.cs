using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioDelayed : MonoBehaviour
    {
        public float delay = 5;

        void Start()
        {
            GetComponent<AudioSource>().PlayDelayed(delay);
        }
    }
}
