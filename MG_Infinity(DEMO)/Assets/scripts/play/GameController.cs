using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System;

public class GameController : MonoBehaviour {

	private int scoreValue;
	private double time = 0f; //used to measure the time.
	private int id, difficulty; //used to load specified musical score.
	private string composer, title; //used to load specified musical score.
	private float speed, hit_decision;
	private int numberOfInstantiatedNotes = 0;
	private Phase phase; //used to express the progress status.
	ChartDataBody chart = new ChartDataBody();
	public float radius; //radius of the two circles.
	public GameObject pauseScene;
	public GameObject pauseButton;
	public GameObject touchToStart;
	public GameObject timerController;
	public GameObject timer;
	public GameObject startScene;
	// public GameObject audioObject;
	public AudioSource audioSource;
	public Text scoreLabel;
	//private int[,] routeRadian;
	private Dictionary<string, int[]> routeDict;
	



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
		public double[][] notesTime;        //[[starttime, endtime], ...]
		public string[] route;             //[0123456789ABCDEF] \hex, 0==F
		public int bpm;                    //[1-320]
		public int[][] setBpm;
		public float offset; // [](単位s)
	}


	void Start () {
		// init chart
		initChart();

		// init audioSource
		initAudioSource();

		// init scoreLabel
		initScoreLabel();
		
		// initialize routeDict
		initRouteDict();

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
				playing();
				break;
			case Phase.pausing:
				break;
			case Phase.ending:
				ending();
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
			time += Time.deltaTime;
		}

		if (time > chart.offset) {
			time = 0;
			pauseButton.SetActive(true);
			phase = Phase.playing;
			audioSource.Play();
		}
	}

	void playing() {
		
		//Debug.Log("notes speed is:" + speed);
		//Debug.Log("number of notes:" + chart.notesTime.Length);
		
		int scanningRange;
		if (chart.notesTime.Length - numberOfInstantiatedNotes >= 10) {
			scanningRange = 10;
		} else {
			scanningRange = chart.notesTime.Length - numberOfInstantiatedNotes;
		}

		for (int i = 0; i < scanningRange; i++) {
			if (chart.notesTime[numberOfInstantiatedNotes][0] - (radius / speed) < time) {
				instantiateNote(numberOfInstantiatedNotes);
				numberOfInstantiatedNotes++;
			}
		}

		if (time >= audioSource.clip.length) {
			audioSource.Stop();
			phase = Phase.ending;
		}
		time += Time.deltaTime;
	}

	void ending() {
		Debug.Log(time);
		Debug.Log(audioSource.clip.length);
		SceneManager.LoadScene("result", LoadSceneMode.Single);
	}

	public void onclickPause() {
		phase = Phase.pausing;
		if (audioSource.isPlaying) {
			audioSource.Pause();
		}
		pauseScene.SetActive(true);

	}
	public void onclickSelect() {
		SceneManager.LoadScene("select", LoadSceneMode.Single);
	}

	public void onclickRetry() {
		SceneManager.LoadScene("play", LoadSceneMode.Single);
	}

	public void onclickContinue() {
		phase = Phase.playing;
		audioSource.Play();
		pauseScene.SetActive(false);
	}

	void initChart() {
		if (Application.isEditor) {
			difficulty = 0;
			id = 1;
			composer = "2bnsn";
			title = "ugokuugoku";
			speed = 5;
			hit_decision = 0.1f;
		} else {

			difficulty = SceneController.getDifficulty();
			id = SceneController.getID();
			composer = SceneController.getComposer();
			title = SceneController.getTitle();
			speed = SceneController.getSpeed();
			hit_decision = SceneController.getDecision();
		}
		

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
		
		chart = JsonMapper.ToObject<ChartDataBody>(json);
		
		Debug.Log("chart in json format:" + json);
		Debug.Log("difficulty: " + chart.difficulty[0] + chart.difficulty[1]);
		Debug.Log("offset to start: " + chart.offset);
		Debug.Log("route: " + chart.route[0]);
		Debug.Log("number of notes: " + chart.notesTime.Length);
		
	}

	void initAudioSource() {
		//audioSource = audioObject.GetComponent<AudioSource>();
		string path = "music/" + id.ToString("D5") + "_" + composer + "_" + title;
		audioSource.clip = Resources.Load<AudioClip>(path);
	}

	void initScoreLabel() {
		scoreValue = 0;
		scoreLabel.text = scoreValue.ToString("D6");
	}
	
	void initRouteDict(){
		// 	Who am I? - Human No 0-F wo CockDo 2 HengKang through
		// 3: 0,1，2  どっちの円から出るか、してん、しゅうてんのかくど
		// chart.route
		routeDict = new Dictionary<string, int[]>();
		//routeRadian = new int[chart.route.Length,];

		routeDict.Add("01", new int[3] {1, 180, 135});
		routeDict.Add("12", new int[3] {1, 135, 90});
		routeDict.Add("23", new int[3] {1, 90, 45});
		routeDict.Add("34", new int[3] {1, 45, 0});
		routeDict.Add("45", new int[3] {1, 360, 315});
		routeDict.Add("56", new int[3] {1, 315, 270});
		routeDict.Add("67", new int[3] {1, 270, 225});
		routeDict.Add("70", new int[3] {1, 225, 180});
		routeDict.Add("F8", new int[3] {0, 0, 45});
		routeDict.Add("89", new int[3] {0, 45, 90});
		routeDict.Add("9A", new int[3] {0, 90, 135});
		routeDict.Add("AB", new int[3] {0, 135, 180});
		routeDict.Add("BC", new int[3] {0, 180, 225});
		routeDict.Add("CD", new int[3] {0, 225, 270});
		routeDict.Add("DE", new int[3] {0, 270, 315});
		routeDict.Add("EF", new int[3] {0, 315, 360});
		routeDict.Add("FE", new int[3] {0, 360, 315});
		routeDict.Add("ED", new int[3] {0, 315, 270});
		routeDict.Add("DC", new int[3] {0, 270, 225});
		routeDict.Add("CB", new int[3] {0, 225, 180});
		routeDict.Add("BA", new int[3] {0, 180, 135});
		routeDict.Add("A9", new int[3] {0, 135, 90});
		routeDict.Add("98", new int[3] {0, 90, 45});
		routeDict.Add("8F", new int[3] {0, 45, 0});
		routeDict.Add("07", new int[3] {1, 180, 225});
		routeDict.Add("76", new int[3] {1, 225, 270});
		routeDict.Add("65", new int[3] {1, 270, 315});
		routeDict.Add("54", new int[3] {1, 315, 360});
		routeDict.Add("43", new int[3] {1, 0, 45});
		routeDict.Add("32", new int[3] {1, 45, 90});
		routeDict.Add("21", new int[3] {1, 90, 135});
		routeDict.Add("10", new int[3] {1, 135, 180});

	}

	void instantiateNote(int id) {
		double startTime = chart.notesTime[id][0];
		double endTime = chart.notesTime[id][1];
		double dividedTime = 0;
		string route = chart.route[id];
	} 
	
}
