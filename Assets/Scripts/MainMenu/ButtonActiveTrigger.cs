using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonActiveTrigger : MonoBehaviour
{

    public List<Button> buttonToActivate = new List<Button>();
    public List<Button> buttonToDesactivate = new List<Button>();

    public void Trigger()
    {
        foreach (var b in buttonToDesactivate)
        {
            b.transition = Selectable.Transition.None;
        }
        foreach (var b in buttonToActivate)
        {
            b.transition = Selectable.Transition.Animation;
            b.Select();
        }
    }
}
