using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class SettingsGoogle : MonoBehaviour
{
    public string leaderBoard = "CgkIr9XZ1cgTEAIQBg";
    AnalyticsScript analytics;
    public GoogleUserInfos googleUserInfos;
    public GameObject userInfosUI;

#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
    void Awake()
    {
        analytics = GetComponent<AnalyticsScript>();
        GameObject.Find("ShowleaderBoard").GetComponent<Button>().onClick.AddListener(() => { ShowLeaderBoard(); });
        GameObject.Find("DisconnectFromGooglePlay").GetComponent<Button>().onClick.AddListener(() => { DisconnectFromGooglePlay(); });
        GameObject.Find("ConnecttoGooglePlay").GetComponent<Button>().onClick.AddListener(() => { ConnectToGooglePlay(); });
    }

    public void DisconnectFromGooglePlay()
    {
        PlayGamesPlatform.Instance.SignOut();
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
                GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "";
            }

        }
        );
    }

    public void ShowLeaderBoard()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderBoard);
    }

    public void UpdateLeaderBoard()
    {
        Social.ReportScore(UserData.GetCredits(), leaderBoard, (bool success) =>
        {
            // handle success or failure
        });
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
            GameObject.Find("ErrorCatcher").GetComponent<Text>().text = "";
            userInfosUI.GetComponentInChildren<Text>().text = "Not connected";
        }
    }
#endif
}
