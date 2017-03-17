using System;
using UnityEngine;

[Serializable]
public class TileSprite
{
    // Variables
    public Sprite tileImage;
    public SpriteRenderer renderer;
    public string tileName;
    public Tiles tileType;

    // Functions
    /*
     * public TileSprite()
     * public TileSprite(string name, Sprite image, Tiles type)
     * public TileSprite(TileSprite tileSprite)
     *
     * public void ClearPathColor(int x, int y, float height)
     * public void SetPathColor(int x, int y, float height)
     */

    public TileSprite()
    {
        tileName = "Unset";
        tileImage = new Sprite();
        tileType = Tiles.Unset;
    }

    public TileSprite(string name, Sprite image, Tiles type)
    {
        tileName = name;
        tileImage = image;
        tileType = type;
    }

    public TileSprite(TileSprite tileSprite)
    {
        tileName = tileSprite.tileName;
        tileImage = tileSprite.tileImage;
        tileType = tileSprite.tileType;
    }

    public void ClearPathColor(int x, int y, float height)
    {
        int tileNumber = (x + 1) + (y * (int)height);
        renderer = GameObject.Find("Tile " + tileNumber).GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1.0f);
    }

    public void SetPathColor(int x, int y, float height)
    {
        int tileNumber = (x + 1) + (y * (int)height);
        renderer = GameObject.Find("Tile " + tileNumber).GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
    }
}
