using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextI18N : Text
{
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
