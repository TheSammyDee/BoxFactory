using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public VisualBox boxView2D;
    public VisualBox boxView3D;

    public bool viewIs2D;
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
    private BoxAnimator animator;
    private PuzzleFactory puzzleFactory;
    private bool solved;

	// Use this for initialization
	void Start ()
    {
        puzzleFactory = new PuzzleFactory();
        solved = false;
        solutionBox = puzzleFactory.CreatePuzzle();
        showingBox = solutionBox;
        Set2DBoxView(viewIs2D);
        animationCommands = new List<VisualBox.Command>();
        animator = new GameObject("Box Animator").AddComponent<BoxAnimator>();
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
        viewIs2D = !viewIs2D;
        Set2DBoxView(viewIs2D);
    }

    public void Set2DBoxView(bool value)
    {
        boxView = value ? boxView2D : boxView3D;
        boxViewer.ShowBox(!value);
        boxView2D.gameObject.SetActive(value);
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
        viewIs2D = false;
        Set2DBoxView(viewIs2D);
        boxViewer.ResetView();
        animator.AnimateBox(boxView.transform, boxView, animationCommands, OnFinishDoneAnimation);
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

        solved = CompareToSolution(playerBox);

        if (solved)
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
        if (solved)
        {
            solutionBox = puzzleFactory.CreatePuzzle();
            solved = false;
        }

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
                // TODO: Account for stamps in different order on same face
                if (box.faces[i].stamps[j].rotation != solutionBox.faces[i].stamps[j].rotation)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
