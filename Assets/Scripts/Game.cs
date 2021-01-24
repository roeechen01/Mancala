using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Sprite[] stoneSprites;
    public Stone stonePrefab;
    public Hole[] holes;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Turn(int holeID)
    {
        Stone[] holeStones = holes[holeID].stones.ToArray();
        holes[holeID].stones.Clear();
        int pos = holeID + 1;
        for (int i = holeStones.Length; i > 0; i--)
        {
            if (pos == 14)
                pos = 0;
            holes[pos++].AddStone(holeStones[holeStones.Length - i], holeStones.Length - i + 1);
        }
    }
}
