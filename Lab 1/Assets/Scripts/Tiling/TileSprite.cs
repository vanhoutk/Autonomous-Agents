using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TileSprite
{
    public int movementCost;
    public string tileName;

    public Sprite tileImage;
    public SpriteRenderer renderer;

    public Tiles tileType;

    public TileSprite()
    {
        movementCost = 1;
        tileName = "Unset";
        tileImage = new Sprite();
        tileType = Tiles.Unset;
    }

    public TileSprite(int moveCost, string name, Sprite image, Tiles type)
    {
        movementCost = moveCost;
        tileName = name;
        tileImage = image;
        tileType = type;
    }

    public TileSprite(TileSprite tileSprite)
    {
        movementCost = tileSprite.movementCost;
        tileName = tileSprite.tileName;
        tileImage = tileSprite.tileImage;
        tileType = tileSprite.tileType;
    }

    public void SetTileColor(Color color)
    {
        // TODO: Possibly change this
        renderer = GameObject.Find("Tile").GetComponent<SpriteRenderer>();
        //renderer.color.a = 0.5f;
        renderer.color = color;
    }

    public void SetPathColor(int x, int y, float height)
    {
        int tileNumber = (x + 1) + (y * (int)height);
        renderer = GameObject.Find("Tile " + tileNumber).GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
    }

    public void ClearPathColor(int x, int y, float height)
    {
        int tileNumber = (x + 1) + (y * (int)height);
        renderer = GameObject.Find("Tile " + tileNumber).GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1.0f);
    }
}
