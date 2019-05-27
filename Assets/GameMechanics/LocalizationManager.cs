using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
/*
    Localization class based on instructor written version 

    - Original by Dustin Carroll
    - Version 2 by John Wang
*/
public class LocalizationManager : MonoBehaviour
{

    public Dictionary<string, string> translationDict;

    public string getTranslation(string token) {

        string translatedToken = "ERROR";
        translationDict.TryGetValue(token, out translatedToken);
        return translatedToken;
    }

    public void Setup(string languageName) {

        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Languages");
        translationDict = new Dictionary<string, string>();

        foreach (FileInfo dict in dir.GetFiles(languageName + ".csv")) {

            Debug.Log("Found");

            string fileContents = System.IO.File.ReadAllText(dict.FullName);
            string[] translations = fileContents.Split("\n"[0]);

            for(int i = 0; i<translations.Length; i++)
            {
                string[] tokens = translations[i].Split(',');
                if (tokens != null && tokens.Length > 1) {
                    translationDict.Add(tokens[0], tokens[1]);
                }
            }
            
        }
    }

}
