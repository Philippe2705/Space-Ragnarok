using UnityEngine;
using GooglePlayGames;
using System.Collections;
using UnityEngine.UI;

public class GooglePlayServices : MonoBehaviour {

    public string leaderBoard = "CgkIr9XZ1cgTEAIQBg";
    public string[] success = new string[] { "CgkIr9XZ1cgTEAIQAQ" /*First Ship*/, "CgkIr9XZ1cgTEAIQAg" /*A warrior begining*/, "CgkIr9XZ1cgTEAIQAw"/*War machine*/, "CgkIr9XZ1cgTEAIQBA"/*Ragnarok*/, "CgkIr9XZ1cgTEAIQBQ"/*Tornado*/ };
    public GoogleUserInfos googleUserInfos;
    public Sprite defaultAvatarGoogle;
    public GameObject userInfosUI;
    void Awake()
    {
        if (GameObject.Find("GoogleHandler") != null)
        {
            Destroy(gameObject);
        }
        else
        {
            PlayGamesPlatform.Activate();
            gameObject.name = "GoogleHandler";
            if (Application.isEditor) { return; }
            DontDestroyOnLoad(gameObject);
            UpdateLeaderBoard();

            UpdateUserInfos();
        }
    }
    void Start()
    {
        try
        {
            UpdateUserInfos();
        }
        catch
        {
            GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "Error Updating Googles infos";
        }
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
    public void UpdateUserInfos()
    {
        googleUserInfos.googleUserName = Social.localUser.userName;
        googleUserInfos.googleUserID = Social.localUser.id;
        googleUserInfos.googleAvatar = Social.localUser.image;
        if (gameObject.name == "GoogleHandler" && Application.loadedLevel == 0)
        {
            try {
                if (userInfosUI == null)
                {
                    userInfosUI = GameObject.Find("GoogleInfos");
                }
                if (Social.localUser.authenticated)
                {
                    UpdateUserInfos();
                    userInfosUI.transform.FindChild("username").GetComponent<Text>().text = googleUserInfos.googleUserName;
                    userInfosUI.transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(googleUserInfos.googleAvatar, userInfosUI.transform.FindChild("Image").GetComponent<Rect>(), userInfosUI.transform.FindChild("Image").transform.position);
                }
                else
                {
                    userInfosUI.transform.FindChild("username").GetComponent<Text>().text = "Not connected to Google play";
                    userInfosUI.transform.FindChild("Image").GetComponent<Image>().sprite = defaultAvatarGoogle;
                }
            }
            catch
            {
               GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "Error Updating Googles infos UI";
            }
        }
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
    public struct GoogleUserInfos
    {
        public string googleUserName;
        public string googleUserID;
        public Texture2D googleAvatar;
    }
}
