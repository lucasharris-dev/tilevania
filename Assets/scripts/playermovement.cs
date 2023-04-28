using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playermovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 7.5f;
    [SerializeField] float jumpHeight = 25f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float restartTime = 1f;

    float startingGravityScale;
    bool isAlive;
    
    Vector2 moveInput;
    Rigidbody2D myRigidBody; // component reference
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer mySpriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>(); // this has to be done to all component references, or they will be null
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        startingGravityScale = myRigidBody.gravityScale;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        Run();
        Climb();
        FlipSprite();
        Die();
        
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if ((!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("ground")) && value.isPressed) || !isAlive)
        {
            return;
        }
        
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        Instantiate(bullet, gun.position, bullet.transform.rotation);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void Climb()
    {
        bool canClimb = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("climbing"));
        bool isClimbing = moveInput.y != 0 && canClimb;
        myAnimator.SetBool("isClimbing", PlayerHasVerticalSpeed() && isClimbing);

        if (!canClimb)
        {
            myRigidBody.gravityScale = startingGravityScale;
            return;
        }

        myRigidBody.gravityScale = 0f;
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
    }

    void FlipSprite() // finish changing to !expression
    {
        if (!PlayerHasHorizontalSpeed()) // had to do this to keep the sprite from resetting
        {
            return;
        }

        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f); // Sign returns 1 if positive, -1 if negative
    }

    bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.x) >= Mathf.Epsilon; //Epsilon is the smallest non-zero float value
    }

    bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.y) >= Mathf.Epsilon;
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSecondsRealtime(restartTime);

        SceneManager.LoadScene(0);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("enemies", "hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("dying");
            mySpriteRenderer.color = Color.red;
            myRigidBody.velocity = deathKick;
            StartCoroutine(ResetGame());
        }
    }
}
