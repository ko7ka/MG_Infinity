using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	//[Serializable]// <-JSONへの変換に必要
	public class listSongsInformation{
		public int id;
		public string composer;
		public string title; 
		public int play_count; 
		public int[] difficulty; 
	}

	//[Serializable]
	public class songsInformation {
		public float hit_decision;
		public float speed;
		public listSongsInformation[] listSI;
	}
	


	// Use this for initialization
	void Start () {
		songsInformation si = new songsInformation();

		string json = Resources.Load("all_preference.json").ToString();

		Debug.Log(json);
		//string json = prefDataFileDescriptor.ToString();

		//Debug.Log(json);

		//JsonUtility.FromJsonOverwrite(json, si);

		//Debug.Log(si.hit_decision);
		//Debug.Log(si.speed);
		//Debug.Log(si.listSI[0].composer);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
