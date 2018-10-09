using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	//他シーンに渡す変数
	public static int musicID;
	public static int difficulty;
	public static string composer;
	public static string title;
	//このシーン内で使う変数
	static int sceneDif; //scene内の難易度を扱う変数　easy:0,medium:1,hard:2,infinity:3
	static int sortMode;
	
	static float speed, hit_decision;
	 //sortした時のモード選択　sortのモードいくつあったっけ？（それぞれ割り当てるのじゃ）
	songsInformation si = new songsInformation();

	[System.Serializable]
	public class songsInformation {
		public float hit_decision;
		public float speed;
		public listSongsInformation[] list;
	}

	[System.Serializable]// <-JSONへの変換に必要
	public class listSongsInformation
	{
		public int id;
		public string composer;
		public string title;
		public int play_count;
		public int[] difficulty;
	}

	//他シーンで変数を読み込むとき用関数
	public static int getID(){
		return musicID;
	}
	public static int getDifficulty(){
		return difficulty;
	}
	public static string getComposer(){
		return composer;
	}
	public static string getTitle(){
		return title;
	}

	public static float getSpeed() {
		return speed;
	}

	public static float getDecision() {
		return hit_decision;
	}
	// Use this for initialization
	void Start () {

		string json = Resources.Load("all_preference").ToString();

		JsonUtility.FromJsonOverwrite(json, si);

		// Debug.Log(si.hit_decision);
		// Debug.Log(si.speed);
		// Debug.Log(si.list[0].artist);

		/* insert values in each variable to allow other variables to refer to them */
		/* スタート段階でselectedMusic, sceneDifの値は決まらないからこれはテストとして使う */
		int selectedMusic = 0;
		sceneDif = 0;
		musicID = si.list[selectedMusic].id;
		difficulty = sceneDif;
		composer = si.list[selectedMusic].composer;
		title = si.list[selectedMusic].title;
		speed = si.speed;
		hit_decision = si.hit_decision;


		//他に必要な処理
		/*
		曲を回る箱の中にそれぞれ入れる・・・ぐらい？
		*/
	}

	// Update is called once per frame
	void Update () {
		//必要な処理
		/*
		曲を回る箱の中に入れるんだけど、難易度用変数sceneDifとsortの変数sortModeを食わせるのじゃ！（どうするかわからんのじゃ！）
		選択されている曲の詳細を更新（変数に直で代入するかな）
		入力に沿って箱を回す
		どうでもいいけどマカロン食べたい
		*/
	}
	
	//returnボタン
	void OnClickReturn(){
		SceneManager.LoadScene("start",LoadSceneMode.Single);
	}

	public void OnClickPlay(){
		SceneManager.LoadScene("play", LoadSceneMode.Single);
	}

	//easyボタン
	void OnClickEasy(){
		sceneDif = 0;
	}
	//mediumボタン
	void OnClickMed(){
		sceneDif = 1;
	}
	//hardボタン
	void OnClickHard(){
		sceneDif = 2;
	}
	//infinityボタン
	void OnClickInf(){
		sceneDif = 3;
	}

	//sortボタン
	void OnClickSort(){
		//必要な処理
		/*
		sortする。以上
		(どうやってソートしようか)
		*/
	}
}
