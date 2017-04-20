using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACOA : MonoBehaviour {

    public static ACOA instance;

    public List<List<Spot>> map;

    List<Ant> ants;

    public Spot spotPrefab;
    public Ant antPrefab;
    public GameObject spotsParrent;
    public GameObject antsParrent;

    //parameter
    public float alpha = 0;
    public float beta = 0;
    float pheromone_vaporization = 0.97f; /*** 費洛蒙揮發係數 ***/
    const float Q = 100f;
    bool running = false;
    readonly int countOfAnts = 100;

    //public static int mapSize = 20;
	public static int mapSize = 25;

	string maze = "11101010101110111110101111011110100101010101110001110100110110000101001110011101110011101001011100010111010101001110010000101100110101101001011110010101101101101001011010000101100011111100011001101001101101110100111001101001000101011101100001001101100001010011111010011000011100010101110111001010100001101010001111111111111111111111111111";
    //string maze = "11101011101010101011101011011010011010111000001100110101011010010111100001101101011010010101101110100111111010010101100101101000110111000001110001110011101101101010010111000111000111110011010100011001101001110110010110100001111001001101010011101010111000100111011101011010010110110001110101001010000110000110001111111111111111111111111111";

    // Use this for initialization
    void Start() {
        ants = new List<Ant>();

        int startX = 1;
        int startY = 1;

        int stopX = Random.Range(10, mapSize);
        //int stopX = Random.Range(1, 10);
        int stopY = Random.Range(10, mapSize);
        //int stopY = Random.Range(1, 10);

        //creation map
        map = new List<List<Spot>>();
        for (int i = 0; i < mapSize; i++) {
            map.Add(new List<Spot>());
            for (int j = 0; j < mapSize; j++) {
                Spot spot = GameObject.Instantiate<Spot>(spotPrefab);
                spot.transform.SetParent(spotsParrent.transform);
                spot.transform.position = new Vector3(i, j, 3);
                map[i].Add(spot);
            }
        }

        // Set wall blocks
        int maze_k = 0;
        for (int i = 0; i < mapSize; i += 2) {
            for (int j = 0; j < mapSize; j += 2) {
                map[j][mapSize - i - 1].spotType = Spot.SPOT_TYPE.WALL;
                if (maze.Substring(maze_k, 1).Equals("1") && j + 1 < mapSize)
                    map[j + 1][mapSize - i - 1].spotType = Spot.SPOT_TYPE.WALL;
                if (maze.Substring(maze_k + 1, 1).Equals("1") && i + 1 < mapSize)
                    map[j][mapSize - i - 2].spotType = Spot.SPOT_TYPE.WALL;
                maze_k += 2;
            }
        }

        //for (int i = 0; i < 40; i++) {
        //    map[Random.Range(1, mapSize)][Random.Range(1, mapSize)].spotType = Spot.SPOT_TYPE.WALL;
        //}

        map[startX][startY].spotType = Spot.SPOT_TYPE.START;
        map[23][23].spotType = Spot.SPOT_TYPE.STOP;

        //creation ant
        for (int i = 0; i < countOfAnts; i++) {
            Ant ant = GameObject.Instantiate<Ant>(antPrefab);
            ant.gameObject.transform.SetParent(antsParrent.transform);
            ant.currentSpot = ant.prevSpot = map[startX][startY];
            //ant.currentSpot = ant.prevSpot = map[Random.Range(0, mapSize)][Random.Range(0, mapSize)];
            ants.Add(ant);
        }

        instance = this;
    }

    // Update is called once per frame
    void Update() {
        if (running == true) {
            string antsDis = "";
            foreach (Ant ant in ants) {
                ant.walk();
                antsDis += ant.distance + "  ";
            }
            //Debug.Log(antsDis);
            string phe = "";
            for (int i = 0; i < mapSize; i++) {
                for (int j = 0; j < mapSize; j++) {
                    map[i][j].pheromone1 = pheromone_vaporization * map[i][j].pheromone1;
                    map[i][j].pheromone2 = pheromone_vaporization * map[i][j].pheromone2;
                    phe += map[i][j].pheromone1 + "\t";
                    phe += map[i][j].pheromone2 + "\t";
                }

                phe += "\n";
            }

            //Debug.Log(phe);

            //Debug.Log((Q));
            foreach (Ant ant in ants) {
                if (ant.getFood)
                    ant.currentSpot.pheromone2 += (Q / (Mathf.Pow(ant.distance, 1)));
                else
                    ant.currentSpot.pheromone1 += (Q / (Mathf.Pow(ant.distance, 1)));

            }
        }
    }

    public void btnRunning() {
        running = !running;
    }

}
