using System;
using UnityEngine;
using System.Collections.Generic;

public static class I18N
{

    private static Dictionary<string, string> fields = new Dictionary<string, string>();

    public static void LoadLanguage(string lang)
    {
        fields.Clear();
        TextAsset textAsset = Resources.Load<TextAsset>("I18N/" + lang);
        string allTexts = "";
        if (textAsset == null)
        {
            textAsset = Resources.Load<TextAsset>("I18N/en");
        }
        allTexts = textAsset.text;
        string[] lines = allTexts.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        string key, value;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("#"))
            {
                key = lines[i].Substring(0, lines[i].IndexOf("="));
                value = lines[i].Substring(lines[i].IndexOf("=") + 1,
                        lines[i].Length - lines[i].IndexOf("=") - 1).Replace("\\n", Environment.NewLine);
                fields.Add(key, value);
            }
        }
    }

    public static string GetTranslation(string word)
    {
        string t;
        if (fields.TryGetValue(word, out t))
        {
            return t;
        }
        else
        {
            Debug.LogError("No translation!");
            return word;
        }
    }
}
