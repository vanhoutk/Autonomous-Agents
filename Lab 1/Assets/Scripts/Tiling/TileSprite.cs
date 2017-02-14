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
        this.movementCost = 1;
        this.tileName = "Unset";
        this.tileImage = new Sprite();
        this.tileType = Tiles.Unset;
    }

    public TileSprite(int moveCost, string name, Sprite image, Tiles type)
    {
        this.movementCost = moveCost;
        this.tileName = name;
        this.tileImage = image;
        this.tileType = type;
    }

    public TileSprite(TileSprite tileSprite)
    {
        this.movementCost = tileSprite.movementCost;
        this.tileName = tileSprite.tileName;
        this.tileImage = tileSprite.tileImage;
        this.tileType = tileSprite.tileType;
    }

    public void SetTileColor(Color color)
    {
        // TODO: Possibly change this
        this.renderer = GameObject.Find("Tile").GetComponent<SpriteRenderer>();
        this.renderer.color = color;
    }
}
