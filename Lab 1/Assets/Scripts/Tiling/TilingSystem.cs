using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Lean; //from Unity asset "LeanPool" - freely available in the Asset Store; used here for object pooling

public class TilingSystem : MonoBehaviour
{
    public GameObject tileContainerPrefab;
    public GameObject tilePrefab;
    public List<TileSprite> tileSprites;
    public List<Vector2> locations;
	public Sprite defaultImage;
    public SquareGrid mapGrid;
	public Vector2 CurrentPosition;
    public Vector2 MapSize;
    public Vector2 ViewPortSize;

    private float tileSize = 1f;

    private GameObject controller;
    private GameObject tileContainer;
    private List<GameObject> _tiles = new List<GameObject>();
    private TileSprite[,] _map;

    private TileSprite FindTile(Tiles tile)
    {
        foreach (TileSprite tileSprite in tileSprites)
        {
            if (tileSprite.tileType == tile) return tileSprite;
        }
        return null;
    }

    public TileSprite GetTile(int x, int y)
    {
        return _map[x, y];
    }

	// Create a map of size MapSize of unset tiles
	private void DefaultTiles()
    {
		for (var y = 0; y < MapSize.y - 1; y++)
        {
            for (var x = 0; x < MapSize.x - 1; x++)
            {
                _map[x, y] = new TileSprite(0, "Unset", defaultImage, Tiles.Unset);
            }
        }
	}

	// Set the tiles of the map to what is specified in tileSprites
	private void SetTiles()
    {
        // Add 0 vectors for the tiles we don't care about
        locations.Add(new Vector2(0, 0)); // Unset
        locations.Add(new Vector2(0, 0)); // Plains
        locations.Add(new Vector2(0, 0)); // Mountains

        // Initially make all tiles plain tiles
        for (var y = 0; y < MapSize.y; y++)
        {
            for (var x = 0; x < MapSize.x; x++)
            {
                _map[x, y] = new TileSprite(FindTile(Tiles.Plains));
            }
        }

        // Add Mountain tiles
        for (var i = 0; i < MapSize.x * 2; i++)
        {
            int randomX = UnityEngine.Random.Range(0, (int) MapSize.x);
            int randomY = UnityEngine.Random.Range(0, (int) MapSize.x);

            _map[randomX, randomY] = new TileSprite(FindTile(Tiles.Mountains));

            mapGrid.mountains.Add(new Node(randomX, randomY));
        }

        // Add the unique locations
        for (Tiles i = Tiles.Shack; i < Tiles.NUMBER_OF_TILES; i++)
        {
            int randX = UnityEngine.Random.Range(0, (int)MapSize.x);
            int randY = UnityEngine.Random.Range(0, (int)MapSize.x);

            while (_map[randX, randY].tileType != Tiles.Plains)
            {
                randX = UnityEngine.Random.Range(0, (int)MapSize.x);
                randY = UnityEngine.Random.Range(0, (int)MapSize.x);
            }

            _map[randX, randY] = new TileSprite(FindTile(i));
            locations.Add(new Vector2((randX) * tileSize, (randY) * tileSize)); 
        }
    }

	private void AddTilesToMap()
    {
        foreach (GameObject o in _tiles)
        {
            LeanPool.Despawn(o);
        }
        _tiles.Clear();
        LeanPool.Despawn(tileContainer);
   
        tileContainer = LeanPool.Spawn (tileContainerPrefab);

		
		var viewOffsetX = ViewPortSize.x/2;
		var viewOffsetY = ViewPortSize.y/2;

		for (var y = -viewOffsetY; y <= viewOffsetY; y++)
        {
			for (var x = -viewOffsetX; x <= viewOffsetX; x++)
            {
				var tX = x * tileSize;
				var tY = y * tileSize;

				var iX = x + CurrentPosition.x;
				var iY = y + CurrentPosition.y;

				if (iX < 0)
					continue;
				if (iY < 0)
					continue;
				if (iX > MapSize.x - 1)
					continue;
				if (iY > MapSize.y - 1)
					continue;

				var tile = LeanPool.Spawn (tilePrefab);
				tile.transform.position = new Vector3 (tX, tY, 0);
				tile.transform.SetParent (tileContainer.transform);
				
				// Set an image for the tile - do this when you create your tile prefabs (for shack, mountains, ...)
				var renderer = tile.GetComponent<SpriteRenderer> ();
				renderer.sprite = _map [(int)x + (int)CurrentPosition.x, (int)y + (int)CurrentPosition.y].tileImage;
				
				_tiles.Add (tile);
			}
		}
	}

	public void Start()
    {
        controller = GameObject.Find("Controller");
        _map = new TileSprite[(int)MapSize.x, (int)MapSize.y];
        mapGrid = new SquareGrid((int)MapSize.x, (int)MapSize.y);
		DefaultTiles ();
		SetTiles ();
		AddTilesToMap ();

        GameObject outlawObject = GameObject.Find("Jesse");
        GameObject minerObject = GameObject.Find("Miner");
        Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
        outlaw.currentLocation = locations[(int)Tiles.OutlawCamp];
        Miner miner = minerObject.GetComponent<Miner>();
        miner.currentLocation = locations[(int)Tiles.Shack];

        double testCost = mapGrid.Cost(new Node(0, 0), new Node(0, 1));
        Debug.Log("TilingSystem: testCost = " + testCost);
    }
}
