using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionFactory
{
    private const float STAMP_INCREMENT = 1f / 6f;

    private float stampChance;
    private int stampCount;
    private PuzzleSolver solver;

    private const int LEVEL_MULTIPLIER = 3;

    public SolutionFactory()
    {
        solver = new PuzzleSolver();
    }

    public Solution CreateSolution(int level)
    {
        ResetVariables();
        int maxMoves = LEVEL_MULTIPLIER * level;
        int minMoves = maxMoves - LEVEL_MULTIPLIER + 1;

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
                    stampChance = STAMP_INCREMENT;

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
                stampChance += STAMP_INCREMENT;
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
        stampChance = STAMP_INCREMENT;
        stampCount = 0;
    }
}
