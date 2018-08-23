using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	private float time = 0;
	private Phase phase;

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
	void Start () {
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
		SceneManager.LoadScene("select");
	}

	public void onclickRetry() {
		SceneManager.LoadScene("play");
	}

	public void onclickContinue() {
		
	}
}
