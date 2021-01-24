using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public static Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    Game game;
    void Start()
    {
        game = FindObjectOfType<Game>();
        sprites = game.stoneSprites;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, 8)];

    }
}
