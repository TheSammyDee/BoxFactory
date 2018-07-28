using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionFactory
{
    private float stampChance;
    private int stampCount;
    private PuzzleSolver solver;

    public SolutionFactory()
    {
        solver = new PuzzleSolver();
    }

    public Solution CreateSolution(int level)
    {
        ResetVariables();
        int maxMoves = Config.Instance.SolutionCreationLevelMultiplier * level;
        int minMoves = maxMoves - Config.Instance.SolutionCreationLevelMultiplier + 1;

        Box box = new Box();
        List<Box.Command> solutionCommands = new List<Box.Command>();


        while (solutionCommands.Count < minMoves)
        {
            float rand = Random.value;
            if (rand < stampChance)
            {
                if (box.Stamp())
                {
                    stampCount++;
                    stampChance = Config.Instance.SolutionCreationStampIncrement;

                    solutionCommands = solver.Solve(box);
                    if (solutionCommands.Count > maxMoves)
                    {
                        ResetVariables();
                        box = new Box();
                        solutionCommands.Clear();
                    }
                }
            }
            else
            {
                stampChance += Config.Instance.SolutionCreationStampIncrement;
            }
            rand = Random.value;
            if (rand > 0.5)
            {
                box.RotateYLeft();
            }
            else
            {
                box.RotateZLeft();
            }
        }

        Solution solution = new Solution(box, solutionCommands);

        return solution;
    }

    private void ResetVariables()
    {
        stampChance = Config.Instance.SolutionCreationStampIncrement;
        stampCount = 0;
    }
}
