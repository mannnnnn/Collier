using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public enum State
    {
        FALL = 0, WALL = 1, CUT = 2, STAND = 3, HURT = 4
    }
    State state = State.STAND;

    public float maxCutLength = 8;
    public float minCutLength = 3;

    BoxCollider2D box;

    public GameObject cut;
    CutAnimation currentCut;
    Vector2 cutTarget;
    AudioSource dio;
    Rigidbody2D rb;

    public GameObject leftWall;
    public GameObject rightWall;

    public float driftSpeed = 3;
    //SOUNDS
    public AudioClip sword1;
    public AudioClip sword2;
    public AudioClip sword3;
    public AudioClip impact1;
    public AudioClip impact2;
    public AudioClip impact3;
    public AudioClip oof;
    public AudioClip swish;
    Animator anim;
    SpriteRenderer sr;

    bool damaged = false;
    float damageTimer = 0f;
    float damageDuration = 1f;

    float invframesTimer = 0f;
    float invFramesDur = 0.1f;
    bool inv = false;

    public GameObject pain;

    bool dead = false;
    bool win = false;
    float timer = 0f;
    float duration = 1f;

    int prevWall = 0;

    public float spdCap = 10f;

    // updated by Walled()
    public int wallDirection = 0;

	// Use this for initialization
	void Start () {
        dio = GetComponent<AudioSource>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("Walled:" + Walled());
        Debug.Log("Impact:" + prevWall);
        if (prevWall != wallDirection && Walled()){
            int soundSwitch = UnityEngine.Random.Range(1, 4);
            Debug.Log("Playing Impact");
            switch(soundSwitch){
                case(1): dio.clip = impact1;break;
                case(2): dio.clip = impact2;break;
                case(3): dio.clip = impact3;break;
                default: dio.clip = impact3; break;
            }
            dio.Play();
            ParticleSystem dust =  GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule dustShape = dust.shape;
            if(GetSide() == -1){
            dustShape.rotation = new Vector3(0,0,270);
            dust.Play();
            } else {
            dustShape.rotation = new Vector3(0,0,90);
            dust.Play();
            }
            
        }
        prevWall = wallDirection;


        if (dead || win)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                GameObject.FindGameObjectWithTag("ExitButton")?.SetActive(false);
                if (dead)
                {
                    // pull the trigger piglet
                    GameObject ui = GameObject.FindGameObjectWithTag("UI");
                    ui.GetComponentInChildren<Defeat>(true).gameObject.SetActive(true);
                    Destroy(gameObject);
                }
                if (win)
                {
                    GameObject ui = GameObject.FindGameObjectWithTag("UI");
                    ui.GetComponentInChildren<Victory>(true).gameObject.SetActive(true);

                    bool unlockNext = false;
                    for (int i = 1; i <= SaveLoad.LEVELS; i++)
                    {
                        for (int j = 1; j <= SaveLoad.STAGES; j++)
                        {
                            string key = $"Level_{i}_{j}";
                            if (unlockNext)
                            {
                                SaveLoad.levelUnlocked[key] = Math.Max(0, SaveLoad.levelUnlocked[key]);
                                PlayerPrefs.SetInt(key, Math.Max(0, SaveLoad.levelUnlocked[key]));
                                unlockNext = false;
                            }
                            if (key == SceneManager.GetActiveScene().name)
                            {
                                unlockNext = true;
                            }
                        }
                    }
                }

            }
        }
        Vector2 pos = transform.position;
        if (rb.velocity.y < -spdCap)
        {
            rb.velocity = new Vector2(rb.velocity.x, -spdCap);
        }
        if (damaged && rb.velocity.y < -spdCap * 0.6f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -spdCap * 0.6f);
        }
        anim.SetInteger("State", (int)state);
        if (GetSide() < 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        RaycastHit2D? hit = Raycast((Vector2)transform.position
            + Vector2.down * box.bounds.extents.y * 0.5f, "Hazard");
        RaycastHit2D? enemyHitDown = Raycast((Vector2)transform.position
            + Vector2.down * box.bounds.extents.y * 0.5f, "Enemy");
        RaycastHit2D? enemyHitLeft = Raycast((Vector2)transform.position
           + Vector2.left * box.bounds.extents.y * 1f, "Enemy");
        RaycastHit2D? enemyHitRight = Raycast((Vector2)transform.position
           + Vector2.right * box.bounds.extents.y * 1f, "Enemy");
        damageTimer = Mathf.MoveTowards(damageTimer, 0, Time.deltaTime);
        invframesTimer = Mathf.MoveTowards(damageTimer, 0, Time.deltaTime);
        if (hit != null || enemyHitDown != null || enemyHitLeft != null || enemyHitRight != null)
        {
            Damage((hit ?? enemyHitDown ?? enemyHitLeft ?? enemyHitRight).Value);
        }
        // if not in contact with obstacle, decrement invincibilty timer
        else
        {
            if (damaged && state == State.WALL)
            {
                invframesTimer = invFramesDur;
                inv = true;
            }
        }
        if (damaged && inv && invframesTimer == 0f)
        {
            inv = false;
            damaged = false;
        }

        switch (state)
        {
            case State.FALL:
                // go towards a wall
                if (GetSide() < 0)
                {
                    rb.position = new Vector2(rb.position.x - (driftSpeed * Time.deltaTime), rb.position.y);
                }
                else
                {
                    rb.position = new Vector2(rb.position.x + (driftSpeed * Time.deltaTime), rb.position.y);
                }
                // until we touch a wall or hit the ground
                if (Grounded())
                {
                    state = State.STAND;
                }
                if (Walled())
                {
                    state = State.WALL;
                }
                break;
            case State.WALL:
                // animation down wall
                // check for cuts
                if (TouchInput.GetSwipe() != null)
                {
                    TryCut(TouchInput.GetSwipe().direction);
                }
                if (!Walled())
                {
                    state = State.FALL;
                }
                if (Grounded())
                {
                    state = State.STAND;
                }
                break;
            case State.CUT:
                // play animation, then fall
                if (currentCut == null)
                {
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(cutTarget.x, cutTarget.y, transform.position.z);
                    state = State.FALL;
                }
                // if currently in cut animation, check for obstacles
                else
                {
                    KillEnemies(transform.position, currentCut.position);
                    RaycastHit2D? cutHit = Raycast(currentCut.position, "Hazard");
                    if (cutHit != null)
                    {
                        transform.position = new Vector3(currentCut.position.x,
                            currentCut.position.y, transform.position.z);
                        Damage(cutHit.Value);
                        transform.position = new Vector3(currentCut.position.x,
                            currentCut.position.y, transform.position.z);
                    }
                }
                break;
            case State.STAND:
                // can only swipe one way
                if (TouchInput.GetSwipe() != null)
                {
                    TryCut(TouchInput.GetSwipe().direction);
                }
                if (!Grounded())
                {
                    state = State.FALL;
                }
                break;
            case State.HURT:
                if (damageTimer == 0)
                {
                    state = State.FALL;
                }
                if (Grounded())
                {
                    state = State.STAND;
                }
                break;
        }
        if (state == State.CUT)
        {
            rb.velocity = Vector2.zero;
        }
	}

    void KillEnemies(Vector2 start, Vector2 end)
    {
        Vector2 pos = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, (end - start).normalized,
            (end - start).magnitude, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                int soundSwitch = UnityEngine.Random.Range(1, 4);
                switch(soundSwitch){
                    case(1): dio.clip = sword1;break;
                    case(2): dio.clip = sword2;break;
                    case(3): dio.clip = sword3;break;
                    default: dio.clip = sword3; break;
                }

                dio.Play();
               ParticleSystem explode =  hit.collider.gameObject.GetComponent<ParticleSystem>();
               explode.Play();
               hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                float totalDuration = explode.duration + explode.startLifetime;
                Destroy(hit.collider.gameObject.GetComponentInChildren<BoxCollider2D>());
                Destroy(hit.collider.gameObject, totalDuration);
                Coins coins = GameObject.FindGameObjectWithTag("Coins").GetComponent<Coins>();
                coins.coins++;
            }
        }
    }

    void Damage(RaycastHit2D hit)
    {
        Health health = GameObject.FindGameObjectWithTag("Health").GetComponent<Health>();
        if (!damaged && health.health > 0 && state != State.CUT)
        {
            damageTimer = damageDuration;
            damaged = true;
            dio.clip = oof;
            dio.Play();
            // set player to move away from obstacle
            rb.velocity = new Vector2(5f * -GetSide(),
                5f * Mathf.Max(Mathf.Sign(transform.position.y * hit.collider.transform.position.y), 0));
            state = State.HURT;
            // sparks to indicate pain
            Instantiate(pain, transform.position + Vector3.back, transform.rotation)
                .GetComponent<Pain>().Initialize(hit.collider.gameObject);
            // decrement health
            health.health--;
            if (health.health <= 0)
            {
                // game over
                box.isTrigger = true;
                dead = true;
            }
        }
    }

    // -1 for left, 1 for right
    int GetSide()
    {
        float left = leftWall.GetComponent<BoxCollider2D>().bounds.max.x;
        float right = rightWall.GetComponent<BoxCollider2D>().bounds.min.x;
        float leftDist = Mathf.Abs(transform.position.x - left);
        float rightDist = Mathf.Abs(transform.position.x - right);
        if (leftDist < rightDist)
        {
            return -1;
        }
        return 1;
    }

    bool TryCut(Vector2 direction)
    {
        if (dead || win)
        {
            return false;
        }
        // we check a bit beyond where we want to land
        float dist = maxCutLength + box.bounds.extents.x;
        RaycastHit2D? raycast = Raycast((Vector2)transform.position +
            direction * dist, "Wall", true);
        // if no collisions, go to target position
        if (raycast == null)
        {
            Vector2 end = (Vector2)transform.position +
                direction * maxCutLength;
            Cut(transform.position, end);
            return true;
        }
        // otherwise if far enough away, go to it
        else if ((raycast.Value.point - (Vector2)transform.position).magnitude > minCutLength)
        {
            float freeDist = Mathf.MoveTowards((raycast.Value.point - (Vector2)transform.position).magnitude,
                0, box.bounds.extents.x);
            Vector2 end = (Vector2)transform.position + direction * freeDist;
            Cut(transform.position, end);
            return true;
        }
        return false;
    }

    // start the cut animation
    void Cut(Vector2 start, Vector2 end)
    {
        dio.clip = swish;
        dio.Play();
        state = State.CUT;
        rb.velocity = Vector2.zero;
        cutTarget = end;
        currentCut = Instantiate(cut).GetComponent<CutAnimation>();
        currentCut.Initialize(start, end);
    }

    // find closest physics collision with player when attempting to go to target
    public RaycastHit2D? Raycast(Vector2 target, string layer = "Wall", bool ignorePlatform = false)
    {
        Vector2 pos = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, (target - pos).normalized,
            (target - pos).magnitude, LayerMask.GetMask(layer));
        // return closest hit
        float minDist = (target - pos).magnitude;
        RaycastHit2D? min = null;
        foreach (RaycastHit2D hit in hits)
        {
            if ((hit.point - pos).magnitude < minDist && (!ignorePlatform || hit.collider.gameObject.tag != "Platform"))
            {
                minDist = (hit.point - pos).magnitude;
                min = hit;
            }
        }
        return min;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Goal" && InLevel())
        {
            if (!damaged && !dead)
            {
                win = true;
            }
        }
    }

    public static bool InLevel()
    {
        return SceneManager.GetActiveScene().name != "1_Town"
                && SceneManager.GetActiveScene().name != "Level_Select_1"
                && SceneManager.GetActiveScene().name != "Level_Select_2"
                && SceneManager.GetActiveScene().name != "Level_Select_3"
                && SceneManager.GetActiveScene().name != "Level_Select_4"
                && SceneManager.GetActiveScene().name != "Level_Select_5";
    }

    bool Grounded()
    {
        return Raycast((Vector2)transform.position
            + Vector2.down * box.bounds.extents.y * 1.8f) != null;
    }
    bool Walled()
    {
        if (Raycast((Vector2)transform.position + Vector2.left * box.bounds.extents.x * 1.3f) != null) {
            wallDirection = -1;
        }
        if (Raycast((Vector2)transform.position + Vector2.right * box.bounds.extents.x * 1.3f) != null) {
            wallDirection = 1;
        }
        return  wallDirection != 0;
    }
}