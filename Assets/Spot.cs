using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    //public float pheromone {
    //    get {
    //        return _pheromone;
    //    }
    //    set {
    //        if (value > 10000)
    //            value = 10000;
    //        if (value < 1)
    //            value = 1;
    //        _pheromone = value;
    //    }
    //}
    //private float _pheromone = 1.0f;
    private float _pheromone1 = 1.0f;
    private float _pheromone2 = 1.0f;
    public float pheromone1 {
        get {
            return _pheromone1;
        }
        set {
            if (value > 10000)
                value = 10000;
            if (value < 1)
                value = 1;
            _pheromone1 = value;
        }
    }
    public float pheromone2 {
        get {
            return _pheromone2;
        }
        set {
            if (value > 10000)
                value = 10000;
            if (value < 1)
                value = 1;
            _pheromone2 = value;
        }
    }

    public enum SPOT_TYPE { SPOT, START, STOP, WALL };

    public SPOT_TYPE spotType = SPOT_TYPE.SPOT;

    // Use this for initialization
    void Start() {
        this.GetComponent<MeshRenderer>().material.color = Color.black;
        if (spotType == SPOT_TYPE.START)
            this.GetComponent<MeshRenderer>().material.color = Color.blue;
        else if (spotType == SPOT_TYPE.STOP)
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
        else if (spotType == SPOT_TYPE.WALL)
            this.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update() {
        //if (spotType == SPOT_TYPE.START)
        //	this.GetComponent<MeshRenderer> ().material.color = Color.blue;
        //else if(spotType == SPOT_TYPE.STOP)
        //	this.GetComponent<MeshRenderer> ().material.color = Color.yellow;
        //if (spotType == SPOT_TYPE.SPOT)
        //    this.GetComponent<MeshRenderer>().material.color = new Color(pheromone / 500, pheromone / 500, pheromone / 500);
        if (spotType == SPOT_TYPE.SPOT)
            this.GetComponent<MeshRenderer>().material.color = new Color(pheromone1 / 1000, (pheromone1 + 0.5f * pheromone2) / 1000, (0.2f * pheromone1 + pheromone2) / 1000);
    }
}
