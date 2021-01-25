using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    Game game;
    public Text scoreText;
    private int startStonesAmount = 3;
    protected int id;
    public int stonesAmount = 0;//Made for Ai purpouses
    public List<Stone> stones = new List<Stone>();
    float delay = 0.2f;
    public static int animations = 0;

    void Start()
    {
        animations = 0;
        game = FindObjectOfType<Game>();
        AutoSetId();
        if (!(this is BigHole))
            for(int i = 0; i < startStonesAmount; i++)
            {
                Stone stone = Instantiate(game.stonePrefab, new Vector3(), Quaternion.identity, transform);
                int rnd = Random.Range(0, 2);
                if(rnd == 0)
                    stone.transform.localPosition = new Vector3(Random.Range(-2.8f, -0.4f), Random.Range(-2.6f, 2.7f), -25f);
                else stone.transform.localPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(0.3f, 2.4f), -25f);

                stones.Add(stone);
            }
        
    }

    void Update()
    {
        if (GetStonesAmount() == 0)
            scoreText.text = "0";
        else scoreText.text = GetStonesAmount().ToString();
    }

    void OnMouseDown()
    {
        if(!game.over && GetStonesAmount() > 0 && !(this is BigHole) && animations == 0 && IsMyHole() && !IsInvoking("RepeatCase"))
            game.Turn(id);
    }

    bool IsMyHole()
    {
        return (game.p1Turn && id < 6)/* || (!game.p1Turn && id > 6)*/;//AI CHANGE
    }

    public int GetStonesAmount()
    {
        return this.stones.Count;
    }

    List<Stone> stonesToAdd = new List<Stone>();
    public void AddStone(Stone stone, int index)
    {
        stones.Add(stone);
        stonesToAdd.Add(stone);
        animations++;
        Invoke("Animate", delay * index);
    }

    void Animate()
    {
        stonesToAdd[0].transform.parent = transform;
        if (this is BigHole)
            stonesToAdd[0].transform.localPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(6f, -6f), -25f);
        else
        {
            int rnd = Random.Range(0, 2);
            if (rnd == 0)
                stonesToAdd[0].transform.localPosition = new Vector3(Random.Range(-2.8f, -0.4f), Random.Range(-2.6f, 2.7f), -25f);
            else stonesToAdd[0].transform.localPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(0.3f, 2.4f), -25f);
        }
        stonesToAdd.Remove(stonesToAdd[0]);
        animations--;
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
