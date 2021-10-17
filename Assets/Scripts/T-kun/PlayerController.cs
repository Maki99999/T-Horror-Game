using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace T
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D rb;
        new public Collider2D collider;
        public Animator anim;
        public Animator WrapAnim;
        public InputMaster controls;
        public float speed = 1f;

        private int freeze = 0;
        private Vector2 movement = Vector2.zero;
        private bool facingLeft = true;
        private bool overrideMove = false;

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
                TryMove(!GameController.Instance.gameActive);
        }

        private void TryMove(bool standStill = false)
        {
            if (overrideMove)
                return;
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
            Move(direction);
        }

        private void Move(Vector3 direction)
        {
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

        public IEnumerator MoveTo(Vector3 position, bool ignoreCollision = false)
        {
            if (ignoreCollision)
                collider.enabled = false;

            overrideMove = true;
            Vector3 direction = (position - transform.position).normalized * speed;
            float distance = 2f;
            while (freeze == 0 && distance > 0.1f)
            {
                Move(direction);
                distance = (position - transform.position).sqrMagnitude;
                yield return null;
            }

            overrideMove = false;
            TryMove(true);
            if (ignoreCollision)
                collider.enabled = true;
        }

        public void Freeze()
        {
            if (--freeze == 0)
            {
                TryMove(true);
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