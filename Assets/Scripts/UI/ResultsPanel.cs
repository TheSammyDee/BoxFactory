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
    Button resultButton;

    [SerializeField]
    Button goalButton;

    public void Show(bool solved, int perfectScore)
    {
        gameObject.SetActive(true);

        string result;

        if (solved)
        {
            result = "Correct";
        }
        else
        {
            result = "Nope";
        }

        resultText.text = result + "\nPerfect score: " + perfectScore;
        nextButton.gameObject.SetActive(solved);
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
