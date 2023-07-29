using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    // REFERENCES to other components and objects in the game.
    private EffectController effectController; // Reference to the EffectController script
    private PlayerController playerController; // Reference to the PlayerController script
    private PlayerCombat playerCombat; // Reference to the PlayerCombat script
    private Animator animator; // Reference to the Animator component
    public GameObject[] punchEffect; // An array of punch effect game objects

    // VARIABLES used by the script.
    [HideInInspector]
    public float punchTimer; // A timer for the punch action

    public float punchResetTime = 1.5f; // The time it takes for the punch action to reset
    public bool hasCharged = false; // Whether the player has charged their punch
    public bool hasPunched = false; // Whether the player has punched
    public bool hasPowerPunched = false; // Whether the player has power punched
    public bool hasNormalPunched = false; // Whether the player has normal punched

    private GameObject spawnPunchEffect; // A reference to the punch effect game object being spawned
    private Vector3 spawnPunchEffectPos1; // The position of the first punch effect
    private Vector3 spawnPunchEffectPos2; // The position of the second punch effect
    private Vector3 spawnPunchEffectPos3; // The position of the third punch effect
    private bool hasSpawnPunchEffect3 = false; // Whether the third punch effect has been spawned

    public bool hasKicked = false; // Whether the player has kicked
    public bool hasFlyKicked = false; // Whether the player has performed a fly kick
    public bool hasFlyingKicked = false; // Whether the player has performed a flying kick
    public bool hasCrouchKicked = false; // Whether the player has performed a crouching kick

    public Transform kickAttackPoint; // The point from which the kick attack originates
    public Transform powerPunchAttackPoint; // The point from which the power punch attack originates
    public Vector3 powerPunchAttackSize;    // The size of the power punch attack
    public float attackRange; // The range of the player's attacks
    public LayerMask enemyLayers; // The layers on which enemies are located
    // Start is called before the first frame update
    void Start()
    {
        effectController = GameObject.Find("----Game Master----").GetComponent<EffectController>();
        playerController = GetComponent<PlayerController>();
        playerCombat = GetComponent<PlayerCombat>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        PlayerPunch();
        PlayerKick();
    }
    #region  Draw Player's Attack Points
    void OnDrawGizmosSelected()
    {
        /** Draws gizmos in the Unity editor to represent the attack range for the kick and punch attacks **/
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(kickAttackPoint.position, attackRange);   // Draw a wire sphere around the kick attack point to represent the attack range
        Gizmos.DrawWireCube(powerPunchAttackPoint.position, powerPunchAttackSize);  // Draw a wire cube around the power punch attack point to represent the attack range
    }
    #endregion
    #region  Player's Punch Functions
    private void PlayerPunch()
    {
        /** Conditions to make player punch **/

        // Charge up punch by holding down W key
        if (Input.GetKey(KeyCode.W) && !hasPunched && !hasCharged && !hasKicked
            && !hasFlyKicked && !hasFlyingKicked && !hasCrouchKicked
            && !playerController.isJumping && !playerController.isFalling && !playerCombat.isPushingAway
            && !playerController.isHealing && !playerController.isRolling && !playerController.isLedgeClimbing
            && !playerCombat.hasCollided && !playerController.isWallClimbing && !playerController.isLedgeClimbing)
        {
            hasCharged = true;
            animator.SetBool(nameof(hasCharged), true);
        }
        // Spawn punch effect 3 when player velocity is low and punch is charged
        if (hasCharged)
        {
            if (playerController.rb2D.velocity.x > -0.5f && playerController.rb2D.velocity.x < 0.5f && hasSpawnPunchEffect3 == false)
            {
                SpawnEffect(3);
                hasSpawnPunchEffect3 = true;
            }
        }
        // Release punch when W key is released and punch is charged
        if (Input.GetKeyUp(KeyCode.W) && hasCharged && !playerCombat.isPushingAway)
        {
            // Destroy punch effect 3 and reset variables

            GameObject punchEffect3 = GameObject.Find("Hero_PunchImpact_3(Clone)");
            Destroy(punchEffect3);
            hasSpawnPunchEffect3 = false;

            hasCharged = false;
            animator.SetBool(nameof(hasCharged), false);

            hasPunched = true;
            animator.SetBool(nameof(hasPunched), true);

            // Determine if punch is normal or power punch based on punch timer
            if (punchTimer >= punchResetTime)
            {
                hasPowerPunched = true;

                // Spawn punch effects 1 and 2 for power punch
                SpawnEffect(1);
                SpawnEffect(2);
            }
            else if (punchTimer < punchResetTime)
            {
                hasNormalPunched = true;
            }
            punchTimer = 0; //reset punchTimer when release W key
        }

        // Attack enemies when power or normal punch is active and no enemies are hit yet, and also end punching after a delay
        if (hasPowerPunched && playerCombat.hitEnemiesList.Count == 0)
        {
            playerCombat.Attack2(powerPunchAttackPoint, powerPunchAttackSize, 0, enemyLayers);
            AddHitEnemiesToList();  // Add hit enemies to the list

            animator.SetFloat("Punch Speed", 0.5f);
            Invoke(nameof(EndPunch), 0.8f);
        }
        if (hasNormalPunched && playerCombat.hitEnemiesList.Count == 0)
        {
            playerCombat.Attack1(kickAttackPoint, 1.8f, enemyLayers);
            AddHitEnemiesToList();

            animator.SetFloat("Punch Speed", 1f);
            Invoke(nameof(EndPunch), 0.4f);
        }

        // Increment punchTimer when punch is charging and player is not jumping or falling
        if (hasCharged && !playerController.isJumping && !playerController.isFalling)
        {
            punchTimer += Time.deltaTime;
        }

        // Reset punchTimer and cancel charge when player jumps
        if (playerController.isJumping)
        {
            punchTimer = 0;
            hasCharged = false;
            animator.SetBool(nameof(hasCharged), false);
        }

        //Reset punchTimer and destroy punch effect 3
        if (playerCombat.isPushingAway)
        {
            punchTimer = 0;
            hasCharged = false;
            animator.SetBool(nameof(hasCharged), false);

            GameObject punchEffect3 = GameObject.Find("PunchImpact_3(Clone)");
            Destroy(punchEffect3);
            hasSpawnPunchEffect3 = false;
        }
    }
    private void SetUpPunchEffects()
    {
        /** This function sets up the positions where punch effects will be spawned based on the player's facing direction **/

        // If the player is facing right or left, set the punch effect positions accordingly
        if (playerController.isFacing)
        {
            spawnPunchEffectPos1 = gameObject.transform.position + new Vector3(2f, -1.6f, 0);
            spawnPunchEffectPos2 = gameObject.transform.position + new Vector3(5, 0, 0);
            spawnPunchEffectPos3 = gameObject.transform.position + new Vector3(-1.55f, -0.8f, 0);
        }
        else if (!playerController.isFacing)
        {
            spawnPunchEffectPos1 = gameObject.transform.position - new Vector3(2f, 1.6f, 0);
            spawnPunchEffectPos2 = gameObject.transform.position - new Vector3(5, 0, 0);
            spawnPunchEffectPos3 = gameObject.transform.position - new Vector3(-1.55f, 0.8f, 0);
        }
    }
    private void SpawnEffect(int effect)
    {
        /** This method is responsible for spawning punch effects based on the specified effect type **/

        // Call the SetUpPunchEffects method to set up the punch effects
        SetUpPunchEffects();

        // Use a switch statement to determine which punch effect to spawn based on the effect type
        switch (effect)
        {
            case 1: // If effect type is 1, spawn the first punch effect at spawnPunchEffectPos1 with a scale of 0.8f
                SpawnPunchEffect(punchEffect[0], spawnPunchEffectPos1, 0.8f);
                break;
            case 2: // If effect type is 2, spawn the second punch effect at spawnPunchEffectPos2 with a scale of 0.6f
                SpawnPunchEffect(punchEffect[1], spawnPunchEffectPos2, 0.6f);
                break;
            case 3: // If effect type is 3, spawn the third punch effect at spawnPunchEffectPos3 with an infinite scale
                SpawnPunchEffect(punchEffect[2], spawnPunchEffectPos3, Mathf.Infinity);
                break;
        }
    }
    private void SpawnPunchEffect(GameObject typeOfEffect, Vector3 spawnPos, float destroyEffectTime)
    {
        /* This function spawns a punch effect at the specified position with the specified destroy time. 
         * Parameters
             typeOfEffect The prefab of the effect to spawn.
             spawnPos The position at which to spawn the effect.
             destroyEffectTime The time after which the effect should be destroyed.
         * Return value: None
         * Example usage:
             SpawnPunchEffect(punchEffect[0], spawnPunchEffectPos1, 0.8f); */

        spawnPunchEffect = Instantiate(typeOfEffect, spawnPos, transform.rotation); // Creates a new instance of a game object and assigns it to the variable "spawnPunchEffect"
        StartCoroutine(effectController.DestroyEffect(spawnPunchEffect, destroyEffectTime));    // Starts a coroutine called "DestroyEffect" that belongs to the "effectController" script
    }
    #endregion
    #region  Player's Kick Functions
    private void PlayerKick()
    {
        /** This function is called when the player presses the "E" key, and triggers a kick attack if certain conditions are met. **/

        // Check if the "E" key is pressed and the player is not currently performing any other attacks.
        if (Input.GetKeyDown(KeyCode.E) && !hasPunched && !hasCharged && !hasKicked
            && !hasFlyKicked && !hasFlyingKicked && !hasCrouchKicked
            && !playerCombat.isPushingAway && !playerController.isHealing && !playerController.isRolling
            && !playerCombat.hasCollided && !playerController.isWallClimbing && !playerController.isLedgeClimbing)
        {
            punchTimer = 0; //Reset punchTimer

            // If the player is not jumping or crouching, perform a regular kick attack.
            if (!playerController.isJumping && !playerController.isCrouching)
            {
                hasKicked = true;
                animator.SetBool(nameof(hasKicked), true);
                Invoke(nameof(EndKick), 0.5f);
            }
            // If the player is jumping and not walking, perform a flying kick attack.
            else if (playerController.isJumping && !playerController.isWalking)
            {
                hasFlyKicked = true;
                animator.SetBool(nameof(hasFlyKicked), true);
                Invoke(nameof(EndFlyKick), 0.5f);
            }
            // If the player is jumping and walking, perform a running flying kick attack.
            else if (playerController.isJumping && playerController.isWalking)
            {
                hasFlyingKicked = true;
                animator.SetBool(nameof(hasFlyingKicked), true);
                Invoke(nameof(EndFlyingKick), 0.5f);
            }
            // If the player is crouching and not jumping, perform a crouch kick attack.
            else if (playerController.isCrouching && !playerController.isJumping)
            {
                hasCrouchKicked = true;
                animator.SetBool(nameof(hasCrouchKicked), true);
                Invoke(nameof(EndCrouchKick), 0.5f);
            }
        }

        // If the player successfully performed a kick attack and did not hit any enemies, attempt to hit enemies within the attack range.
        if ((hasKicked || hasFlyKicked || hasFlyingKicked || hasCrouchKicked) && playerCombat.hitEnemiesList.Count == 0)
        {
            playerCombat.Attack1(kickAttackPoint, 1.8f, enemyLayers);
            AddHitEnemiesToList();
        }
    }
    #endregion
    #region  Player's Add and Remove hit enemies when attacked them
    private void AddHitEnemiesToList()
    {
        /** This function adds the enemies hit by the player's attack to a list, to avoid Attack() function constantly call when collide with Enemy **/

        for (int i = 0; i < playerCombat.hitEnemies.Length; i++)
        {
            playerCombat.hitEnemiesList.Add(playerCombat.hitEnemies[i]);
        }
    }
    public void RemoveHitEnemiesFromList()
    {
        /** This function removes the enemies hit by the player's attack to a list. **/

        for (int i = playerCombat.hitEnemiesList.Count - 1; i >= 0; i--)
        {
            playerCombat.hitEnemiesList.Remove(playerCombat.hitEnemiesList[i]);
        }
    }
    #endregion
    #region  Player's End Combat Functions
    private void EndCombat(string combatName, ref bool combatCondition)
    {
        /** Ends a combat sequence by removing hit enemies from the list, setting the combat condition to false, and disabling the animator parameter for the combat **/

        RemoveHitEnemiesFromList();
        combatCondition = false;
        animator.SetBool(combatName, false);
    }
    private void EndPunch()
    {
        /** Set the combat condition to false **/

        hasNormalPunched = false;
        hasPowerPunched = false;
        EndCombat(nameof(hasPunched), ref hasPunched);
    }
    private void EndKick()
    {
        /** Ends a kick by calling EndCombat for hasKicked **/

        EndCombat(nameof(hasKicked), ref hasKicked);
    }
    private void EndFlyKick()
    {
        /** Ends a fly kick by calling EndCombat for hasFlyKicked **/

        EndCombat(nameof(hasFlyKicked), ref hasFlyKicked);
    }
    private void EndFlyingKick()
    {
        /** Ends a flying kick by calling EndCombat for hasFlyingKicked **/

        EndCombat(nameof(hasFlyingKicked), ref hasFlyingKicked);
    }
    private void EndCrouchKick()
    {
        /** Ends a crouch kick by calling EndCombat for hasCrouchKicked **/
        EndCombat(nameof(hasCrouchKicked), ref hasCrouchKicked);
    }
    #endregion
}
