using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class FenceGate : MonoBehaviour
    {
        public UnityEngine.U2D.Animation.SpriteSkin spriteSkin;
        public Transform collisionTransform;

        void Update()
        {
            foreach (Transform bone in spriteSkin.boneTransforms)
                bone.rotation = collisionTransform.rotation;
        }
    }
}
