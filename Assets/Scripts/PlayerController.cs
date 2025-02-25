﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 700f;
    public float jumpForce2 = 700f;
    public float runSpeed = 2.0f;
    public float runSpeedWhileJump = 2.0f;
    private int jumpCount = 0;
    private bool isJumping = false;
    private bool isDoubleJump = false;
    private Rigidbody2D playerRigidbody;
    Animator anim;
    Vector3 pos;
    float damegedWhen = 0;

    public GameObject sound_j1;
    public GameObject sound_j2;
    public GameObject sound_d;

    //public int life;
    public GameObject[] heart;

    void Start()
    {

        GameManager.instance.player = gameObject;

        Debug.Log(GameManager.instance.life);
        for (int index = 0; index < 3; index++)
        {
            heart[index].gameObject.SetActive(false);
        }
        for (int index = 0; index < GameManager.instance.life; index++)
        {
            heart[index].gameObject.SetActive(true);
        }
        anim = GetComponent<Animator>();
        this.playerRigidbody = this.GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        // 달리기
        transform.Translate(Vector3.right * runSpeed * Time.deltaTime);

        // 점프
        if (Input.GetButtonDown("Jump") && this.jumpCount < 1)
        {
            transform.Translate(Vector3.right * runSpeedWhileJump * Time.deltaTime);
            sound_j1.GetComponent<AudioSource>().Play();   
            this.jumpCount++;
            playerRigidbody.velocity = new Vector2(0, 0);
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            //this.playerRigidbody.AddForce(transform.up * this.jumpForce);
            //if (this.jumpCount > 1)
            //{
            //    this.isDoubleJump = true;
            //    transform.GetComponent<PlayerSound>().PlaySound("jumpTwice");
            //}
            anim.SetBool("isJumping", true);
        }
        else if (Input.GetButtonDown("Jump") && this.jumpCount < 2)
        {
            transform.Translate(Vector3.right * runSpeedWhileJump * Time.deltaTime);
            sound_j2.GetComponent<AudioSource>().Play();
            this.isDoubleJump = true;
            this.jumpCount++;
            playerRigidbody.velocity = new Vector2(0, 0);
            playerRigidbody.AddForce(Vector3.up * jumpForce2, ForceMode2D.Impulse);
            //this.playerRigidbody.AddForce(transform.up * this.jumpForce);
            //if (this.jumpCount > 1)
            //{
            //    this.isDoubleJump = true;
            //    transform.GetComponent<PlayerSound>().PlaySound("jumpTwice");
            //}
            anim.SetBool("isJumping", true);
        }
        else
            transform.Translate(Vector3.right * runSpeed * Time.deltaTime);

        // y축 속도가 0이면 바닥인 것으로 판단
        if (playerRigidbody.velocity.y == 0.0)
        {
            this.isJumping = false;
            this.jumpCount = 0;
            anim.SetBool("isJumping", false);

        }

        pos = this.transform.position;
        if (pos.y < -30)
        {
            SceneManager.LoadScene("Gameover");

        }


    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (Time.time - damegedWhen > 0.5f)
        {
            if (o.gameObject.CompareTag("Monster") || o.gameObject.CompareTag("GingerBread"))
            {
                GameManager.instance.life--;
                OnDamage();

                for (int index = 0; index < 3; index++)
                {
                    heart[index].gameObject.SetActive(false);
                }
                for (int index = 0; index < GameManager.instance.life; index++)
                {
                    heart[index].gameObject.SetActive(true);
                }
                if (GameManager.instance.life <= 0)
                {
                    SceneManager.LoadScene("Gameover");
                }

            }

        }
    }

    void OnDamage()
    {
        damegedWhen = Time.time;
        anim.SetTrigger("hurt");
        sound_d.GetComponent<AudioSource>().Play();
    }
}