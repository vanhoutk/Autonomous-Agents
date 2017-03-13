using Lean; // From Unity asset "LeanPool" - freely available in the Asset Store; used here for object pooling
using System.Collections.Generic;
using UnityEngine;

public class TilingSystem : MonoBehaviour
{
    // Variables
    private Dictionary<Tiles, AttenuationData> tileAttenuationData = new Dictionary<Tiles, AttenuationData>();
    private float tileSize = 1f;
    private GameObject tileContainer;
    private List<GameObject> _tiles = new List<GameObject>();
    private TileSprite[,] _map;

    public GameObject tileContainerPrefab;
    public GameObject tilePrefab;
    public List<TileSprite> tileSprites;
    public List<Vector2> locations;
	public Sprite defaultImage;
    public SquareGrid mapGrid;
    //public SquareGrid2 mapGrid2;
	public Vector2 CurrentPosition;
    public Vector2 MapSize;
    public Vector2 ViewPortSize;

    // Functions
    /*
     * private TileSprite FindTile(Tiles tile)
     * public TileSprite GetTile(int x, int y)
     *
     * private void AddTilesToMap()
     * private void DefaultTiles()
     * private void SetTileAttenuationData()
     * private void SetTiles()
     * public void Start()
     */

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

    private void AddTilesToMap()
    {
        foreach (GameObject o in _tiles)
        {
            LeanPool.Despawn(o);
        }

        _tiles.Clear();
        LeanPool.Despawn(tileContainer);

        tileContainer = LeanPool.Spawn(tileContainerPrefab);

        var viewOffsetX = ViewPortSize.x / 2;
        var viewOffsetY = ViewPortSize.y / 2;

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

                var tile = LeanPool.Spawn(tilePrefab);
                tile.transform.position = new Vector3(tX, tY, 0);
                tile.transform.SetParent(tileContainer.transform);

                // Set an image for the tile - do this when you create your tile prefabs (for shack, mountains, ...)
                var renderer = tile.GetComponent<SpriteRenderer>();
                renderer.sprite = _map[(int)x + (int)CurrentPosition.x, (int)y + (int)CurrentPosition.y].tileImage;

                _tiles.Add(tile);
            }
        }
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

    // Initialise the dictionary with the tile attenuation data
    private void SetTileAttenuationData()
    {
        tileAttenuationData.Add(Tiles.Bank, new AttenuationData(1, 10, 10, 10));
        tileAttenuationData.Add(Tiles.Cemetery, new AttenuationData(1, 1, 1, 1));
        tileAttenuationData.Add(Tiles.GoldMine, new AttenuationData(1, 10, 10, 10));
        tileAttenuationData.Add(Tiles.Mountains, new AttenuationData(5, 10, 1, 1));
        tileAttenuationData.Add(Tiles.OutlawCamp, new AttenuationData(1, 5, 1, 5));
        tileAttenuationData.Add(Tiles.Plains, new AttenuationData(1, 1, 1, 1));
        tileAttenuationData.Add(Tiles.Saloon, new AttenuationData(1, 10, 20, 10));
        tileAttenuationData.Add(Tiles.Shack, new AttenuationData(1, 10, 10, 10));
        tileAttenuationData.Add(Tiles.SheriffsOffice, new AttenuationData(1, 10, 10, 10));
        tileAttenuationData.Add(Tiles.Undertakers, new AttenuationData(1, 10, 10, 10));
        tileAttenuationData.Add(Tiles.Unset, new AttenuationData(0, 0, 0, 0));
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
                Coordinates coordinates = new Coordinates(x, y);
                mapGrid.nodeSet.Add(coordinates, new Node(coordinates, tileAttenuationData[Tiles.Plains]));
            }
        }

        // Add Mountain tiles
        for (var i = 0; i < MapSize.x * 2; i++)
        {
            int randomX = UnityEngine.Random.Range(0, (int) MapSize.x);
            int randomY = UnityEngine.Random.Range(0, (int) MapSize.x);

            _map[randomX, randomY] = new TileSprite(FindTile(Tiles.Mountains));

            //mapGrid.mountains.Add(new Node(randomX, randomY));

            Coordinates random_coordinates = new Coordinates(randomX, randomY);
            if (mapGrid.nodeSet.ContainsKey(random_coordinates))
                mapGrid.nodeSet.Remove(random_coordinates);
            mapGrid.nodeSet.Add(random_coordinates, new Node(random_coordinates, tileAttenuationData[Tiles.Mountains]));
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

            Coordinates rand_coordinates = new Coordinates(randX, randY);
            if (mapGrid.nodeSet.ContainsKey(rand_coordinates))
                mapGrid.nodeSet.Remove(rand_coordinates);
            mapGrid.nodeSet.Add(rand_coordinates, new Node(rand_coordinates, tileAttenuationData[i])); // TODO: Need to change the last 4 values to be location dependent
        }
    }

    public void Start()
    {
        SetTileAttenuationData();

        _map = new TileSprite[(int)MapSize.x, (int)MapSize.y];
        mapGrid = new SquareGrid((int)MapSize.x, (int)MapSize.y);
        //mapGrid2 = new SquareGrid2((int)MapSize.x, (int)MapSize.y);

        DefaultTiles();
        SetTiles();
        AddTilesToMap();

        // Set the initial locations of each of the agents
        // Ideally would do this within the classes, but agent classes get created before the tiling system
        GameObject outlawObject = GameObject.Find("Jesse");
        if(outlawObject != null)
        {
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
            outlaw.ChangeLocation(Tiles.OutlawCamp);
        }

        GameObject minerObject = GameObject.Find("Miner");
        if(minerObject != null)
        {
            Miner miner = minerObject.GetComponent<Miner>();
            miner.ChangeLocation(Tiles.Shack);
        }
        
        GameObject sheriffObject = GameObject.Find("Wyatt");
        if(sheriffObject != null)
        {
            Sheriff sheriff = sheriffObject.GetComponent<Sheriff>();
            sheriff.ChangeLocation(Tiles.SheriffsOffice);
        }

        GameObject undertakerObject = GameObject.Find("Under");
        if(undertakerObject != null)
        {
            Undertaker undertaker = undertakerObject.GetComponent<Undertaker>();
            undertaker.ChangeLocation(Tiles.Undertakers);
        }
    }
}
