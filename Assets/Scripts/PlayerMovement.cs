using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    //Adjustments
    public float speedDodge = 10f;
    public float jumpPower = 7f;
    public float groundProximityJumpGrace = 0.8f; // To adjust jump grace
    public float jumpMultiplier = 3;
    public float rollCoolDown = 0.2f;

    //References
    public GameManager gameManager;
    public InputManager inputManager;
    public CharacterController cc;
    public Animator anim;
    public SkinnedMeshRenderer skinRenderer;
    public Material defaultMaterial;
    public Material hurtMaterial;
    

    //Movement bools - triggered for one frame when receieved from input script
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;

    //Key movement bools
    private bool canMove;
    private bool isInRoll;
    private bool isInJump;
    private bool hasCollided;
    private bool isImmuneToCollision;

    //Lane fields
    private enum Lane { Left, Middle, Right }
    private Lane currentLane = Lane.Middle;
    public float xLaneChangeOffset = 1;
    private float tmpXValueToAdjust = 0f;

    //Velocity
    private float xVel; //horisontal velocity
    private float yVel; //vertical velocity... its set to jumppower when jumping up...

    //Raycast information
    private float tmpColHeight;
    private float tmpColCenterY;
    [SerializeField] private float playerRadius;
    [SerializeField] private float playerHeight;
    [SerializeField] private float reachDistance;

    public void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        inputManager.OnRightSwipe.AddListener(StartRightMove);
        inputManager.OnLeftSwipe.AddListener(StartLeftMove);
        inputManager.OnSwipeUp.AddListener(StartJump);
        inputManager.OnSwipeDown.AddListener(StartRoll);

        //gameManager.OnLoadLevel.AddListener(GetPlayerReady);
        gameManager.OnGameStart.AddListener(StartRunning);
        gameManager.OnCollision.AddListener(RespondToCollision);

        gameManager.OnGameOver.AddListener(Die);
        gameManager.OnGameQuit.AddListener(Quit);
    }

    private void OnEnable()
    {
        GetPlayerReady();
    }

    private void GetPlayerReady()
    {
        
        anim.Play("Idle");
        canMove = false;

        skinRenderer.material = defaultMaterial;
        tmpXValueToAdjust = 0;
        xVel = 0;

        transform.position = new Vector3(0,0,0);
        currentLane = Lane.Middle;

        playerHeight = cc.height;
        playerRadius = cc.radius;
        tmpColHeight = cc.height;
        tmpColCenterY = cc.center.y;
        reachDistance = cc.radius;

        hasCollided = false;
        isInRoll = false;
        isImmuneToCollision = false;

        
    }

    private void StartRunning()
    {
        canMove = true;
        anim.CrossFadeInFixedTime("Running", 0.5f);
    }

    void Update()
    {

        //Move Left
        if (swipeLeft && !isInRoll && canMove)
        {
            swipeLeft = false;

            if (currentLane == Lane.Middle)
            {
                tmpXValueToAdjust = -xLaneChangeOffset;
                currentLane = Lane.Left;
            }
            else if (currentLane == Lane.Right)
            {
                tmpXValueToAdjust = 0;
                currentLane = Lane.Middle;
            }
        }

        //Move Right
        if (swipeRight && !isInRoll && canMove)
        {
            swipeRight = false;

            if (currentLane == Lane.Middle)
            {
                tmpXValueToAdjust = xLaneChangeOffset;
                currentLane = Lane.Right;
            }
            else if (currentLane == Lane.Left)
            {
                tmpXValueToAdjust = 0;
                currentLane = Lane.Middle;
            }
        }

        float targetX = tmpXValueToAdjust;

        xVel = Mathf.Lerp(xVel, tmpXValueToAdjust, Time.deltaTime * speedDodge);
        Vector3 moveVector = new Vector3(xVel - transform.position.x, yVel * Time.deltaTime, 0);
        cc.Move(moveVector);

        Jump();
        Roll();

        if (!isImmuneToCollision)
        {
            CheckForCollisions();
        }
        

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void Jump()
    {
        if (cc.isGrounded && canMove || Physics.Raycast(transform.position, Vector3.down, groundProximityJumpGrace) && canMove) //inbuilt function for charactercontrollers
        {
            if (swipeUp) 
            {
                yVel = jumpPower;

                anim.CrossFadeInFixedTime("Jump", 0.5f); //transitions from this to the next animation, in the duration given...

                isInJump = true;
            }

            //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling")) //landing process
            //{
            //    isInJump = false;
            //}
        }

        else
        {
            yVel -= jumpPower * jumpMultiplier * Time.deltaTime;
        }

        swipeUp = false;
    }

    internal float RollCounter;

    public void Roll()
    {
        if (canMove)
        {
            //Turn off the roll  when 0 is larger than our counter - make sure to reset roll height/center/bool back to non-roll state 
            if (RollCounter <= 0f)
            {
                RollCounter = 0;
                cc.center = new Vector3(0, tmpColCenterY, 0);
                cc.height = tmpColHeight;
                isInRoll = false;
            }

            //if swiping down and the timer is still not out of time... 
            if (swipeDown && RollCounter <= 0f)
            {
                RollCounter = rollCoolDown;
                yVel -= 10f;
                cc.center = new Vector3(0, tmpColCenterY / 2, 0);
                cc.height = tmpColHeight / 2;
                anim.CrossFadeInFixedTime("Roll", 0.3f);
                isInRoll = true;
                isInJump = false;
            }
        }

        RollCounter -= Time.deltaTime; //detract from our roll counter timer 
        swipeDown = false; 
    }

    public void CheckForCollisions()
    {
        Vector3 sphereCastOrigin = transform.position + cc.center; // Adjust origin to the center of the character controller

        if (!hasCollided)
        {
            if (Physics.SphereCast(sphereCastOrigin, playerRadius, transform.forward, out RaycastHit hit, reachDistance))
            {
                if (hit.collider.CompareTag("Obstacle") && canMove)
                {
                    GameManager.Instance.Collide();
                    Debug.Log("Obstacle coll detected!");

                }
            }

            if (Physics.SphereCast(sphereCastOrigin, playerRadius, transform.right, out RaycastHit rightHit, reachDistance))
            {
                if (rightHit.collider.CompareTag("Obstacle") && canMove)
                {
                    GameManager.Instance.Collide();
                    Debug.Log("Obstacle coll detected!");
                }
            }

            if (Physics.SphereCast(sphereCastOrigin, playerRadius, -transform.right, out RaycastHit leftHit, reachDistance))
            {
                if (leftHit.collider.CompareTag("Obstacle") && canMove)
                {
                    GameManager.Instance.Collide();
                    Debug.Log("Obstacle coll detected!");
                }
            }
        }
    }

    private void RespondToCollision()
    {
        StartCoroutine("CollisionProcess");
    }

    private IEnumerator CollisionProcess()
    {
        //at start of wait:
        hasCollided = true;
        isImmuneToCollision = true;
        skinRenderer.material = hurtMaterial;


        yield return new WaitForSeconds(0.3f);

        //Done waiting:
        hasCollided = false;
        isImmuneToCollision = false;
        skinRenderer.material = defaultMaterial;
    }

    private void Die()
    {
        canMove = false;
        anim.Play("Dead");
    }

    private void Quit()
    {
        gameObject.SetActive(false);
    }

    //use input and register action with the local bool
    public void StartRightMove()
    {
        swipeRight = true;
    }
    public void StartLeftMove()
    {
        swipeLeft = true;
    }
    public void StartJump()
    {
        swipeUp = true;
    }
    public void StartRoll()
    {
        swipeDown = true;
    }

}
