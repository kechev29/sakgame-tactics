using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class TileMap : MonoBehaviour {

	//public GameObject selectedUnit;

	public TileType[] tileTypes;

	public int[,] tiles;
	public Node[,] graph;

	public event Action<int, int> OnTileClick;

	int mapSizeX = 40;
	int mapSizeY = 40;

	void Awake() {
		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
	}

	void GenerateMapData() {
		// Allocate our map tiles
		tiles = new int[mapSizeX,mapSizeY];
		
		int x,y;
		
		// Initialize our map tiles to be grass
		for(x=0; x < mapSizeX; x++) {
			for(y=0; y < mapSizeX; y++) {
				tiles[x,y] = 0;
			}
		}

        //Agua - Charco

        for (x = 5; x <= 8; x++)
        {
            for (y = 12; y <= 18; y++)
            {
                tiles[x, y] = 1;
            }
        }
        for (x = 9; x <= 13; x++)
        {
            for (y = 15; y <= 18; y++)
            {
                tiles[x, y] = 1;
            }
        }
        for (x = 9; x <= 11; x++)
        {
            for (y = 14; y <= 14; y++)
            {
                tiles[x, y] = 1;
            }
        }
        for (x = 23; x <= 26; x++)
        {
            for (y = 21; y <= 21; y++)
            {
                tiles[x, y] = 1;
            }
        }
        for (x = 21; x <= 27; x++)
        {
            for (y = 22; y <= 24; y++)
            {
                tiles[x, y] = 1;
            }
        }

        //Muros

        for (x = 0; x <= 4; x++)
        {
            for (y = 0; y <= 29; y++)
            {
                tiles[x, y] = 2;
            }
        }
        for (x = 5; x <= 33; x++)
        {
            for (y = 25; y <= 29; y++)
            {
                tiles[x, y] = 2;
            }
        }
        for (x = 29; x <= 33; x++)
        {
            for (y = 0; y <= 24; y++)
            {
                tiles[x, y] = 2;
            }
        }
        for (x = 5; x <= 28; x++)
        {
            for (y = 0; y <= 4; y++)
            {
                tiles[x, y] = 2;
            }
        }
    }

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {

		TileType tt = tileTypes[ tiles[targetX,targetY] ];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		if( sourceX!=targetX && sourceY!=targetY) {
			// We are moving diagonally!  Fudge the cost for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}

		return cost;

	}

	void GeneratePathfindingGraph() {
		// Initialize the array
		graph = new Node[mapSizeX,mapSizeY];

		// Initialize a Node for each spot in the array
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {
				graph[x,y] = new Node();
				graph[x,y].x = x;
				graph[x,y].y = y;
			}
		}

		// Now that all the nodes exist, calculate their neighbours
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {

				// This is the 4-way connection version:
				if(x > 0)
					graph[x,y].neighbours.Add( graph[x-1, y] );
				if(x < mapSizeX-1)
					graph[x,y].neighbours.Add( graph[x+1, y] );
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );


				// This is the 8-way connection version (allows diagonal movement)
				// Try left
				//if(x > 0) {
				//	graph[x,y].neighbours.Add( graph[x-1, y] );
				//	if(y > 0)
				//		graph[x,y].neighbours.Add( graph[x-1, y-1] );
				//	if(y < mapSizeY-1)
				//		graph[x,y].neighbours.Add( graph[x-1, y+1] );
				//}

				// Try Right
				//if(x < mapSizeX-1) {
				//	graph[x,y].neighbours.Add( graph[x+1, y] );
				//	if(y > 0)
				//		graph[x,y].neighbours.Add( graph[x+1, y-1] );
				//	if(y < mapSizeY-1)
				//		graph[x,y].neighbours.Add( graph[x+1, y+1] );
				//}

				// Try straight up and down
				//if(y > 0)
				//	graph[x,y].neighbours.Add( graph[x, y-1] );
				//if(y < mapSizeY-1)
				//	graph[x,y].neighbours.Add( graph[x, y+1] );

				// This also works with 6-way hexes and n-way variable areas (like EU4)
			}
		}
	}

	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y) {
		return new Vector3(x, y, 0);
	}

	public bool UnitCanEnterTile(int x, int y) {

		// We could test the unit's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.

		return tileTypes[ tiles[x,y] ].isWalkable && graph[x, y].walkable;
	}

	private List<Node> GeneratePathTo(int sourceX, int sourceY, int targetX, int targetY, Unit unit) {
		// Clear out our unit's old path.
		unit.currentPath = null;

		if( UnitCanEnterTile(targetX,targetY) == false ) {
			
			// We probably clicked on a mountain or something, so just quit out.
			return null;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    sourceX,
		                    sourceY
		                    ];
		
		Node target = graph[
		                    targetX, 
		                    targetY
		                    ];
		
		dist[source] = 0;
		prev[source] = null;

		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach(Node v in graph) {
			if(v != source) {
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach(Node possibleU in unvisited) {
				if(u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}

			if(u == target) {
				break;	// Exit the while loop!
			}

			unvisited.Remove(u);

			foreach(Node v in u.neighbours) {
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.

		if(prev[target] == null) {
			// No route between our target and the source
			return null;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}

		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!

		currentPath.Reverse();

		return currentPath;

		//selectedUnit.GetComponent<Unit>().currentPath = currentPath;
	}

	public List<Node> CheckPathPrice(int sourceX, int sourceY, int targetX, int targetY, int availablePoints, Unit unit)
    {
		List<Node> path = GeneratePathTo(sourceX, sourceY, targetX, targetY, unit);

		float currentPrice = ReturnPathPrice(sourceX, sourceY, targetX, targetY, unit);

		if (currentPrice <= availablePoints) {
			return path; 
		}
		else return null;
	}

	public float ReturnPathPrice(int sourceX, int sourceY, int targetX, int targetY, Unit unit)
	{
		List<Node> path = GeneratePathTo(sourceX, sourceY, targetX, targetY, unit);

		float currentPrice = 0;

		if (path != null)
		{
			for (int i = 0; i < path.Count - 1; i++)
			{
				currentPrice += CostToEnterTile(path[i].x, path[i].y, path[i + 1].x, path[i + 1].y);
			}
		}

		return currentPrice;
	}

	public void BroadcastClickedTile(int X, int Y)
    {
		OnTileClick?.Invoke(X, Y);
    }

}
