﻿using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum Language
    {
        English, Spanish, French
    }

    public Language language;
    public LocalizationManager dict;
    public Text scoreText;
    private string scoreString = "Scored: ";
    public Text livesText;
    private string livesString = "Life: ";
    public Text enemyText;
    private string enemyString = "Foes: ";

    private void Awake()
    {

        dict = this.GetComponent<LocalizationManager>();
        if (language == Language.English) {
            dict.Setup("English");
        }
        if (language == Language.French)
        {
            dict.Setup("French");
        }
        if (language == Language.Spanish)
        {
            dict.Setup("Spanish");
        }

        scoreString = dict.getTranslation("Score");
        livesString = dict.getTranslation("Life");
        enemyString = dict.getTranslation("Foes");
        
        Debug.Log(scoreString + " "+ livesString + " " + enemyString);

    }

    private void UpdateText(Text textObj, string words, bool value)
    {
        textObj.text = Concatenate(words, value);
    }

    private string Concatenate(string words, bool value)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(words);
        builder.Append(value);

        return builder.ToString();
    }

    private void UpdateText(Text textObj, string words, float value)
    {
        textObj.text = Concatenate(words, value);
    }

    private string Concatenate(string words, float value)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(words);
        builder.Append(value);

        return builder.ToString();
    }

    public void UpdateLives(int lives)
    {
        UpdateText(livesText, livesString, lives);
    }

    public void UpdateScore(int score)
    {
        UpdateText(scoreText, scoreString, score);
    }

    public void UpdateEnemies(int enemies)
    {
        UpdateText(enemyText, enemyString, enemies);
    }

    private void UpdateText(Text textObj, string words, int value)
    {
        textObj.text = Concatenate(words, value);
    }

    private string Concatenate(string words, int value)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(words);
        builder.Append(value);

        return builder.ToString();
    }
}


