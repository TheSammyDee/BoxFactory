using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPanel : MonoBehaviour {

    [SerializeField]
    Button nextButton;

    [SerializeField]
    Text resultText;

    [SerializeField]
    Image longTextBG;

    [SerializeField]
    Image shortTextBG;

    [SerializeField]
    Button resultButton;

    [SerializeField]
    Button goalButton;

    public void Show(bool solved, int perfectScore, int playerScore)
    {
        gameObject.SetActive(true);

        string result;

        if (solved)
        {
            result = "Correct!" + "\nYour score: " + playerScore + "\nPerfect: " + perfectScore;
            longTextBG.gameObject.SetActive(true);
            shortTextBG.gameObject.SetActive(false);
        }
        else
        {
            result = "Not quite...";
            longTextBG.gameObject.SetActive(false);
            shortTextBG.gameObject.SetActive(true);
        }

        resultText.text = result;
    }

    public void ShowResult()
    {
        resultButton.interactable = false;
        goalButton.interactable = true;
    }

    public void ShowGoal()
    {
        goalButton.interactable = false;
        resultButton.interactable = true;
    }
}
