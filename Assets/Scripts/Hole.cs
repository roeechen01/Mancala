using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    Game game;
    public Text scoreText;
    private int startStonesAmount = 2;
    protected int id;
    public List<Stone> stones = new List<Stone>();
    float delay = 0.2f;
    public static int animations = 0;

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
        if(GetStonesAmount() > 0 && !(this is BigHole) && animations == 0 && IsMyHole())
            game.Turn(id);
    }

    bool IsMyHole()
    {
        return (game.p1Turn && id < 6) || (!game.p1Turn && id > 6);
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
        stonesToAdd[0].transform.localPosition = new Vector3(Random.Range(-3f, 0f), Random.Range(2.5f, -2.5f), -25f);
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
