using UnityEngine;
using GooglePlayGames;
using System.Collections;

public class GooglePlayServices : MonoBehaviour {

    public string leaderBoard = "CgkIr9XZ1cgTEAIQBg";
    public string[] sucess = new string[] { "CgkIr9XZ1cgTEAIQAQ" /*First Ship*/, "CgkIr9XZ1cgTEAIQAg" /*A warrior begining*/, "CgkIr9XZ1cgTEAIQAw"/*War machine*/, "CgkIr9XZ1cgTEAIQBA"/*Ragnarok*/, "CgkIr9XZ1cgTEAIQBQ"/*Tornado*/ };

	// Use this for initialization
	void Start () {
        PlayGamesPlatform.Activate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
