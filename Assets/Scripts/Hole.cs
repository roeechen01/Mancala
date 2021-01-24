using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    Game game;
    public Text scoreText;
    private int startStonesAmount = 4;
    //protected int stonesAmount;
    protected int id;
    protected List<Stone> stones = new List<Stone>();
    void Start()
    {
        game = FindObjectOfType<Game>();
        AutoSetId();
        if (!(this is BigHole))
            for(int i = 0; i < startStonesAmount; i++)
            {
                Stone stone = Instantiate(game.stonePrefab, new Vector3(), Quaternion.identity, transform);
                stone.transform.localPosition = new Vector3(Random.Range(-3f, 0f), Random.Range(2.5f, -2.5f), -25f);
                stones.Add(stone);
            }
        
    }

    void Update()
    {
        if (GetStonesAmount() == 0)
            scoreText.text = "";
        else scoreText.text = GetStonesAmount().ToString();
    }

    public int GetStonesAmount()
    {
        return this.stones.Count;
    }

    void AutoSetId()
    {
        for(int i = 0; i < 14; i++)
        {
            if (this == game.holes[i])
                id = i;
        }
    }
}
