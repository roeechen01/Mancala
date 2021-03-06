﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    Game game;
    public Text scoreText;
    private int startStonesAmount = 4;
    protected int id;
    public List<Stone> stones = new List<Stone>();
    float delay = 0.2f;
    public static int animations = 0;

    void Start()
    {
        animations = 0;
        game = FindObjectOfType<Game>();
        AutoSetId();
        if (!IsBigHole())
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
        if(!game.GetOver() && GetStonesAmount() > 0 && !IsBigHole() && animations == 0 && IsMyHole() && !IsInvoking("RepeatCase"))
            game.Turn(id);
    }

    bool IsMyHole()
    {
        if(game.aiPlaying)
            return (game.GetP1Turn() && id < 6);
        else return (game.GetP1Turn() && id < 6) || (!game.GetP1Turn() && id > 6);
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
        Vector3 pos;
        if (IsBigHole())
        {
            stonesToAdd[0].transform.localPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(-4.5f, 6f), -25f);
            pos = stonesToAdd[0].transform.position;
            //print(pos.x + ", " + pos.y);
        }
            
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

    bool IsBigHole()
    {
        return id == 6 || id == 13;
    }
}
