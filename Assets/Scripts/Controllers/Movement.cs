using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("GameObjects")]
    private GameObject player;
    [SerializeField] private GameObject shadowPF;
    [SerializeField] private GameObject shadowSpawnEffectPF;
    [SerializeField] private GameObject shadowDeathEffectPF;
    [SerializeField] private GameObject playerDeathEffectPF;

    [Header("Movement")]
    [SerializeField] private float speed = 7;
    [SerializeField] private float horizontalSlowdownSpeed = 0.1f;
    [SerializeField] private float jumpStrength = 7;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask platformLayermask;

    [Header("Shadow Variables")]
    [SerializeField] private float shadowSpawnDelay = 2;

    //Shadow
    private float numShadowsSpawning = 0;
    private bool firstMove = false;
    private List<ShadowInfo> shadows = new List<ShadowInfo>();
    private List<Vector2Int> inputs = new List<Vector2Int>();

    private LevelLoader levelLoader;

    private List<ShadowInfo> toBeDestroyed = new List<ShadowInfo>();

    [SerializeField] private AudioManager audioManagerPF;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = Instantiate(audioManagerPF);
        player = FindObjectOfType<PlayerCollisions>().gameObject;
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private bool shadowDead = false;

    private void FixedUpdate()
    {
        #region Move Shadows
        foreach (ShadowInfo shadowInfo in shadows)
        {
            if (shadowInfo == null) { continue; }

            GameObject shadow = shadowInfo.shadow;

            if (CheckCruchDeath(shadow, shadowDeathEffectPF))
            {
                audioManager.PlayDeathSound();

                GameObject deathEffect = Instantiate(shadowDeathEffectPF);
                deathEffect.transform.position = shadow.transform.position;

                shadow.SetActive(false);
                shadowDead = true;
            }

            if (shadowInfo.InputFrame == inputs.Count)
            {
                if (!toBeDestroyed.Contains(shadowInfo))
                {
                    StartCoroutine(RemoveShadow(shadowInfo));
                    toBeDestroyed.Add(shadowInfo);
                }

                StopHorizontalMovement(shadow, horizontalSlowdownSpeed);
                continue;
            }

            Vector2Int input = inputs[shadowInfo.InputFrame];
            shadowInfo.InputFrame++;

            if (input.x == -1)
            {
                MoveLeft(shadow, speed);
            }
            else if (input.x == 1)
            {
                MoveRight(shadow, speed);
            }
            else
            {
                StopHorizontalMovement(shadow, horizontalSlowdownSpeed);
            }

            if (input.y == 1)
            {
                Jump(shadow, jumpStrength);
            }
        }
        
        if (shadowDead)
            shadows = new List<ShadowInfo>();
        #endregion

        PlayerCollisions playerCollisions = player.GetComponent<PlayerCollisions>();

        if (playerCollisions.IsDead)
        {
            levelLoader.ReloadLevel();
            return;
        }
        else if (playerCollisions.ReachedPortal)
        {
            if (shadows.Count == 0 && numShadowsSpawning == 0)
            {
                levelLoader.NextLevel();
            }
            StopHorizontalMovement(player, horizontalSlowdownSpeed);
            return;
        }

        #region Record and control player movements
        if (CheckCruchDeath(player, playerDeathEffectPF))
        {
            audioManager.PlayDeathSound();
            levelLoader.ReloadLevel();
        }

        int xInput = 0;
        int yInput = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft(player, speed);
            TryFirstMovement();
            xInput = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight(player, speed);
            TryFirstMovement();
            xInput = 1;
        }
        else
        {
            StopHorizontalMovement(player, horizontalSlowdownSpeed);
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Jump(player, jumpStrength);
            TryFirstMovement();
            yInput = 1;
        }

        if (firstMove)
        {
            inputs.Add(new Vector2Int(xInput, yInput));
        }
        #endregion
    }

    private bool CheckCruchDeath(GameObject gameObject, GameObject deathEffectPF)
    {
        if (IsCrushed(gameObject) && gameObject.active)
        {
            print("Player Died");
            GameObject deathEffect = Instantiate(deathEffectPF);
            deathEffect.transform.position = gameObject.transform.position;
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    private void TryFirstMovement()
    {
        if (!firstMove)
        {
            firstMove = true;
            StartCoroutine(StartShadowSpawn());
        }
    }

    private IEnumerator StartShadowSpawn()
    {
        numShadowsSpawning++;

        GameObject spawnEffect = Instantiate(shadowSpawnEffectPF);
        spawnEffect.transform.position = player.transform.position;

        Vector3 spawnLoc = player.transform.position;

        yield return new WaitForSeconds(shadowSpawnDelay);

        GameObject shadow = Instantiate(shadowPF, spawnLoc, Quaternion.identity);
        shadows.Add(new ShadowInfo(shadow));
        
        numShadowsSpawning--;
    }

    private IEnumerator RemoveShadow(ShadowInfo shadowInfo)
    {
        yield return new WaitForSeconds(1);

        if (shadowInfo.shadow.active == false) { yield return null; }
        audioManager.PlayDeathSound();
        GameObject deathEffect = Instantiate(shadowDeathEffectPF);
        deathEffect.transform.position = shadowInfo.shadow.transform.position;

        shadowInfo.shadow.SetActive(false);
        shadows.Remove(shadowInfo);
    }

    private bool IsCrushed(GameObject gameObject)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        //if not touching ground, don't jump
        RaycastHit2D platformInPlayer = Physics2D.BoxCast(collider.bounds.center, new Vector2(collider.size.x * 0.3f, collider.size.y * 0.3f), 0f, Vector2.down, 0, platformLayermask);

        if (platformInPlayer.collider == null) { return false; }
        return true;
    }

    #region Movement Commands
    private void ClimbStair(GameObject gameObject)
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D touchingGround = Physics2D.BoxCast(collider.bounds.center, new Vector2(collider.size.x * 0.8f, collider.size.y), 0f, Vector2.down, 0.1f, platformLayermask);

        StairClimbingColliders stairClimbingColliders = gameObject.GetComponentInChildren<StairClimbingColliders>();
        if (touchingGround.collider!=null && stairClimbingColliders.CanClimbStairs)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.26f, 0);
        }
    }

    private void Jump(GameObject gameObject, float jumpStrength)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        //if not touching ground, don't jump
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, new Vector2(collider.size.x * 0.8f, collider.size.y), 0f, Vector2.down, 0.1f, platformLayermask);
        
        if(hit.collider == null) { return; }

        //set velocity
        if (gameObject == player)
            audioManager.PlayJumpSound();
        rb.velocity = new Vector3(rb.velocity.x, jumpStrength, 0);
    }

    private void MoveLeft(GameObject gameObject, float speed)
    {
        ClimbStair(gameObject);
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        //set velocity
        rb.velocity = new Vector3(-speed, rb.velocity.y, 0);

        //reverse character
        gameObject.GetComponent<Transform>().localScale = new Vector3(-1, 1, 1); ;
    }

    private void MoveRight(GameObject gameObject, float speed)
    {
        ClimbStair(gameObject);
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        //set velocity
        rb.velocity = new Vector3(speed, rb.velocity.y, 0);

        //reverse character
        gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    }

    private void StopHorizontalMovement(GameObject gameObject, float slowdown)
    {
        ClimbStair(gameObject);
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        //set velocity
        float xVelocity = rb.velocity.x / (1+slowdown);

        if(Mathf.Abs(xVelocity) < 0.1)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        else
            rb.velocity = new Vector3(xVelocity, rb.velocity.y, 0);
    }
    #endregion
}

public class ShadowInfo
{
    public GameObject shadow;
    public int InputFrame;

    public ShadowInfo(GameObject _shadow)
    {
        shadow = _shadow;
    }
}