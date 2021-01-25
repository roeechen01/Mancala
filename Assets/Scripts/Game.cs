using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public Sprite[] stoneSprites;
    public Stone stonePrefab;
    public Hole[] holes;
    public Text[] indicators;
    Ai ai;

    public bool p1Turn;
    public bool over = false;

    void Start()
    {
        ai = FindObjectOfType<Ai>();
        p1Turn = true;
    }

    void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        CheckKeys();
        if (!over)
        {
            if (Hole.animations == 0 && !IsInvoking("RepeatCase"))
            {
                if (p1Turn)
                {
                    indicators[0].text = "Turn: P1";
                    indicators[1].text = "";
                }
                else
                {
                    indicators[1].text = "Turn: Ai";
                    indicators[0].text = "";
                    Invoke("AiTurn", 1f);//AI CHANGE
                }
            }
            else
            {
                indicators[1].text = "";
                indicators[0].text = "";
            }

        }
        else
        {
            if (holes[6].GetStonesAmount() > holes[13].GetStonesAmount())
            {
                indicators[0].text = "Winner: P1";
                indicators[1].text = "";
            }
            else if (holes[6].GetStonesAmount() < holes[13].GetStonesAmount())
            {
                indicators[1].text = "Winner: Ai";
                indicators[0].text = "";
            }
            else
            {
                indicators[1].text = "Tie";
                indicators[0].text = "Tie";
            }
        }
        CheckGameOver();
    }

    void AiTurn()
    {
        ai.DoTurn();
        CancelInvoke("AiTurn");
    }

    public void Turn(int holeID)
    {
        Stone[] holeStones = holes[holeID].stones.ToArray();
        holes[holeID].stones.Clear();
        int pos = holeID + 1;
        bool extra = false;
        for (int i = holeStones.Length; i > 0; i--)
        {
            if (pos == 14)
                pos = 0;
            if (pos == 13 && p1Turn)
                pos = 0;
            if (pos == 6 && !p1Turn)
                pos = 7;
            holes[pos++].AddStone(holeStones[holeStones.Length - i], holeStones.Length - i + 1);
            if (i == 1 && ((pos - 1 == 6 && p1Turn) || (pos - 1 == 13 && !p1Turn)))
            {
                extra = true;
            }

            if (i == 1 && holes[pos - 1].GetStonesAmount() == 1 && ((p1Turn && pos - 1 < 6) || (!p1Turn && pos - 1 > 6)))
            {
                finalPos = pos - 1;
                InvokeRepeating("RepeatCase", 0.2f, 0.2f); 
            }
        }
        if (!extra)
            p1Turn = !p1Turn;
        CheckGameOver();
    }

    void RepeatCase()
    {
        if (Hole.animations == 0)
        {
            Case();
            CancelInvoke("RepeatCase");
        }
            
    }

    int finalPos;
    void Case()
    {
        switch (finalPos)
        {
            case 0:
                HandleCase(0, 12);
                break;
            case 1:
                HandleCase(1, 11);
                break;
            case 2:
                HandleCase(2, 10);
                break;
            case 3:
                HandleCase(3, 9);
                break;
            case 4:
                HandleCase(4, 8);
                break;
            case 5:
                HandleCase(5, 7);
                break;
            case 7:
                HandleCase(7, 5);
                break;
            case 8:
                HandleCase(8, 4);
                break;
            case 9:
                HandleCase(9, 3);
                break;
            case 10:
                HandleCase(10, 2);
                break;
            case 11:
                HandleCase(11, 1);
                break;
            case 12:
                HandleCase(12, 0);
                break;

            default: break;
        }
    }

    void HandleCase(int pos, int counterPos)
    {
        if (holes[counterPos].GetStonesAmount() > 0)
        {
            Stone[] stonesToHandle = new Stone[1 + holes[counterPos].GetStonesAmount()];
            int dest = 13;
            if (pos < 6)
                dest = 6;

            stonesToHandle[0] = holes[pos].stones[0];
            for (int i = 0; i < holes[counterPos].GetStonesAmount(); i++)
                stonesToHandle[i + 1] = holes[counterPos].stones[i];

            holes[pos].stones.Clear();
            holes[counterPos].stones.Clear();


            for (int i = 0; i < stonesToHandle.Length; i++)
                holes[dest].AddStone(stonesToHandle[i], i + 1);

            CheckGameOver();
        }
    }

        void CheckGameOver()
        {
            int counter = 0;
            for (int i = 0; i < 6; i++)
                counter += holes[i].GetStonesAmount();
            if (counter == 0)
                over = true;
            counter = 0;
            for (int i = 7; i < 13; i++)
                counter += holes[i].GetStonesAmount();
            if (counter == 0)
                over = true;
        }
    }
