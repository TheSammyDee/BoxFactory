using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public VisualBox unfoldedBoxView;
    public VisualBox foldedBoxView;

    public bool flatViewOn;
    public Text resultText;
    public Text commandList;
    public Button doneButton;
    public Button resetButton;
    public Button resultButton;
    public Button goalButton;
    public Text boxTypeButtonText;

    public BoxViewer boxViewer;

    private Box solutionBox;
    private Box playerBox;
    private Box showingBox;
    private VisualBox boxView;
    private List<VisualBox.Command> animationCommands;

	// Use this for initialization
	void Start ()
    {
        solutionBox = CreateSolution();
        showingBox = solutionBox;
        SetFlatBoxView(flatViewOn);
        animationCommands = new List<VisualBox.Command>();
        ResetGame();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Box CreateSolution()
    {
        Box box = new Box();
        box.RotateZLeft();
        box.Stamp();
        box.RotateYLeft();
        box.RotateYLeft();
        box.RotateZLeft();
        box.Stamp();

        return box;
    }

    public void ToggleBoxView()
    {
        flatViewOn = !flatViewOn;
        SetFlatBoxView(flatViewOn);
    }

    public void SetFlatBoxView(bool value)
    {
        boxView = value ? unfoldedBoxView : foldedBoxView;
        boxViewer.ShowBox(!value);
        unfoldedBoxView.gameObject.SetActive(value);
        boxView.ApplyBox(showingBox);
        boxTypeButtonText.text = value ? "3D" : "2D";
    }

    public void RotateYLeft()
    {
        commandList.text = commandList.text + "Left Y\n";
        animationCommands.Add(VisualBox.Command.Left90Y);
        playerBox.RotateYLeft();
    }

    public void RotateZLeft()
    {
        commandList.text = commandList.text + "Left Z\n";
        animationCommands.Add(VisualBox.Command.Left90Z);
        playerBox.RotateZLeft();
    }

    public void Stamp()
    {
        commandList.text = commandList.text + "Stamp\n";
        animationCommands.Add(VisualBox.Command.Stamp);
        playerBox.Stamp();
    }

    public void Done()
    {
        ActivateButtons(false);
        flatViewOn = false;
        SetFlatBoxView(flatViewOn);
        boxViewer.ResetView();
        foldedBoxView.AnimateBox(animationCommands, OnFinishDoneAnimation);
    }

    private void ActivateButtons(bool value)
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = value;
        }
    }

    private void OnFinishDoneAnimation()
    {
        ActivateButtons(true);

        bool result = CompareToSolution(playerBox);

        if (result)
        {
            resultText.text = "Correct";
        }
        else
        {
            resultText.text = "Nope"; 
        }

        boxViewer.ResetView();
        boxView.ApplyBox(playerBox);

        doneButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(true);
        resultButton.gameObject.SetActive(true);
        goalButton.gameObject.SetActive(true);
    }

    public void ShowResult()
    {
        showingBox = playerBox;
        boxView.ApplyBox(showingBox);
        resultButton.interactable = false;
        goalButton.interactable = true;
    }

    public void ShowGoal()
    {
        showingBox = solutionBox;
        boxView.ApplyBox(showingBox);
        goalButton.interactable = false;
        resultButton.interactable = true;
    }

    public void ResetGame()
    {
        resultText.text = "";
        showingBox = solutionBox;
        boxView.ApplyBox(showingBox);
        commandList.text = "";
        playerBox = new Box();
        boxViewer.ResetView();
        animationCommands.Clear();

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
