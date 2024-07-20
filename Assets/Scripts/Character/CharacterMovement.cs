using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace SnipOuts.Character
{
    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] 
        private Rigidbody2D rb = null;

        [SerializeField]
        private Animator charAnim = null;

        [SerializeField] 
        private Transform groundCheck = null;

        [SerializeField]
        private LayerMask groundLayer = default;
        
        [SerializeField]
        private float speed = 100.0f;

        [SerializeField]
        private float jumpingPower = 30.0f;

        private bool isFacingRight = true;
        private float horizontal;

        private void Update ()
        {
            MovePlayer ();
        }

        private void FixedUpdate ()
        {
            rb.velocity = new Vector2 (horizontal * speed, rb.velocity.y);
        }

        private void MovePlayer ()
        {
            horizontal = Input.GetAxisRaw ("Horizontal");

            if (Input.GetButtonDown ("Jump") && IsGrounded ())
            {
                rb.velocity = new Vector2 (rb.velocity.x, jumpingPower);
            }

            if (Input.GetButtonUp ("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2 (rb.velocity.x, 0);
            }

            if (Mathf.Abs (rb.velocity.x) > 0)
            {
                charAnim.SetBool ("IsRunning", true);
            }
            else
            {
                charAnim.SetBool ("IsRunning", false);
            }

            Flip();
        }

        private bool IsGrounded ()
        {
            return Physics2D.OverlapCircle (groundCheck.position, 0.2f, groundLayer);
        }

        private void Flip ()
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }
}