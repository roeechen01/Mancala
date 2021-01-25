using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    Game game;
    void Start()
    {
        game = FindObjectOfType<Game>();
    }

    void Update()
    {
        
    }

    public void DoTurn()
    {
        if (!game.p1Turn && !game.over)
        {
            bool zero = true;
            int rnd = Random.Range(7, 13);
            while (zero)
            {
                if (game.holes[rnd].GetStonesAmount() == 0)
                    rnd = Random.Range(7, 13);
                else zero = false;
            }
            game.Turn(rnd);
        }
        
    }
}
