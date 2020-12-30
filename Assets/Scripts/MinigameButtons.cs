using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class MinigameButtons : MonoBehaviour
    {
        public Animator buttonAnimator;

        public void ChangeButton(bool controller, string button)
        {
            if (button.Equals("stop")) {
                buttonAnimator.SetTrigger(button);
                return;
            }
            buttonAnimator.SetBool("controller", controller);
            buttonAnimator.SetTrigger(button);
        }
    }
}
