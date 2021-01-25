using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    Game game;
    AiHole[] holes;
    void Start()
    {
        holes = new AiHole[14];
        for (int i = 0; i < holes.Length; i++)
            holes[i] = new AiHole();
        game = FindObjectOfType<Game>();
        
    }

    void Update()
    {
        
    }

    void UpdateHoles()
    {
        for(int i = 0; i < 14; i++)
            holes[i].stonesAmount = game.holes[i].GetStonesAmount();
    }

    public void DoTurn()
    {
        UpdateHoles();
        if (!game.p1Turn && !game.over)
        {
            /////////////////Needs to be because of a bug that when only one hole contains stones, the AI doesnt move (probably fixed)
            /*int counter = 0;
            int index = 0;
            for(int i = 0; i < 6; i++)
            {
                if (holes[i + 7].stonesAmount != 0)
                {
                    counter++;
                    index = i + 7;
                }
            }
            if (counter == 1)
            {
                game.Turn(index);
                return;
            }*/
            //////////////////
            int[] pointsGained = new int[6];
            for (int i = 0; i < 6; i++)
            {
                UpdateHoles();
                int[] p1Gained = new int[6];
                for(int j = 0; j < 6; j++)
                {
                    UpdateHoles();
                    SimulateAiTurn(i + 7);
                    p1Gained[j] = SimulateP1Turn(j);
                    //print(i + ": p1Gained[" + j + "] = " + p1Gained[j]);
                }
                //int p1Turn = 0;
                int p1MaxPoints = -10;

                for (int j = 0; j < 6; j++)
                {
                    if (p1Gained[j] > p1MaxPoints)
                    {
                        p1MaxPoints = p1Gained[j];
                    }
                }
                //print(i + ": p1MaxPoints = " + p1MaxPoints);

                UpdateHoles();
                pointsGained[i] = SimulateAiTurn(i + 7) - p1MaxPoints;
            }
            int turn = 0;
            int maxPoints = -10;
            for (int i = 0; i < 6; i++)
            {
                if (pointsGained[i] == maxPoints)
                {   //Here are some Ai methods that might give it the winning edge
                    int cur = turn + holes[turn].stonesAmount + 1;
                    int next = i + 7 + holes[i + 7].stonesAmount + 1;
                    if(cur % 13.0 == 0 && next % 13.0 == 0)
                    {
                        maxPoints = pointsGained[i];
                        turn = i + 7;
                    }
                    else if (cur % 13.0 != 0 && next % 13.0 == 0)
                    {
                        maxPoints = pointsGained[i];
                        turn = i + 7;
                    }
                    else if (cur % 13.0 != 0 && next % 13.0 != 0)
                    {
                        if(next > cur)
                        {
                            maxPoints = pointsGained[i];
                            turn = i + 7;
                        }
                    }
                }
                else
                {
                    if(pointsGained[i] > maxPoints)
                    {
                        maxPoints = pointsGained[i];
                        turn = i + 7;
                    }
                }

            }

            //print(turn);
            game.Turn(turn);
        }

    }

    int finalPos = 0;
    int extraValue = 2;
    int SimulateAiTurn(int pos)
    {
        if (holes[pos].stonesAmount == 0)
            return -20;
        int basePoints = holes[13].stonesAmount;
        int stonesAmount = holes[pos].stonesAmount;
        holes[pos].stonesAmount = 0;
        pos++;
        bool extra = false;
        for (int i = stonesAmount; i > 0; i--)
        {
            if (pos == 14)
                pos = 0;
            if (pos == 6)
                pos = 7;

            holes[pos++].stonesAmount++;
            if (i == 1 && pos - 1 == 13)
            {
                extra = true;
            }

            if (i == 1 && holes[pos - 1].stonesAmount == 1 && pos - 1 > 6)
            {
                finalPos = pos - 1;
                Case();
            }
        }
        int gained = holes[13].stonesAmount - basePoints;
        //print(firstPos + " " + (holes[13].stonesAmount - basePoints));
        if (extra)
            gained += extraValue;
        return gained;
    }

    int SimulateP1Turn(int pos)
    {
        if (holes[pos].stonesAmount == 0)
            return -20;
        int basePoints = holes[6].stonesAmount;
        int stonesAmount = holes[pos].stonesAmount;
        holes[pos].stonesAmount = 0;
        pos++;
        bool extra = false;
        for (int i = stonesAmount; i > 0; i--)
        {
            if (pos == 14)  
                pos = 0;
            if (pos == 13)
                pos = 0;

            holes[pos++].stonesAmount++;
            if (i == 1 && pos - 1 == 6)
            {
                extra = true;
            }

            if (i == 1 && holes[pos - 1].stonesAmount == 1 && pos - 1 < 6)
            {
                finalPos = pos - 1;
                Case();
            }
        }
        int gained = holes[6].stonesAmount - basePoints;
        //print(firstPos + " " + (holes[13].stonesAmount - basePoints));
        if (extra)
            gained += extraValue;
        return gained;
    }

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
        if (holes[counterPos].stonesAmount > 0)
        {
            int stonesAmount = 1 + holes[counterPos].stonesAmount;
            int dest = 13;
            if (pos < 6)
                dest = 6;

            holes[pos].stonesAmount = 0;
            holes[counterPos].stonesAmount = 0;


            for (int i = 0; i < stonesAmount; i++)
                holes[dest].stonesAmount++;

        }
    }

    class AiHole
    {
        public int stonesAmount;
    }
    
}

