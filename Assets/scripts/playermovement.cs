using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpHeight = 25f;
    [SerializeField] float climbSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D myRigidBody; // component reference
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    float startingGravityScale;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>(); // this has to be done to all component references, or they will be null
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        startingGravityScale = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Climb();

        if (moveInput.x != 0) // had to do this to keep the sprite from resetting
        {
            FlipSprite();
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("ground")) && value.isPressed)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void Climb()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("climbing")))
        {
            myRigidBody.gravityScale = 0f;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
            return;
        }
        myRigidBody.gravityScale = startingGravityScale;
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f); // Sign returns 1 if positive, -1 if negative
    }

    bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.x) >= Mathf.Epsilon; //Epsilon is the smallest non-zero float value
    }
}
