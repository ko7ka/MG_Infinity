using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {
	public Text timerText;
	public float totalTime;
	public GameObject startScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		totalTime -= Time.deltaTime;
		
		if (totalTime >= 1.0) {
			timerText.text = "Ready";
		} else if (totalTime >= 0) {
			timerText.text = "Go";
		} else {
			startScene.SetActive(false);
		}
	}
}
