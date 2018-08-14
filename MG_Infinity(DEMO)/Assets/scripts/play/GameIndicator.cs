using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIndicator : MonoBehaviour {
    static int score;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        score = 244242;
        this.GetComponent<Text>().text = score.ToString();
	}
}
