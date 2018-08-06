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
        Box backupBox = new Box();
        List<Box.Command> backupCommands = new List<Box.Command>();

        while (solutionCommands.Count < minMoves)
        {
            float rand = Random.value;
            if (rand < stampChance)
            {
                if (box.Stamp())
                {
                    stampChance = Config.Instance.SolutionCreationStampIncrement;

                    solutionCommands = solver.Solve(box);

                    // If the number of commands to make the Box exceeds the desired level,
                    // revert back to the state of the previous stamp and try again, to save
                    // from having to recreate and solve all the levels up to this point
                    if (solutionCommands.Count > maxMoves)
                    {
                        box = new Box(backupBox);
                        solutionCommands = backupCommands;
                    }
                    else
                    {
                        stampCount++;
                        backupBox = new Box(box);
                        backupCommands = solutionCommands;
                    }
                }
                else if (stampChance >= 1)
                {
                    stampChance = Config.Instance.SolutionCreationStampIncrement;
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
