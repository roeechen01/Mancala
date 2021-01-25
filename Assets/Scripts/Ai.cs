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
        if (!game.p1Turn && !game.over)
        {
            int[] pointsGained = new int[6];
            for (int i = 0; i < 6; i++)
            {
                UpdateHoles();
                pointsGained[i] = SimulateTurn(i + 7);
            }
            int turn = 0;
            int maxPoints = -2;
            for (int i = 0; i < 6; i++)
            {
                if (pointsGained[i] > maxPoints)
                {
                    maxPoints = pointsGained[i];
                    turn = i + 7;
                }
            }


            /*
            bool zero = true;
            int rnd = Random.Range(7, 13);
            while (zero)
            {
                if (game.holes[rnd].GetStonesAmount() == 0)
                    rnd = Random.Range(7, 13);
                else zero = false;
            }
            game.Turn(rnd);
            */
            game.Turn(turn);
        }

    }

    int finalPos = 0;
    int SimulateTurn(int pos)
    {
        int firstPos = pos;
        if (holes[pos].stonesAmount == 0)
            return -1;
        int basePoints = holes[13].stonesAmount;
        int stonesAmount = holes[pos].stonesAmount;
        holes[pos].stonesAmount = 0;
        pos++;
        bool extra = false;
        for (int i = stonesAmount; i > 0; i--)
        {
            if (pos == 14)
                pos = 0;
            if (pos == 13 && game.p1Turn)
                pos = 0;
            if (pos == 6 && !game.p1Turn)
                pos = 7;

            holes[pos++].stonesAmount++;
            if (i == 1 && ((pos - 1 == 6 && game.p1Turn) || (pos - 1 == 13 && !game.p1Turn)))
            {
                extra = true;
            }

            if (i == 1 && holes[pos - 1].stonesAmount == 1 && ((game.p1Turn && pos - 1 < 6) || (!game.p1Turn && pos - 1 > 6)))
            {
                finalPos = pos - 1;
                Case();
            }
        }
        int extraValue = 2;
        int gained = holes[13].stonesAmount - basePoints;
        //print(firstPos + " " + (holes[13].stonesAmount - basePoints));
        if (extra)
            gained += extraValue;
        return gained;
    }

    void Case()
    {
        switch (finalPos)
        {
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

