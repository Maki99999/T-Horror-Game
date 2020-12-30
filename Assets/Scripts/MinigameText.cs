using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class MinigameText : MonoBehaviour
    {
        Animator[] anims;

        int currentLetterCount = 0;

        void Start()
        {
            anims = GetComponentsInChildren<Animator>();
        }

        public void SetLetters(int letterCount)
        {
            for (int i = currentLetterCount; i < letterCount; i++)
            {
                anims[i].SetTrigger("Animation");
            }
                currentLetterCount = letterCount;
        }

        public void ResetText()
        {
            foreach (Animator anim in anims)
            {
                anim.SetTrigger("Reset");
            }
            currentLetterCount = 0;
        }
    }
}
