using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public VisualBox unfoldedSolution;
    public VisualBox foldedSolution;
    public bool flatBox;
    public Text resultText;
    public Text commandList;
    public Button doneButton;
    public Button resetButton;
    public Button resultButton;
    public Button goalButton;
    public BoxCamRotator boxCamRotator;

    private Box solutionBox;
    private Box box;
    private VisualBox visualSolution;

	// Use this for initialization
	void Start () {
        visualSolution = flatBox ? unfoldedSolution : foldedSolution;
        CreateSolution();
        ResetGame();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateSolution()
    {
        solutionBox = new Box();
        solutionBox.RotateZLeft();
        solutionBox.Stamp();
        solutionBox.RotateYLeft();
        solutionBox.RotateYLeft();
        solutionBox.RotateZLeft();
        solutionBox.Stamp();
    }

    public void RotateYLeft()
    {
        commandList.text = commandList.text + "Left Y\n";
        box.RotateYLeft();
    }

    public void RotateZLeft()
    {
        commandList.text = commandList.text + "Left Z\n";
        box.RotateZLeft();
    }

    public void Stamp()
    {
        commandList.text = commandList.text + "Stamp\n";
        box.Stamp();
    }

    public void Done()
    {
        bool result = CompareToSolution(box);

        if (result)
        {
            resultText.text = "Correct";
        }
        else
        {
            resultText.text = "Nope";
            visualSolution.ApplyBox(box);
        }

        doneButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(true);
        resultButton.gameObject.SetActive(true);
        goalButton.gameObject.SetActive(true);
    }

    public void ShowResult()
    {
        visualSolution.ApplyBox(box);
        resultButton.interactable = false;
        goalButton.interactable = true;
    }

    public void ShowGoal()
    {
        visualSolution.ApplyBox(solutionBox);
        goalButton.interactable = false;
        resultButton.interactable = true;
    }

    public void ResetGame()
    {
        resultText.text = "";
        visualSolution.ApplyBox(solutionBox);
        commandList.text = "";
        box = new Box();
        boxCamRotator.Reset();

        doneButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(false);
        resultButton.interactable = false;
        resultButton.gameObject.SetActive(false);
        goalButton.gameObject.SetActive(false);
        goalButton.interactable = true;
    }

    private bool CompareToSolution(Box box)
    {
        for (int i = 0; i < box.faces.Count; i++)
        {
            if (box.faces[i].stamps.Count != solutionBox.faces[i].stamps.Count)
            {
                return false;
            }
            for (int j = 0; j < box.faces[i].stamps.Count; j++)
            {
                if (box.faces[i].stamps[j].rotation != solutionBox.faces[i].stamps[j].rotation)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
