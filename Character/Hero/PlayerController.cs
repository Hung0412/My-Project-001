using UnityEngine;
public class PlayerController : MonoBehaviour
{
    // This script contains references to components and game objects, as well as variables used for controlling the player character

    private CharacterData characterData;
    [HideInInspector]
    public Rigidbody2D rb2D;
    [HideInInspector]
    public Animator animator;
    private PlayerCombat playerCombat;
    private EffectController effectController;


    private PlayerCombatController playerCombatController;

    #region  Player's for Movement References & Values
    private float playerMovement;
    private float movementSpeed;
    public bool movementCheck = true;
    public float walksSpeed = 7f; // The speed at which the player walks
    public bool isWalking = false;
    public float runSpeed = 15f; // The speed at which the player runs
    public bool isRunning = false;
    public float runningResetTime = 3;
    [HideInInspector]
    public float runningTimer;
    public float nextRunningResetTime = 4;
    [HideInInspector]
    public float nextRunningTimer;
    private GameObject bloomVolume;
    #endregion
    #region  Player's for Jump References & Values
    public GameObject jumpEffectPrefab;
    public GameObject doubleJumpEffectPrefab;
    public bool isJumping = false; // Whether the player is currently jumping
    public float jumpForce; // The force applied to the player when they jump
    public bool isDoubleJump = true; // Whether the player is currently able to double jump
    public float doubleJumpForce; // The force applied to the player when they double jump
    public bool isFalling = false; // Whether the player is currently falling
    #endregion
    public bool isOnGround = true; // Whether the player is currently on the ground
    public bool isFacing = true; // Whether the player is facing left (false) or right (true)
    public int facingVal;
    public bool isCrouching = false; // Whether the player is currently crouching
    public bool isOnAir = false; //Whether the player is currently on the air

    public Transform rollThroughPoint;
    public float rollThroughRange;
    private Collider2D[] hitEnemy;
    public LayerMask enemyLayer;
    public bool isRolling = false; // Whether the player is currently rolling
    public float rollingForce; // The force applied to the player when they roll

    public bool isHealing = false; //Whether the player is currently medizing
    // Start is called before the first frame update
    void Start()
    {
        // Get references to the player's Rigidbody2D, Animator, and PlayerCombat components
        characterData = GetComponent<CharacterDataReference>().characterData;
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
        playerCombatController = GetComponent<PlayerCombatController>();

        // Find and get references to the EffectController game object and the bloom volume game object
        effectController = GameObject.Find("----Game Master----").GetComponent<EffectController>();
        bloomVolume = GameObject.Find("Volume");

        // Set up the player's runningTimer and nextRunningTimer
        runningTimer = Time.time;
        nextRunningTimer = nextRunningResetTime;
    }
    // Update is called once per frame
    void Update()
    {
        FaceDirection();
        CharacterMovement();
        CharacterJump();
        CharacterCrouch();
        CharacterHeal();
        CharacterRoll();
    }
    #region  Draw Player's Roll Through
    private void OnDrawGizmos()
    {
        // Draw a sphere at the rollThroughPoint position with a radius of rollThroughRange
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rollThroughPoint.position, rollThroughRange);
    }
    #endregion
    #region  Player's OnCollision or OnTrigger Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //use for detect when the character's Box collider trigger with the "Ground

        if (collision.gameObject.CompareTag("Ground"))  //Check if the player has collided with the objects has tagged "Ground"
        {
            isOnAir = false;
            isDoubleJump = true;

            isJumping = false;
            animator.SetBool(nameof(isJumping), false);

            isOnGround = true;

            isFalling = false;
            animator.SetBool(nameof(isFalling), false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // This method is called when the game object's box collider exits a trigger area associated with another collider in the game world

        if (collision.gameObject.CompareTag("Ground"))  //Check if the player has collided with the objects has tagged "Ground"
        {
            isOnAir = true;
            isJumping = true;
            animator.SetBool(nameof(isJumping), true);

            isOnGround = false;
        }
    }
    #endregion
    #region  Player's FaceDirecton Functions
    public void FaceDirection()
    {
        // Check if the character is currently facing right and moving left
        if (isFacing == true && playerMovement < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);    // Rotate the character 180 degrees to face left
            isFacing = false;   // Set the isFacing flag to false to indicate the character is now facing left
        }
        else if (isFacing == false && playerMovement > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);  // Rotate the character back to its original orientation to face right
            isFacing = true;    // Set the isFacing flag to true to indicate the character is now facing right
        }
        if (isFacing == true)
        {
            facingVal = 1;
        }
        else if (isFacing == false)
        {
            facingVal = -1;
        }
    }
    #endregion
    #region  Player's Movement Functions
    private void CharacterMovement()
    {
        if (movementCheck == true && playerCombat.hasCollided == false)
        {
            // Check if the player is not crouching, has not attacked and has not pushed
            if (isCrouching == false && playerCombatController.hasPunched == false && playerCombatController.hasKicked == false
                && playerCombatController.hasCharged == false && playerCombat.isPushingAway == false && playerCombat.hasCollided == false
                && isHealing == false)
            {
                playerMovement = Input.GetAxisRaw("Horizontal");    // Get the horizontal input axis from the player
                CharacterMove();
            }

            // Check if the player is running
            CharacterRun();
        }
    }
    private void CharacterMove()
    {
        // Check if the player is moving left or right
        if (playerMovement > 0 || playerMovement < 0)
        {
            rb2D.velocity = new Vector2(playerMovement * movementSpeed, rb2D.velocity.y);   // Set the player's horizontal velocity based on its movement speed and input
            isWalking = true;   // Set the isWalking flag to true to indicate that the player is currently walking
            animator.SetBool(nameof(isWalking), true);    // Set the nameof(isWalking) parameter in the animator to true to play the walking animation
        }
        // Check if the player is not moving
        else if (playerMovement == 0)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);    // Set the player's horizontal velocity to zero to stop its movement
            isWalking = false;  // Set the isWalking flag to false to indicate that the player is not walking
            animator.SetBool(nameof(isWalking), false);   // Set the nameof(isWalking) parameter in the animator to false to stop the walking animation
        }
    }
    private void CharacterRun()
    {
        // Check if the player is allowed to run based on the TimeForNextRun()
        if (TimeForNextRun() == true)
        {
            //Check if the Left Shift key is held down
            if (Input.GetKey(KeyCode.LeftShift))    //when holding LeftShift: nexRunningTimer = 0 and if(TimeForEachRun()==true)else just run only one time so move it down to condition if(isRunning == true){}
            {
                isRunning = true;   // If so, set the isRunning flag to true
            }
        }

        // If the Left Shift key is released, reset the isRunning flag and the runningTimer
        if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            isRunning = false;
            runningTimer = 0;
        }

        // If the isRunning flag is true, set the movement speed to the runSpeed and activate the bloom effect
        if (isRunning == true)
        {
            animator.SetBool(nameof(isRunning), true);
            movementSpeed = runSpeed;
            nextRunningTimer = 0;

            // Check if the time limit for running has been reached
            if (TimeForEachRun() == true)
            {
                isRunning = true;   // If so, set the isRunning flag to true to allow the player to keep running
            }
            else isRunning = false; // Otherwise, set the isRunning flag to false to stop the player from running

            bloomVolume.SetActive(true);
        }
        // If the isRunning flag is false, set the movement speed to the walkSpeed and deactivate the bloom effect
        else if (isRunning == false)
        {
            animator.SetBool(nameof(isRunning), false);
            movementSpeed = walksSpeed;
            bloomVolume.SetActive(false);
        }
    }
    private bool TimeForEachRun()
    {
        /** This method updates a runningTimer and checks if it has exceeded a runningResetTime.
          * Returns true if the runningTimer has not exceeded the runningResetTime, false otherwise. **/

        runningTimer += Time.deltaTime; // Update the runningTimer with the elapsed time since the last frame

        // Check if the runningTimer has not exceeded the runningResetTime
        if (runningTimer <= runningResetTime)
        {
            return true;     // Return true if the runningTimer is still within the runningResetTime
        }
        else return false;   // Return false if the runningTimer has exceeded the runningResetTime
    }
    private bool TimeForNextRun()
    {
        /** This method updates a nextRunningTimer for the next run and checks if it has reached a nextRunningResetTime.
          * Returns true if the nextRunningTimer for the next run has reached the nextRuningResetTime, false otherwise. **/

        nextRunningTimer += Time.deltaTime; // Update the timer for the next run with the elapsed time since the last frame

        // Check if the timer for the next run has reached the reset time
        if (nextRunningTimer >= nextRunningResetTime)
        {
            return true;    // Return true if the timer for the next run has reached the reset time
        }
        else return false;  // Return false if the timer for the next run has not yet reached the reset time
    }
    #endregion
    #region  Player's Jump Functions
    private void CharacterJump()
    {

        // This method controls the character's jump behavior based on player input and game conditions.

        // Check if the jump key is pressed and the character is not currently jumping, crouch-kicking, or charging a special attack and has not been pushed away
        if (Input.GetKeyDown(KeyCode.Space) && isHealing == false && !isJumping && playerCombatController.hasCrouchKicked == false
        && playerCombatController.hasCharged == false && playerCombat.isPushingAway == false)
        {
            // Set the character's upward velocity to the jump force and spawn a jump effect
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            Vector3 spawnJumpEffectPos = gameObject.transform.position - new Vector3(0, 1.5f, 0);
            SpawnJumpEffect(jumpEffectPrefab, spawnJumpEffectPos, 0.9f);
        }
        // Check if the jump key is pressed while the character is already jumping and has a double jump available
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == true && isDoubleJump == true && isRunning == true)
        {
            // Set the character's upward velocity to the double jump force, spawn a double jump effect, and update animation variables
            rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpForce);
            Vector3 spawnDoubleJumpEffectPos = gameObject.transform.position - new Vector3(0, 2f, 0);
            SpawnJumpEffect(doubleJumpEffectPrefab, spawnDoubleJumpEffectPos, 0.3f);
            isDoubleJump = false;
            isFalling = false;
            animator.SetBool(nameof(isFalling), false);
            animator.SetBool(nameof(isJumping), true);
        }
        // Check if the character is falling after jumping
        if (rb2D.velocity.y < 0 && isJumping == true)
        {
            // Update animation variables to reflect the character's falling state
            isFalling = true;
            animator.SetBool(nameof(isFalling), true);
            animator.SetBool(nameof(isJumping), false);
        }
        else if (rb2D.velocity.y > 0 && isJumping == false)
        {
            // Update animation variables to reflect the character's falling state
            isFalling = false;
            animator.SetBool(nameof(isFalling), false);
        }
    }
    private void SpawnJumpEffect(GameObject typeJumpEffect, Vector3 spawnPos, float destroyEffectTime)
    {

        /* This function spawns a jump effect at a given position and sets a timer for it to be destroyed.
         * Parameters:
             typeJumpEffect: The game object that represents the jump effect to be spawned.
             spawnPos: The position in the world where the jump effect will be spawned.
             destroyEffectTime: The time (in seconds) after which the jump effect will be destroyed.
             Return value: None
         * Example usage:
             SpawnJumpEffect(jumpEffectPrefab, transform.position, 1.5f); */

        GameObject spawnJumpEffect = Instantiate(typeJumpEffect, spawnPos, Quaternion.identity);    // Creates a new instance of a game object and assigns it to the variable "spawnJumpEffect"
        StartCoroutine(effectController.DestroyEffect(spawnJumpEffect, destroyEffectTime)); // Starts a coroutine called "DestroyEffect" that belongs to the "effectController" script.
    }
    #endregion
    #region  Player's Crouch Functions
    private void CharacterCrouch()
    {
        // This method checks for a specific input from and sets boolean variables, animation states for the gameObject to make the player Crouching

        // Check if the player has pressed and is holding down the "Down Arrow" key, and if the game object is currently on the ground
        if (Input.GetKey(KeyCode.DownArrow) && isOnGround == true && playerCombatController.hasPunched == false && playerCombatController.hasKicked == false)
        {
            isCrouching = true;
            animator.SetBool(nameof(isCrouching), true);

            isWalking = false;
            animator.SetBool(nameof(isWalking), false);
        }
        else
        {
            isCrouching = false;
            animator.SetBool(nameof(isCrouching), false);
        }
    }
    #endregion
    #region  Player's Roll Functions
    private void CharacterRoll()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isCrouching == true && isRolling == false && isOnGround == true && playerCombat.hasCollided == false && playerCombat.isPushingAway == false && playerCombatController.hasCrouchKicked == false)
        {
            Debug.Log("s");
            isRolling = true;
            animator.SetBool(nameof(isRolling), true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rb2D.velocity = new Vector2(1, 0) * facingVal * rollingForce;
            Invoke(nameof(SetIsRollingToFalse), .5f);
        }
        if (isRolling == true)
        {
            CharacterRollThroughEnemy();
        }
        else if (isRolling == false)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            rb2D.gravityScale = 5;
        }
    }
    private void SetIsRollingToFalse()
    {
        if (isRolling == true)
        {
            isRolling = false;
            animator.SetBool(nameof(isRolling), false);
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    private void CharacterRollThroughEnemy()
    {
        //Debug.Log(hitEnemy.Length);
        hitEnemy = Physics2D.OverlapCircleAll(rollThroughPoint.position, rollThroughRange, enemyLayer);
        if (hitEnemy.Length == 0)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            rb2D.gravityScale = 5;
        }
        foreach (Collider2D enemy in hitEnemy)
        {
            if (enemy.CompareTag("Enemy"))
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                rb2D.gravityScale = 0;
            }
        }
    }
    #endregion
    #region  Player's Heal Functions
    private void CharacterHeal()
    {
        if (Input.GetKeyDown(KeyCode.R) && characterData.CurrentHealingChargeValue == characterData.MaxHealingChargeValue
        && isOnGround == true && playerCombatController.hasPunched == false && playerCombatController.hasKicked == false)
        {
            isHealing = true;
            animator.SetBool(nameof(isHealing), true);
            characterData.CurrentHealthValue += 30;
            characterData.CurrentHealingChargeValue = 0;
            Invoke(nameof(SetIsHealingToFalse), 1f);
        }
    }
    private void SetIsHealingToFalse()
    {
        isHealing = false;
        animator.SetBool(nameof(isHealing), false);
    }
    #endregion
}