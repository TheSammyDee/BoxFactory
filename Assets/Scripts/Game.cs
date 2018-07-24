using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public VisualBox boxView2D;
    public VisualBox boxView3D;
    public VisualBox invisiBoxPrefab;

    public bool viewIs2D;
    public Text commandList;
    public Text boxTypeButtonText;
    public GameObject puzzlePanel;
    public ResultsPanel resultsPanel;
    public Button boxTypeButton;

    public BoxViewer boxViewer;

    private Solution solution;
    private Box playerBox;
    private Box showingBox;
    private VisualBox boxView;
    private List<Box.Command> animationCommands;
    private BoxAnimator animator;
    private SolutionFactory solutionFactory;

    private bool solved = false;
    private bool viewingResults = false;
    private bool showingGoal = false;

	// Use this for initialization
	void Start ()
    {
        solutionFactory = new SolutionFactory();
        solution = solutionFactory.CreateSolution();
        showingBox = solution.box;
        Set2DBoxView(viewIs2D);
        animationCommands = new List<Box.Command>();
        animator = new GameObject("Box Animator").AddComponent<BoxAnimator>();
        animator.Initialize(invisiBoxPrefab);
        ResetGame();
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
        bool isTemplate = !viewingResults || showingGoal;
        boxView.ApplyBox(showingBox, isTemplate);
        boxTypeButtonText.text = value ? "3D" : "2D";
    }

    public void RotateYLeft()
    {
        commandList.text = commandList.text + "Left Y\n";
        animationCommands.Add(Box.Command.Left90Y);
        playerBox.RotateYLeft();
    }

    public void RotateZLeft()
    {
        commandList.text = commandList.text + "Left Z\n";
        animationCommands.Add(Box.Command.Left90Z);
        playerBox.RotateZLeft();
    }

    public void Stamp()
    {
        commandList.text = commandList.text + "Stamp\n";
        animationCommands.Add(Box.Command.Stamp);
        playerBox.Stamp();
    }

    public void Done()
    {
        ActivateButtons(false);
        puzzlePanel.SetActive(false);
        viewIs2D = false;
        Set2DBoxView(viewIs2D);
        boxViewer.ResetView();
        animator.AnimateBox(boxView.transform, animationCommands, OnFinishDoneAnimation);
    }

    private void ActivateButtons(bool value)
    {
        boxTypeButton.interactable = value;
    }

    private void OnFinishDoneAnimation()
    {
        ActivateButtons(true);

        solved = playerBox.Compare(solution.box);
        viewingResults = true;

        resultsPanel.Show(solved, solution.score);

        boxViewer.ResetView();
        ShowResult();
    }

    public void ShowResult()
    {
        showingBox = playerBox;
        boxView.ApplyBox(showingBox);
        resultsPanel.ShowResult();
        showingGoal = false;
    }

    public void ShowGoal()
    {
        showingBox = solution.box;
        boxView.ApplyBox(showingBox, true);
        resultsPanel.ShowGoal();
        showingGoal = true;
    }

    public void NextPuzzle()
    {
        solution = solutionFactory.CreateSolution();
        ResetGame();
    }

    public void ResetGame()
    {
        resultsPanel.gameObject.SetActive(false);
        puzzlePanel.SetActive(true);

        solved = false;
        viewingResults = false;
        showingGoal = false;

        showingBox = solution.box;
        boxView.ApplyBox(showingBox, true);
        commandList.text = "";
        playerBox = new Box();
        boxViewer.ResetView();
        animationCommands.Clear();
    }
}
