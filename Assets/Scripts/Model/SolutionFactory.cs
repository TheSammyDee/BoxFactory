using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionFactory
{
    private const float STAMP_INCREMENT = 1f / 6f;

    private float stampChance;
    private int stampCount;
    private PuzzleSolver solver;

    public SolutionFactory()
    {
        solver = new PuzzleSolver();
    }

    public Solution CreateSolution()
    {
        ResetVariables();

        Box box = new Box();
        
        while (stampCount < 2)
        {
            float rand = Random.value;
            if (rand < stampChance)
            {
                if (box.Stamp())
                {
                    stampCount++;
                    stampChance = STAMP_INCREMENT;
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

        List<Box.Command> solutionCommands = solver.Solve(box);

        Solution solution = new Solution(box, solutionCommands);

        return solution;
    }

    private void ResetVariables()
    {
        stampChance = STAMP_INCREMENT;
        stampCount = 0;
    }
}
