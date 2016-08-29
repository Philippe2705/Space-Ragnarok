using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextI18N : Text
{
    protected override void Awake()
    {
        base.Awake();
        I18N.LoadLanguage("en");
    }

    public override string text
    {
        get
        {
            if (Application.isPlaying)
            {
                return I18N.GetTranslation(m_Text);
            }
            else
            {
                return m_Text;
            }
        }

        set
        {
            m_Text = value;
        }
    }
}
