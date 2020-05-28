using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    public GameObject wall;
    public GameObject goal;

    public GameObject bg;

    public GameObject web, insect;

    int goalCount = 0;

    bool spawnedGoal = false;
    bool redo = false;

    [Range(0,100)]
    public int randomFillPercent;

    int[,] map;
    bool [,] checkMap;


    public Sprite[] specialWallSprites = new Sprite[19];
    public Sprite solidWallSprite;

    string[] byteStrings = {
        "11111011",
        "11011111",
        "11010000",
        "01101000",
        "01101011",
        "11010110",
        "00001011",
        "00010110",
        "01111111",
        "11111110",
        "00011111",
        "11111000",
        "11101000",
        "11111001",
        "11111100",
        "11110110",
        "11010100",
        "11110100",
        "11110000",
        "11101001",
        "11101011",
        "11010111",
        "00101111",
        "10011111",
        "00010111",
        "00001111",
        "10010111",
        "10010110",
        "01101111",
        "00101011",
        "11010111",
        "00111111",
        "00101011",
        "01101001",
        "11010111"

    };

    void Start() {
        GenerateMap();

        // Tuple<int, int> one = new Tuple<int, int>(1, 1);
        // Tuple<int, int> two = new Tuple<int, int>(1, 1);
        // print (one == two);
    }

    void Update() {
        // if (Input.GetMouseButtonDown(0)) {
        //     GenerateMap();
        // }
    }

    void GenerateMap() {
        map = new int[width,height];
        redo = false;

        RandomFillMap();

        for (int i = 0; i < 5; i ++) {
            SmoothMap();
        }

        MaxMap();


        while (!spawnedGoal && !redo) {
        	// print("test");
        	int randomX = UnityEngine.Random.Range(0, width);
        	int randomY = UnityEngine.Random.Range(0, height); 

        	if (map[randomX, randomY] == 0 && PositionNotAtStart(randomX, randomY)) {
                print("print between the ifs");
                if (GetPath(randomX,randomY)) {
                    print("spawned Goal 1");
                    Vector2 pos = new Vector3(-width/2 + randomX, -height/2 + randomY);
                    Instantiate(goal, pos, Quaternion.identity);
                    spawnedGoal = true;
                    print("spawned Goal 2");
                }

                goalCount++;

                if (goalCount >= 20) {
                    redo = true;
                }
        		
        	}

            // print("still running");
        }


        if (redo) {
            GenerateMap();
        }

        else {
            InstantiateMap();
        }

        bool spawnedWeb = false;

        while (!spawnedWeb) {
            // print("test");
            int randomX = UnityEngine.Random.Range(0, width);
            int randomY = UnityEngine.Random.Range(0, height); 

            if (map[randomX, randomY] == 0 && PositionNotAtStart(randomX, randomY)) {
                    
                Vector2 pos = new Vector3(-width/2 + randomX, -height/2 + randomY);
                Instantiate(web, pos, Quaternion.identity);
                spawnedWeb = true;  
                
            }

        }

        bool spawnedBug = false;

        while (!spawnedBug) {
            // print("test");
            int randomX = UnityEngine.Random.Range(0, width);
            int randomY = UnityEngine.Random.Range(0, height); 

            if (map[randomX, randomY] == 0 && PositionNotAtStart(randomX, randomY)) {
                    
                Vector2 pos = new Vector3(-width/2 + randomX, -height/2 + randomY);
                Instantiate(insect, pos, Quaternion.identity);
                spawnedBug = true;  
                
            }

        }

    }

    bool GetPath(int goalX, int goalY) {
        bool connected = false;

        Tuple<int, int> goal = new Tuple<int, int>(goalX, goalY);
        Tuple<int, int> center = new Tuple<int, int>(width/2, height/2);

        Dictionary <Tuple<int, int>, Tuple<int, int>> cameFrom = new Dictionary <Tuple<int, int>, Tuple<int, int>>();
        Dictionary <Tuple<int, int>, int> costSoFar = new Dictionary <Tuple<int, int>, int>();

        PriorityQueue<Tuple<int, int>> frontier = new PriorityQueue<Tuple<int, int>>();

        frontier.Enqueue(goal, 0);

        cameFrom[goal] = goal;
        costSoFar[goal] = 0;

        while (frontier.Count > 0) {
            Tuple<int, int> current = frontier.Dequeue();

            //print("Frontier Count: " + frontier.Count);

            // current = frontier.Dequeue();

            //print("Frontier Count 2: " + frontier.Count);

            // current = frontier.Dequeue();
            // current = frontier.Dequeue();

            if (current == center) {
                connected = true;
                break;
            }

            List<Tuple<int, int>> neighbors = GetNeighbors(current);

            foreach (Tuple<int, int> next in neighbors) {
                int newCost = costSoFar[current] + 1;

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    int priority = newCost;

                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }

            }
            // goalCount++;
        }

        // goalCount++;
        print("Goal Placement Count: " + goalCount);
        

        return connected;
    }

    List<Tuple<int, int>> GetNeighbors(Tuple<int, int> current) {
        
        int x = current.first, y = current.second;
        //print("Y: " + y);
        //print("X: " + x);

        List<Tuple<int, int>> neighbors = new List<Tuple<int, int>>();

        for (int ii = x - 1; ii <= x + 1; ii++) {
            for (int jj = y - 1; jj <= y + 1; jj++) {
                if ((ii < 0 || ii >= width) || (jj < 0 || jj >= height)) {
                    continue;
                }

                else if (ii == x && jj == y) {
                    continue;
                }

                else if (map[ii, jj] == 0 && !checkMap[ii, jj] ) {
                    Tuple<int, int> newNeighbor = new Tuple<int, int>(ii, jj);
                    checkMap[ii, jj] = true;
                    neighbors.Add(newNeighbor);
                }

            }

        }

        return neighbors;
    }


    void RandomFillMap() {
        if (useRandomSeed) {
            // seed = Time.time.ToString();
            seed = UnityEngine.Random.Range(0, 100f).ToString();
            // seed = System
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                if (x == 0 || x == width-1 || y == 0 || y == height -1) {
                    map[x,y] = 1;
                }
                else {
                    map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0;
                }
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                int neighbourWallTiles = GetSurroundingWallCount(x,y);

                if (neighbourWallTiles > 4)
                    map[x,y] = 1;
                else if (neighbourWallTiles < 4 || !PositionNotAtStart(x, y)) //might break
                    map[x,y] = 0;

            }
        }

        // InstantiateMap();

    }

    int GetSurroundingWallCount(int gridX, int gridY) {
        int q = 1;
        int wallCount = 0;
        // for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX,neighbourY];
                    }
                }
                else {
                    wallCount ++;
                }
            }
        }

        return wallCount;
    }

    void MaxMap () {
        checkMap = new bool[width,height];
        for (int x = 0; x < width; x ++) {
           for (int y = 0; y < height; y ++) {

                checkMap[x, y] = false;
            }
        }
    }

    void InstantiateMap () {
    	if (map != null) {
            for (int x = 0; x < width; x ++) {
                for (int y = 0; y < height; y ++) {
                		// Vector2 pos = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f);
                		if (map[x, y] == 1) {
                			Vector3 pos = new Vector3(-width/2 + x, -height/2 + y, -7.5f);

                            Sprite tileSprite = GetTileSprite(x, y);

                			GameObject wallSpawn = (GameObject) Instantiate(wall, pos, Quaternion.identity);

                            wallSpawn.GetComponent<SpriteRenderer>().sprite = tileSprite;


                			wallSpawn.name = "Wall " + x + y;
                            print(wallSpawn.name);
                		}

                        Vector3 bgPos = new Vector3(-width/2 + x, -height/2 + y, -8.5f);
                        Instantiate(bg, bgPos, Quaternion.identity); //need this
                		
                    // Gizmos.color = (map[x,y] == 1)?Color.black:Color.white;
                    // 
                    // Gizmos.DrawCube(pos,Vector3.one);
                }
            }
        }
    }

    Sprite GetTileSprite(int x, int y) {
        string compareString = "";
        // print("X: " + x + " Y: " + y);
        for (int yy = y + 1; yy >= y - 1; yy--) {
            for (int xx = x - 1; xx <= x + 1; xx++) {
                if (xx == x && yy == y) {
                    // print("X: " + xx + "Y: " + yy + ", Skip!");
                    continue;
                }
                if ((xx < 0 || xx >= width) || (yy < 0 || yy >= height)) {
                    // print("X: " + xx + " Y: " + yy + ", Out of bounds");
                    compareString += "1";
                }
                else {
                    // print("X: " + xx + " Y: " + yy + ", Add");
                    compareString += map[xx, yy];
                    // compareString += "1";
                }
            }
        }

        // print(compareString);

        for (int ii = 0; ii < byteStrings.Length; ii++) {
            // print(compareString + ", " + byteStrings[ii]);
            if (compareString.Equals(byteStrings[ii])) {
                return specialWallSprites[ii];
            }
        }

        return solidWallSprite;

    }


    void OnDrawGizmos() {
        if (map != null) {
            for (int x = 0; x < width; x ++) {
                for (int y = 0; y < height; y ++) {

                    // Gizmos.color = (map[x,y] == 1)?Color.black:Color.white;
                    // Vector3 pos = new Vector3(-width/2 + x + .5f,0, -height/2 + y+.5f);
                    // Gizmos.DrawCube(pos,Vector3.one);
                }
            }
        }
    }

    bool PositionNotAtStart (int x, int y) {
    	int centerX = width/2;
    	int centerY = height/2;

    	if ((x < centerX + 5 && x > centerX - 5) && (y < centerY + 5 && y > centerY - 5)) {
    		return false;
    	}

    	return true;
    }

}