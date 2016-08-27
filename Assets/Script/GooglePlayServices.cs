using UnityEngine;
using GooglePlayGames;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System;

public class GooglePlayServices : MonoBehaviour
{

    public string leaderBoard = "CgkIr9XZ1cgTEAIQBg";
    public string[] success = new string[] { "CgkIr9XZ1cgTEAIQAQ" /*First Ship*/, "CgkIr9XZ1cgTEAIQAg" /*A warrior begining*/, "CgkIr9XZ1cgTEAIQAw"/*War machine*/, "CgkIr9XZ1cgTEAIQBA"/*Ragnarok*/, "CgkIr9XZ1cgTEAIQBQ"/*Tornado*/ };
    public GoogleUserInfos googleUserInfos;
    public Sprite defaultAvatarGoogle;
    public GameObject userInfosUI;
    public GameObject connectPopUp;
    void Awake()
    {
        if (GameObject.Find("GoogleHandler") != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.name = "GoogleHandler";
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        PlayGamesPlatform.Activate();
        ConnectToGooglePlay();
    }
    bool buttonHasBeenSet = true;
    void FixedUpdate()
    {
        if (Application.loadedLevel == 0 && buttonHasBeenSet == false)
        {

            GameObject.Find("Play").GetComponent<Button>().onClick.AddListener( () =>{ CheckIfConnected(); });
            buttonHasBeenSet = true;
        }
        else { buttonHasBeenSet = false; }
        if (!Application.isEditor)
        {
            try
            {
                UpdateUserInfos();
            }
            catch (Exception e)
            {
                print("Error Updating Googles infos");
                GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "Error Updating Googles infos";
            }
            try
            {
                UpdateLeaderBoard();
            }
            catch
            {
                GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "Error Updating LeaderBoard";
            }

        }
        if(Application.loadedLevel == 0 && gameObject.name == "GoogleHandler")
        {
            if (connectPopUp == null)
            {
                connectPopUp = GameObject.Find("ConnectToGoogle");
            }
        }
    }

    public void ConnectToGooglePlay()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("You are connected");
                GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "";
                GetComponent<AnalyticsScript>().SendSystemInfos(Social.localUser.userName, Social.localUser.id);
            }
            else
            {
                Debug.Log("The connection failed");
            }

        }
        );
    }
    public void CheckIfConnected()
    {
        if (Social.localUser.authenticated || Application.isEditor)
        {
            GameObject.Find("Configuration").GetComponent<LoadSceneScript>().LoadScene(1);
        }
        else
        {
            connectPopUp.SetActive(true);
        }
    }
    public void UpdateUserInfos()
    {

        if (gameObject.name == "GoogleHandler" && Application.loadedLevel == 0)
        {
           
            if (userInfosUI == null)
            {
                userInfosUI = GameObject.Find("GoogleInfos");
            }
            try
            {
                if (Social.localUser.authenticated)
                {

                    googleUserInfos.googleUserName = Social.localUser.userName;
                    googleUserInfos.googleUserID = Social.localUser.id;
                    googleUserInfos.googleAvatar = Social.localUser.image;
                    userInfosUI.transform.FindChild("username").GetComponent<Text>().text = googleUserInfos.googleUserName;
                    userInfosUI.transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(googleUserInfos.googleAvatar, userInfosUI.transform.FindChild("Image").GetComponent<Rect>(), userInfosUI.transform.FindChild("Image").transform.position);
                }
                else
                {
                    GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "Not Connected to google";
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
public void CloseConnectToGooglePopUp()
    {
        connectPopUp.SetActive(false);
    }
public void DisconectFromGooglePlay()
{
        PlayGamesPlatform.Instance.SignOut();

}
public void UpdateLeaderBoard()
{
    Social.ReportScore(UserData.GetCredits(), leaderBoard, (bool success) =>
    {
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
