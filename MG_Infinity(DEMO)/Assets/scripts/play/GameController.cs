using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject pause_scene;
	// Use this for initialization
	void Start () {
		pause_scene.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onclick_pause() {
		pause_scene.SetActive(true);

	}
	public void onclick_select() {
		SceneManager.LoadScene("select");
	}

	public void onclick_retry() {
		SceneManager.LoadScene("play");
	}

	public void onclick_continue() {
		
	}
}
