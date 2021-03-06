﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	private float time = 0; //used to measure the time.
	private int id, difficulty; //used to load specified musical score.
	private string composer, title; //used to load specified musical score.
	private Phase phase; //used to express the progress status.
	ChartDataBody chart = new ChartDataBody();
	public GameObject pauseScene;
	public GameObject pauseButton;
	public GameObject touchToStart;
	public GameObject timerController;
	public GameObject timer;
	public GameObject startScene;



	enum Phase {
		beforeTouchToStart,
		afterTouchToStart,
		playing,
		pausing,
		ending,
	}

	[System.Serializable]
	public class ChartDataBody {
		public string version;             // s.t. "1#1.0.1":"devteamによる譜面の仕様ver#各譜面のversion"
		public int[] difficulty;           //[(0=easy | 1=medium | 2=hard | 3=infinity)(int), 1-10(int)]
		public float[][] notesTime;        //[[starttime, endtime], ...]
		public string[] route;             //[0123456789ABCDEF] \hex, 0==F
		public int bpm;                    //[1-320]
		public int[][] setBpm;
		public float offset; // [](単位s)
	}


	void Start () {
		// init chart
		initChart();

		// set 

		pauseScene.SetActive(false);
		pauseButton.SetActive(false);
		timerController.SetActive(false);
		timer.SetActive(false);

		phase = Phase.beforeTouchToStart;
	}
	
	// Update is called once per frame
	void Update () {
		switch (phase)
		{
			case Phase.beforeTouchToStart:
				if (Application.isEditor) {
					if (Input.GetButtonUp("Fire1")) {
						beforeTouchToStart();
					}
				} else {
					Touch[] touches = Input.touches;

					if (touches != null) {
						beforeTouchToStart();
					}
				}
				break;
			case Phase.afterTouchToStart:
				afterTouchToStart();
				break;
			case Phase.playing:
				break;
			case Phase.pausing:
				break;
			case Phase.ending:
				break;
		}
	}

	void beforeTouchToStart() {
		touchToStart.SetActive(false);
		timerController.SetActive(true);
		timer.SetActive(true);
		phase = Phase.afterTouchToStart;	
	}

	void afterTouchToStart() {
		if (timerController.activeInHierarchy == false) {
			pauseButton.SetActive(true);
			phase = Phase.playing;
		}
	}

	public void onclickPause() {
		pauseScene.SetActive(true);

	}
	public void onclickSelect() {
		SceneManager.LoadScene("select", LoadSceneMode.Single);
	}

	public void onclickRetry() {
		SceneManager.LoadScene("play", LoadSceneMode.Single);
	}

	public void onclickContinue() {
		
	}

	void initChart() {
		difficulty = SceneController.getDifficalty();
		id = SceneController.getID();
		composer = SceneController.getComposer();
		title = SceneController.getTitle();

		string difficultyFolder = "";

		switch (difficulty) {
			case 0:
				difficultyFolder = "easy";
				break;
			case 1:
				difficultyFolder = "medium";
				break;
			case 2:
				difficultyFolder = "hard";
				break;
			case 3:
				difficultyFolder = "infinity";
				break;
		}
		
		string path = "chart/" + difficultyFolder + "/" + id.ToString("D5") + "_" + composer + "_" + title;
		string json = Resources.Load(path).ToString();
		JsonUtility.FromJsonOverwrite(json, chart);
		
		Debug.Log(json);
		Debug.Log(chart.difficulty);
		Debug.Log(chart.offset);
	}
}
