using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Slider healthFill;
    private Rigidbody2D rigid;
    public float jumpForce;
    private bool jumped;
    public float runSpeed;
    public float moveVel;
    public bool attacking;
    private Animator attack;
    private Animation anim;
    public GameObject sword;
    public float health;
    public float maxHealth = 100f;
    public float dmgPow;
    bool faceL;
    bool faceR;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        attack = sword.GetComponent<Animator>();
        anim = sword.GetComponent<Animation>();
        attacking = false;
        sword.SetActive(false);
        health = 100;
        dmgPow = Random.Range(10, 16);
        faceR = true;
        faceL = false;
        Time.timeScale = 1;
    }

    public void Update()
    {
        // moveVel is the base movement speed
        moveVel = 0;


        if (Input.GetKeyDown(KeyCode.Space) && jumped == false || Input.GetKeyDown("joystick 1 button 2") && jumped == false || Input.GetKeyDown(KeyCode.W) && jumped == false)
        {
            // add jump force on press, while dynamic physics is enabled, this works exactly as intended.
            rigid.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            jumped = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //this sets run speed to a flat rate in the positive direction (the right)
            moveVel = runSpeed;
            if (faceL)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            faceR = true;
            faceL = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // this sets the speed to a negative (left) run direction
            moveVel = -runSpeed;
            if (faceR)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            faceL = true;
            faceR = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // insert reload scene
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        //this sets the constant velocity per frame, allowing us to stop on a pixel
        rigid.velocity = new Vector2 (moveVel, rigid.velocity.y);
        
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            sword.SetActive(true);
            attacking = true;
            anim.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            sword.SetActive(false);
            attacking = false;
            anim.Stop();
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "ground")
        {
            jumped = false;
        }
    }
    public void DoDmg()
    {
        dmgPow = Random.Range(5, 11);
        health -= dmgPow;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthFill.value = health;
        if(health > 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
