using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System;
using System.Collections;

public class GooglePlayServices : MonoBehaviour
{

    public string leaderBoard = "CgkIr9XZ1cgTEAIQBg";
    public string[] success = new string[] { "CgkIr9XZ1cgTEAIQAQ" /*First Ship*/, "CgkIr9XZ1cgTEAIQAg" /*A warrior begining*/, "CgkIr9XZ1cgTEAIQAw"/*War machine*/, "CgkIr9XZ1cgTEAIQBA"/*Ragnarok*/, "CgkIr9XZ1cgTEAIQBQ"/*Tornado*/ };
    public GoogleUserInfos googleUserInfos;
    public Sprite defaultGoogleAvatar;
    public GameObject userInfosUI;
    public GameObject connectPopup;
    public Text errorText;

    AnalyticsScript analytics;
    Button playButton;
    void Awake()
    {
        if (FindObjectsOfType<GooglePlayServices>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SetupPlayButton();
        analytics = GetComponent<AnalyticsScript>();

        PlayGamesPlatform.Activate();
        ConnectToGooglePlay();
    }
    void Update()
    {
        UpdateUserInfos();
    }


    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SetupPlayButton();
        }
    }

    void SetupPlayButton()
    {
        if (playButton == null)
        {
            playButton = GameObject.Find("Play").GetComponent<Button>();
        }
        playButton.onClick.AddListener(() => {CheckIfConnected(); });
    
    }

    public void ConnectToGooglePlay()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                analytics.SendSystemInfos(Social.localUser.userName, Social.localUser.id);

                UpdateUserInfos();

                UpdateLeaderBoard();
            }
            else
            {
                errorText.text += "Error Connecting to Google Play" + "\n";
            }

        }
        );
    }

    public void CheckIfConnected()
    {
        if (Social.localUser.authenticated || Application.isEditor)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            connectPopup.SetActive(true);
        }
    }

    public void UpdateUserInfos()
    {
        if (Social.localUser.authenticated)
        {
            googleUserInfos.googleUserName = Social.localUser.userName;
            googleUserInfos.googleUserID = Social.localUser.id;
            googleUserInfos.googleAvatar = Social.localUser.image;
            userInfosUI.GetComponentInChildren<Text>().text = googleUserInfos.googleUserName;
            //userInfosUI.transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(googleUserInfos.googleAvatar, userInfosUI.transform.FindChild("Image").GetComponent<Rect>(), userInfosUI.transform.FindChild("Image").transform.position);
        }
        else
        {
            errorText.text += "Not Connected to Google Play\n";
            userInfosUI.GetComponentInChildren<Text>().text = "Not connected";
            userInfosUI.GetComponentInChildren<Image>().sprite = defaultGoogleAvatar;
        }
    }

    public void CloseConnectToGooglePopUp()
    {
        connectPopup.SetActive(false);
    }
    public void DisconnectFromGooglePlay()
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
