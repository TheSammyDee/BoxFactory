using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[TestFixture]
public class PuzzleSolverSpeedTests
{
	[Test]
    // Runs the solver 7,725,065 times
    public void SolveForLargeBoxCreationSimulation()
    {
        Box box = new Box();
        PuzzleSolver solver = new PuzzleSolver();
        Stopwatch stopwatch = Stopwatch.StartNew();

        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.RotateYLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateYLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateZLeft();
        box.Stamp();
        solver.Solve(box);

        box.RotateYLeft();
        box.RotateYLeft();
        box.Stamp();
        solver.Solve(box);

        stopwatch.Stop();

        UnityEngine.Debug.Log(solver.solves + " solves in " + (stopwatch.ElapsedMilliseconds / 1000f) + " seconds");
    }
}
