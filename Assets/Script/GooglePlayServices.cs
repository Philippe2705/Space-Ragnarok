using UnityEngine;
using GooglePlayGames;
using System.Collections;

public class GooglePlayServices : MonoBehaviour {

    public string leaderBoard = "CgkIr9XZ1cgTEAIQBg";
    public string[] success = new string[] { "CgkIr9XZ1cgTEAIQAQ" /*First Ship*/, "CgkIr9XZ1cgTEAIQAg" /*A warrior begining*/, "CgkIr9XZ1cgTEAIQAw"/*War machine*/, "CgkIr9XZ1cgTEAIQBA"/*Ragnarok*/, "CgkIr9XZ1cgTEAIQBQ"/*Tornado*/ };

	// Use this for initialization
	void Start ()
    {
        if (Application.isEditor) { return;}
        PlayGamesPlatform.Activate();
        UpdateLeaderBoard();
	}

    public void ConnectToGooglePlay()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("You are connected");
            }
            else
            {
                Debug.Log("The connection failed");
            }
        }
        );
    }
    public void DisconectFromGooglePlay()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }
    public void UpdateLeaderBoard()
    {
        Social.ReportScore(UserData.GetCredits(), leaderBoard, (bool success) => {
            // handle success or failure
        });
    }
    public void UnlockAchievment(int achievementIDComplete)
    {
        Social.ReportProgress(success[achievementIDComplete], 100.0f, (bool successfull) =>
        {
            Debug.Log("Achievement " + (achievementIDComplete + 1) + " Completed !");
        }
        );
    }
    public void ShowLeaderBoard()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderBoard);
    }
    public void SaveGameToCloud()
    {

    }
}
