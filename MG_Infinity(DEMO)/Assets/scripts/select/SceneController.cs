using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {


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

	// Use this for initialization
	void Start () {
        songsInformation si = new songsInformation();

		string json = Resources.Load("all_preference").ToString();

		JsonUtility.FromJsonOverwrite(json, si);

		Debug.Log(si.hit_decision);
		Debug.Log(si.speed);
        Debug.Log(si.list[0].composer);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
