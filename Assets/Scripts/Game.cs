using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public UnfoldedBox unfoldedSolution;
    public Text resultText;
    public Text commandList;
    public Button doneButton;
    public Button resetButton;

    private Box solutionBox;
    private Box box;

	// Use this for initialization
	void Start () {
        CreateSolution();
        unfoldedSolution.UnfoldBox(solutionBox);

        resetButton.gameObject.SetActive(false);

        box = new Box();	
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
            unfoldedSolution.Clear();
            unfoldedSolution.UnfoldBox(box);
        }

        doneButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        resultText.text = "";
        unfoldedSolution.Clear();
        unfoldedSolution.UnfoldBox(solutionBox);
        commandList.text = "";
        box = new Box();

        doneButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(false);
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
