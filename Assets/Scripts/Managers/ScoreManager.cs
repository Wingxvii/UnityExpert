using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Nightmare
{
    public class ScoreManager : MonoBehaviour
    {
        public static int score;
        private int levelThreshhold = 1000;

        Text sText;

        void Awake ()
        {
            sText = GetComponent <Text> ();

            score = 0;
        }


        void Update ()
        {
            sText.text = "Score: " + score;
            if (score >= levelThreshhold)
            {
                AdvanceLevel();
            }
        }

        private void AdvanceLevel()
        {
            levelThreshhold = score + 1000;
            LevelManager lm = FindObjectOfType<LevelManager>();
            lm.AdvanceLevel();
        }
    }
}