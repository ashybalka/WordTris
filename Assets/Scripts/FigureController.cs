using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class FigureController : MonoBehaviour
{
    public char figureChar;
    private float maxDownTime = 0.6f;
    private float maxSideTime = 0.6f;
    private float curDownTime = 0f;


    private bool nextHor;

    private float curHorizontalTime = 0f;
    private float directionHor = 0f;

    private GameManager gameManager;
    private RayController rayController;

    public bool isCollided = false;
    private bool isFalling = true;

    private float downSpeed = 1f;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rayController = GameObject.Find("RayCasters").GetComponent<RayController>();
    }

    void Update()
    {


        if (!isCollided && isFalling)
        {
            Vector2 curPos = transform.position;

            RaycastHit2D lefthittop = Physics2D.Raycast(new Vector2(transform.position.x -0.5f, transform.position.y + 0.51f), Vector2.left, 1f);
            RaycastHit2D lefthitbot = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.51f), Vector2.left, 1f);
            RaycastHit2D righthittop = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.51f), -Vector2.left, 1f);
            RaycastHit2D righthitbot = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f), -Vector2.left, 1f);

            // Left/Right Speed
            if (Input.GetKey(KeyCode.A) && transform.position.x > -4f)
            {
                if (lefthittop.collider == null && lefthitbot.collider == null)
                {
                    directionHor = -1f;
                    maxSideTime = 0.2f;
                }                
            }
            else if (Input.GetKey(KeyCode.D) && transform.position.x < 4f)
            {

                if (righthittop.collider == null && righthitbot.collider == null)
                {
                    directionHor = 1f;
                    maxSideTime = 0.2f;
                }               
            }
            else
            {
                maxSideTime = 0.6f;
                directionHor = 0;
            }

            curPos = transform.position;
            curHorizontalTime += Time.deltaTime;


            if (curHorizontalTime >= maxSideTime)
            {
                transform.position = new Vector2(curPos.x + directionHor, curPos.y);
                curHorizontalTime = 0f;
            }
        }

        var downRaydownRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, 0.48f);
        if (downRaydownRay.collider == null)
        {
            FallingDown();
        }
    }


    public void FallingDown()
    {
        Vector2 curPos = transform.position;
        curDownTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S))
        {
            downSpeed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            downSpeed = 1f;
        }
        

        float levelSpeed = 1 + (gameManager.level / 10);

        if (curDownTime >= (maxDownTime / downSpeed / levelSpeed) && transform.position.y > -3.5)
        {
            
            transform.position += new Vector3(0, -1f, 0);
            curDownTime = 0f;
        }
    }


    public void OnCollisionStay2D(Collision2D collision)
    {
        if (!isCollided)
        {
            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Figure"))
            {
                isCollided = true;  
                rayController.GetRaysToLetters();
                gameManager.FindDictionaryWords();
                if (isFalling)
                {
                    gameManager.LetterSpawner();
                }
                isFalling = false;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (isCollided)
        {
            isCollided = false;
        }
    }

    public void SetCharToTMP(char letter)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = letter.ToString();
    }

    public void HiglightFigure()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }

}
