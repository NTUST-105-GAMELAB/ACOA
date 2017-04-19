using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour {
	public Texture2D Texture_ant;
	public float distance = 0;
    public bool getFood = false;
    public Spot currentSpot;
    public Spot prevSpot;
    List<Spot> possibleSpot = new List<Spot>();
    List<Spot> fallbackSpots = new List<Spot>();
    private float fixedPh = .0f;

    float pheromone_power = 1.25f; /*** 費洛蒙影響力係數 ***/

    // Use this for initialization
    void Start() {
		//this.GetComponent<MeshRenderer>().material.color = Color.red;
		this.GetComponent<MeshRenderer>().material.mainTexture = Texture_ant;
	}

    // Update is called once per frame
    void Update() {
        this.transform.position = currentSpot.transform.position + new Vector3(0, 0, -3);
		//this.GetComponent<MeshRenderer>().material.mainTexture;
	}

	List<float> wheel = new List<float>();

	void direction() {
		if (prevSpot.transform.position.x - currentSpot.transform.position.x > 0) {
			this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
		}
		if (prevSpot.transform.position.x - currentSpot.transform.position.x < 0) {
			this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
		}
		if (prevSpot.transform.position.y - currentSpot.transform.position.y > 0) {
			this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
		}
		if (prevSpot.transform.position.x - currentSpot.transform.position.x > 0) {
			this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}


	public void walk() {
        wheel.Clear();
        possibleSpot.Clear();
        fallbackSpots.Clear();
        int x = (int)currentSpot.transform.position.x;
        int y = (int)currentSpot.transform.position.y;

        Vector3 prevPos = prevSpot.transform.position;

        float sum = 0;

        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                // Remove oblique movements
                if ((i + j) % 2 == 0) continue;

                int xx = x + i;
                int yy = y + j;
                //if (xx >= 0 && yy >= 0 && xx < ACOA.mapSize && yy < ACOA.mapSize && !(xx == (int)prevPos.x && yy == (int)prevPos.y) && !(i == 0 && j == 0)) { 
                //看周圍費洛蒙

                if (xx == (int)prevPos.x && yy == (int)prevPos.y)//不往回走
                    continue;

                if (xx >= 0 && yy >= 0 && xx < ACOA.mapSize && yy < ACOA.mapSize) {
                    Spot nextSpot = ACOA.instance.map[xx][yy];//下一個移動的地點
                    if (nextSpot.spotType == Spot.SPOT_TYPE.WALL)
                        continue;
                    //if ((i * (x - prevPos.x) + j * (y - prevPos.y)) > 0.0f || currentSpot == prevSpot) {
                    if (true) {
                        if (getFood == false) {
                            if (nextSpot.spotType == Spot.SPOT_TYPE.STOP) {
                                prevSpot = nextSpot;
                                currentSpot = nextSpot;
                                getFood = true;
                                distance = 0.0f;
                                return;
                            }
                        }
                        else if (getFood == true)
                            if (nextSpot.spotType == Spot.SPOT_TYPE.START) {
                                prevSpot = nextSpot;
                                currentSpot = nextSpot;
                                getFood = false;
                                distance = 0.0f;
                                return;
                            }

                        possibleSpot.Add(nextSpot);
                        if (getFood)
                            sum += Mathf.Pow(ACOA.instance.map[xx][yy].pheromone1, pheromone_power) + fixedPh;
                        else
                            sum += Mathf.Pow(ACOA.instance.map[xx][yy].pheromone2, pheromone_power) + fixedPh;
                        wheel.Add(sum);
                    }
                    else {
                        fallbackSpots.Add(nextSpot);
                    }
                }
            }
        }


        if (possibleSpot.Count > 0) {
            //Spot chosenSpot = possibleSpot[Random.Range(0, possibleSpot.Count)];

            float rand = Random.Range(0.0f, sum);

            int i = 0;
            for (; i < wheel.Count; i++) {
                if (rand < wheel[i]) {
                    break;
                }
            }

            prevSpot = currentSpot;
            currentSpot = possibleSpot[i];

        }
        else if (fallbackSpots.Count > 0) {
            Spot chosenSpot = fallbackSpots[Random.Range(0, fallbackSpots.Count)];

            //prevSpot = ACOA.instance.map[x][y];
            prevSpot = currentSpot;
            currentSpot = chosenSpot;
            //Debug.Log()
        }
        else {
            currentSpot = prevSpot;
            prevSpot = ACOA.instance.map[x][y];
        }

        //Debug.Log(prevSpot.transform.position + "\t" + currentSpot.transform.position);
        distance += (currentSpot.transform.position - prevSpot.transform.position).magnitude;
		//Debug.Log(distance);
		direction();
	}
}
