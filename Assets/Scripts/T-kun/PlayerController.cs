using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace T
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Animator anim;
        public Animator WrapAnim;
        public InputMaster controls;
        public float speed = 1f;

        private int freeze = 0;
        private Vector2 movement = Vector2.zero;
        private bool facingLeft = true;

        void Awake()
        {
            controls = new InputMaster();

            controls.Player.MovementX.performed += ctx => movement.x = ctx.ReadValue<float>();
            controls.Player.MovementY.performed += ctx => movement.y = ctx.ReadValue<float>();
        }

        void Start()
        {
            Unfreeze();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        void FixedUpdate()
        {
            if (freeze > 0)
                Move(!GameController.Instance.gameActive);
        }

        void Move(bool standStill = false)
        {
            Vector2 direction;
            if (standStill)
            {
                direction = Vector2.zero;
            }
            else
            {
                direction = movement;
                direction.Normalize();
                direction *= speed;
            }

            rb.velocity = direction * Time.deltaTime;

            if (direction.x > 0f && facingLeft)
            {
                facingLeft = false;
                anim.SetTrigger("TurnRight");
            }
            if (direction.x < 0f && !facingLeft)
            {
                facingLeft = true;
                anim.SetTrigger("TurnLeft");
            }
            anim.SetBool("Walking", direction.sqrMagnitude > 0f);
        }

        public void Freeze()
        {
            if (--freeze == 0)
            {
                Move(true);
            }
        }

        public void Unfreeze()
        {
            if (freeze++ == 0)
            {
                //UnfreezeFrame
            }
        }
    }
}