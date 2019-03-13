using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Slider healthFill;
    private Rigidbody2D rigid;
    public float runSpeed;
    public float moveVel;
    private bool sawPlayerL;
    bool sawPlayerR;
    private bool attackDistL;
    private bool attackDistR;
    float distance;
    float atkDist;
    private Animator attack;
    private Animation anim;
    public GameObject sword;
    public bool attacking;
    private bool facingR;
    private bool facingL;
    bool hurt;
    public float health;
    public float maxHealth = 100f;
    float dmgPow;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        sawPlayerL = false;
        sawPlayerR = false;
        attackDistL = false;
        attackDistR = false;
        attack = sword.GetComponent<Animator>();
        anim = sword.GetComponent<Animation>();
        attacking = false;
        sword.SetActive(false);
        facingL = true;
        facingR = false;
        hurt = false;
        health = 100;
        
    }

    public void Update()
    {
        Vector2 currentPos = transform.position;

        //left side variables
        var startCastL = currentPos;
        startCastL.x -= 1.0f;
        var endCastL = startCastL;
        endCastL.x -= 5.0f;
        var atkCastL = startCastL;
        atkCastL.x -= 0.5f;
        //right side variables
        var startCastR = currentPos;
        startCastR.x += 1.0f;
        var endCastR = startCastR;
        endCastR.x += 5.0f;
        var atkCastR = startCastR;
        atkCastR.x += 0.5f;
        //vars for right side to flip into Left side
        var startFlipR = startCastR;
        startFlipR.x += 1.0f;
        var endFlipR = startCastR;
        endFlipR.x += 5.0f;
        //vars for left side to flip into right side
        var startFlipL = startCastL;
        startFlipL.x -= 1.0f;
        var endFlipL = startCastL;
        endFlipL.x -= 5.0f;


        // moveVel is the base movement speed
        moveVel = 0;



        // left side checks
        if (!facingR)
        {
            Debug.DrawLine(startCastL, endCastL, Color.yellow);
            RaycastHit2D enemyLeft = Physics2D.Linecast(startCastL, endCastL);
            if (enemyLeft.collider != null)
            {
                if (enemyLeft.collider.name == "Player")
                {
                    if (!sawPlayerL)
                    {
                        sawPlayerL = true;
                    }
                }
            }
            else
            {
                sawPlayerL = false;
            }
            Debug.DrawLine(startCastL, atkCastL, Color.red);
            RaycastHit2D enemyAtkL = Physics2D.Linecast(startCastL, atkCastL);
            if (enemyAtkL.collider != null)
            {
                if (enemyAtkL.collider.name == "Player")
                {
                    if (!attackDistL)
                    {
                        attackDistL = true;
                        Debug.Log("attacking player now!");
                        AttackPlayer();
                    }
                }
            }
            else
            {
                attackDistL = false;
                stopAttack();
            }
            //flip from left to right
            Debug.DrawLine(startFlipR, endFlipR, Color.green);
            RaycastHit2D enemyFlipR = Physics2D.Linecast(startFlipR, endFlipR);
            if (enemyFlipR.collider != null)
            {
                if (enemyFlipR.collider.name == "Player")
                {
                    if (transform.rotation == Quaternion.Euler(0, 0, 0) && facingL)
                    {
                        FlipRight();
                    }
                }
            }

        }


        //right side checks
        if (!facingL)
        {
            Debug.DrawLine(startCastR, endCastR, Color.yellow);
            RaycastHit2D enemyRight = Physics2D.Linecast(startCastR, endCastR);
            if (enemyRight.collider != null)
            {
                if (enemyRight.collider.name == "Player")
                {
                    if (!sawPlayerR)
                    {
                        sawPlayerR = true;
                    }
                }
            }
            else
            {
                sawPlayerR = false;
            }
            Debug.DrawLine(startCastR, atkCastR, Color.red);
            RaycastHit2D enemyAtkR = Physics2D.Linecast(startCastR, atkCastR);
            if (enemyAtkR.collider != null)
            {
                if (enemyAtkR.collider.name == "Player")
                {
                    if (!attackDistR)
                    {
                        attackDistR = true;
                        Debug.Log("attacking player now!");
                        AttackPlayer();
                    }
                }
            }
            else
            {
                attackDistR = false;
                stopAttack();
            }

            //flip from right to left
            Debug.DrawLine(startFlipL, endFlipL, Color.blue);
            RaycastHit2D enemyFlipL = Physics2D.Linecast(startFlipL, endFlipL);  
            if (enemyFlipL.collider != null)
            {
                if (enemyFlipL.collider.name == "Player")
                {
                    if (transform.rotation != Quaternion.Euler(0, 0, 0) && facingR)
                    {
                        FlipLeft();
                    }
                }
            }
        }
        





        //set movements
        if (sawPlayerR && !attackDistR)
        {
            //this sets run speed to a flat rate in the positive direction (the right)
            moveVel = runSpeed;
        }
        else if (sawPlayerL && !attackDistL)
        {
            // this sets the speed to a negative (left) run direction
            moveVel = -runSpeed;
        }
        if (hurt && sawPlayerR)
        {
            FlipLeft();
            moveVel = -runSpeed * 0.5f;
        }
        else if(hurt && sawPlayerL)
        {
            FlipRight();
            moveVel = runSpeed * 0.5f;
        }
        //this sets the constant velocity per frame, allowing us to stop on a pixel
        rigid.velocity = new Vector2(moveVel, rigid.velocity.y);

    }

    public void AttackPlayer()
    {
        sword.SetActive(true);
        attacking = true;
        anim.Play();
    }
    public void stopAttack()
    {
        sword.SetActive(false);
        attacking = false;
        anim.Stop();
    }

    public void DoDmg()
    {
        dmgPow = Random.Range(5, 11);
        health -= dmgPow;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthFill.value = health;
        if(health <= 20)
        {
            hurt = true;
        }
        else
        {
            hurt = false;
        }
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }
    public void FlipLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        facingR = false;
        facingL = true;
    }
    public void FlipRight()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        facingL = false;
        facingR = true;
    }
}
