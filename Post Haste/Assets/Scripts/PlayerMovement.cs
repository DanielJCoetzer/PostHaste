using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float speed = 5;

    private void Awake() {
        rb = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
    }

    public void OnMovement (InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();

        if (movement.x != 0 || movement.y != 0) {
            animator.SetFloat ("X", movement.x);
            animator.SetFloat ("Y", movement.y);

            animator.SetBool ("isWalking", true);
        }
        else {
            animator.SetBool ("isWalking", false);
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        //if (movement.x != 0 || movement.y != 0) {
        //    rb.velocity = movement * speed;
        //}

        //rb.AddForce (movement * speed);

    
    }
}
