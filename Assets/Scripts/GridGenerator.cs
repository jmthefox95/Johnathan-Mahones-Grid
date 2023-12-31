using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.WSA;

public class GridGenerator : MonoBehaviour
{
    public PlayerControl player;
    // How big the gird is  (how many rows and columns)
    public int rows;
    public int columns;
    public Tile[,] tiles;
    private Tile randomTrapTile;

    public float duration = 3;
    float timeElapsed;

    // Tile prefab were going to use to make the grid
    public GameObject tilePrefab;

    //Origin tile position , All subsequent tiles will be positioned based on this one
    //Origin tile is [0,0];
    public Vector3 originPos = new Vector3(-3, -3, 0);

    [Range(0, 5)] public int holeCount;
    [Range(0, 5)] public int trapTileCount;
    [Range(0, 1)] public int goalTileCount;
    [Range(0, 2)] public int deathTileCount;

    public List<Tile> trapTiles = new List<Tile>();
    private List<Tile> inaccessibleTiles = new List<Tile>();
    private List<Tile> goalTile = new List<Tile>();
    private List<Tile> deathTile = new List<Tile>();

    void Awake()
    {
        //Initilize 2D array
        tiles = new Tile[rows, columns];

        //Call code that makes the grid
        MakeGrid();
        
    }

    private void Start()
    {
        ConvertTileToTrap(GetRandomTile());
    }

    private void MakeGrid()
    {
        //Nested for loops to create the rows and columns
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                //Here we want to get the size of the Tile sprite so that he can place them side by side
                float sizeX = tilePrefab.GetComponent<SpriteRenderer>().size.x;
                float sizeY = tilePrefab.GetComponent<SpriteRenderer>().size.y;
                Vector2 pos = new Vector3(originPos.x + sizeX * r, originPos.y + sizeY * c,0);

                //Here we Instantiate the GameObject and then immediately get a reference to it's Tile script.
                GameObject o = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                Tile t = o.GetComponent<Tile>();

                //We make sure to set the newly created tile in the appropriate slot in the 2D array and then name it accordingly
                tiles[r, c] = t;
                tiles[r, c].name = "[" + r.ToString() + "," + c.ToString() + "]";

            }

        }

        // We run some for loops after making the Grid to set any specific tiles.
        for (int i = 0; i < holeCount; i++) // In this for loop, i < holeCount means there will be holes less than the set range
        {
            AddHoles();
        }
        for (int i = 0; i < trapTileCount; i++) // The amount of set traps will be less than the max amount.
        {
            AddTraps();
        }
        for (int i = 0; i == goalTileCount; i++) // There will be only one goal tile set.
        {
            AddGoal();
        }
        for (int i = 0; i == deathTileCount; i++) // The amount of traps set will be the max amount in the range.
        {
            AddDeath();
        }
        // If I change the i value to a number in any of the for loops above instead of 0, the range will be ignored. 
    }

    //If we ever need the position for a tile, we can get it from one of these two functions.
    //The first one is for getting a position using the row anf column index
    public Vector3 GetTilePosition(int r, int c)
    {
        return tiles[r, c].transform.position;

    }
    //The second one is for getting a position using the tile itself
    public Vector3 GetTilePosition(Tile t)
    {
        return t.transform.position;

    }


    private void AddTraps()
    {
        //We get a random tile 
        Tile t = GetRandomTile();

        //We check that it isnt already been inluded as either a trap or Hole and that it doesnt set the player's start position 
        //as a trap. We do this by checking that, while the tile is either the origin tile, a hole or a trap, we keep getting a new tile

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || goalTile.Contains(t))
        {
            t = GetRandomTile();
        }

        //...when we break out of the while loop, it means what the random tile selected fulfills the above criteria
        //So we add it to the appropriate list, color it and set the appropriate bool to true
        trapTiles.Add(t);
        t.AdjustColor(Color.red);
        t.isTrap = true;
            

    }

    private void AddHoles()
    {
        //We get a random tile 
        Tile t = GetRandomTile();

        //We check that it isnt already been inluded as either a trap or Hole and that it doesnt set the player's start position 
        //as a trap. We do this by checking that, while the tile is either the origin tile, a hole or a trap, we keep getting a new tile

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || goalTile.Contains(t) |deathTile.Contains(t))
        {
            t = GetRandomTile();
        }

        //...when we break out of the while loop, it means what the random tile selected fulfills the above criteria
        //So we add it to the appropriate list, color it and set the appropriate bool to true
        inaccessibleTiles.Add(t);
        t.AdjustColor(Color.black);
        t.isInaccessible = true;
    }


    private Tile GetRandomTile()
    {
        //This just returns a random tile from the 2D area but using a random row and random column index
        return tiles[Random.Range(0, rows), Random.Range(0, columns)];

    }

    private void AddGoal()
    {
        Tile t = GetRandomTile();

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || goalTile.Contains(t) | deathTile.Contains(t))
        {
            t = GetRandomTile();
        }

        goalTile.Add(t);
        t.AdjustColor(Color.green);
        t.isGoal = true;
    }

    private void AddDeath()
    {
        Tile t = GetRandomTile();

        while (t == tiles[0, 0] || inaccessibleTiles.Contains(t) || trapTiles.Contains(t) || goalTile.Contains(t) | deathTile.Contains(t))
        {
            t = GetRandomTile();
        }

        deathTile.Add(t);
        t.AdjustColor(Color.yellow);
        t.isDeath = true;

        
    }

    

    public void ConvertTileToTrap(Tile t)
    {
        t.AdjustColor((Color.red));
        t.isTrap = true;
        trapTiles.Add(t);
        randomTrapTile = t;
    }

    public void ResetTile(Tile t)
    {
        t.AdjustColor((Color.white));
        t.isTrap = false;
        if (trapTiles.Contains(t))
        {
            trapTiles.Remove(t);
        }
    }


    public void TrapConvertTimer()
    {
        if (timeElapsed > 0)
        {
            ConvertTileToTrap(GetRandomTile());

        }
        else if (timeElapsed <= 0)
        {
            ResetTile(GetRandomTile());
            
        }
    }

    private void Update()
    {

        if (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            ResetTile(randomTrapTile);
            ConvertTileToTrap(GetRandomTile());
            timeElapsed = 0;
            randomTrapTile.isTrap = true;
            player.ProcessTileEvents();
        }

    }
}
