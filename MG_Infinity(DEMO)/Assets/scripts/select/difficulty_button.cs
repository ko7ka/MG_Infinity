using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum difficulty
{
    easy,
    medium,
    hard
}

static class difExt
{
    public static string DisplayName(this difficulty dif)
    {
        string[] names = { "easy", "medium", "hard" };
        return names[(int)dif];
    }
}

public class difficulty_button : MonoBehaviour {
   

    difficulty dif;
    int click_count = 1;
	// Use this for initialization
	void Start () {
        dif = difficulty.medium;
        return;
	}
	
	// Update is called once per frame
	void Update () {
        return;
	}

    public void OnClick()
    {
        dif = (difficulty)((++click_count) % 3);
        this.gameObject.GetComponentsInChildren<Text>()[0].text = difExt.DisplayName(dif);
    }
}
