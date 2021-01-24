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
    public List<Stone> stones = new List<Stone>();
    float delay = 0.2f;

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

    void OnMouseDown()
    {
        if(GetStonesAmount() > 0 && !(this is BigHole))
            game.Turn(id);
    }

    public int GetStonesAmount()
    {
        return this.stones.Count;
    }

    Stone stone;
    public void AddStone(Stone stone, int index)
    {
        this.stone = stone;
        Invoke("Animate", delay * index);
        stones.Add(stone);
    }

    void Animate()
    {
        stone.transform.parent = transform;
        stone.transform.localPosition = new Vector3(Random.Range(-3f, 0f), Random.Range(2.5f, -2.5f), -25f);
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
