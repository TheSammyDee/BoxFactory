using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver
{
    public PuzzleSolver() { }

    public List<Box.Command> Solve(Box box)
    {
        return new List<Box.Command> { Box.Command.Stamp };
    }
}
