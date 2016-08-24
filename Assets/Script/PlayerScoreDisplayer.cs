using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreDisplayer : MonoBehaviour
{
    public Text PseudoText;
    public Text NumberOfHitsTexts;
    public Text NumberOfKillText;
    public Text KilledByText;
    public Text XpText;

    public PlayerScore PlayerScore;

    void Update()
    {
        if (PlayerScore != null)
        {
            PseudoText.text = PlayerScore.Pseudo;
            NumberOfHitsTexts.text = PlayerScore.NumberOfHits.ToString();
            NumberOfKillText.text = PlayerScore.NumberOfKill.ToString();
            KilledByText.text = PlayerScore.KilledBy;
            XpText.text = PlayerScore.Xp.ToString();
        }
    }
}
