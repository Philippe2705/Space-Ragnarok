using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class I18N
{

    private static Dictionary<string, string> fields = new Dictionary<string, string>();
    private static string currentLanguage;

    public static void LoadLanguage(string lang)
    {
        currentLanguage = lang;
        fields.Clear();
        TextAsset textAsset = Resources.Load<TextAsset>("I18N/" + lang);
        string allTexts = "";
        if (textAsset == null)
        {
            textAsset = Resources.Load<TextAsset>("I18N/english");
            currentLanguage = "english";
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
                if (!fields.ContainsKey(key))
                {
                    fields.Add(key, value);
                }
                else
                {
                    Debug.Log(key);
                    Debug.Log(value);
                }
            }
        }
    }

    public static string GetTranslation(string word)
    {
        word = word.Trim();
        if (word == "")
        {
            return "";
        }
        else
        {
            string t;
            if (fields.TryGetValue(word, out t))
            {
                return t;
            }
            else
            {
#if UNITY_EDITOR
                LoadLanguage(currentLanguage);
                word = word.Trim();
                if (fields.TryGetValue(word, out t))
                {
                    return t;
                }
                else
                {
                    Debug.LogError("No translation for " + word + " !");
                    TextAsset textAsset = Resources.Load<TextAsset>("I18N/" + currentLanguage);
                    var s = "\r\n" + word + "=" + word;
                    File.AppendAllText(UnityEditor.AssetDatabase.GetAssetPath(textAsset), s);
                    return word;
                }
#endif
            }
        }
    }
}
