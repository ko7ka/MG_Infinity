﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIndicator : MonoBehaviour {
    static int score;
	private Text text;
	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        score = 244242;
		text.text = score.ToString();
	}
}
