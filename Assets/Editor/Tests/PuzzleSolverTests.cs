using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestFixture]
public class PuzzleSolverTests {

    [Test]
    public void PuzzleSolverTest0_FrontStamp()
    {
        Box box = new Box();
        box.Stamp();

        PuzzleSolver solver = new PuzzleSolver();
        List<Box.Command> commands = solver.Solve(box);

        Assert.AreEqual(1, commands.Count);
        Assert.AreEqual(Box.Command.Stamp, commands[0]);
    }

    [Test]
    public void PuzzleSolverTest1_RotateOnce()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.Stamp();

        PuzzleSolver solver = new PuzzleSolver();
        List<Box.Command> commands = solver.Solve(box);

        List<Box.Command> expectedCommands = new List<Box.Command> {
            Box.Command.Left90Y,
            Box.Command.Stamp };

        Assert.True(ListsAreEqual(expectedCommands, commands));
    }

    [Test]
    public void PuzzleSolverTest2_RotateTwice()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.RotateZLeft();
        box.Stamp();

        PuzzleSolver solver = new PuzzleSolver();
        List<Box.Command> commands = solver.Solve(box);

        List<Box.Command> expectedCommands = new List<Box.Command> {
            Box.Command.Left90Y,
            Box.Command.Left90Z,
            Box.Command.Stamp };

        Assert.True(ListsAreEqual(expectedCommands, commands));
    }

    [Test]
    public void PuzzleSolverTest3_TwoStamps()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.RotateZLeft();
        box.Stamp();
        box.RotateYLeft();
        box.RotateYLeft();
        box.Stamp();

        PuzzleSolver solver = new PuzzleSolver();
        List<Box.Command> commands = solver.Solve(box);

        List<Box.Command> expectedCommands = new List<Box.Command> {
            Box.Command.Left90Y,
            Box.Command.Left90Z,
            Box.Command.Stamp,
            Box.Command.Left90Y,
            Box.Command.Left90Y,
            Box.Command.Stamp };

        Assert.True(ListsAreEqual(expectedCommands, commands));
    }

    [Test]
    public void PuzzleSolverTest4_TwoStampsSameFace()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.Stamp();
        box.RotateZLeft();
        box.Stamp();

        PuzzleSolver solver = new PuzzleSolver();
        List<Box.Command> commands = solver.Solve(box);

        List<Box.Command> expectedCommands = new List<Box.Command> {
            Box.Command.Left90Y,
            Box.Command.Stamp,
            Box.Command.Left90Z,
            Box.Command.Stamp };

        Assert.True(ListsAreEqual(expectedCommands, commands));
    }

    [Test]
    public void PuzzleSolverTest5_ThreeStamps()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.RotateZLeft();
        box.Stamp();
        box.RotateZLeft();
        box.RotateYLeft();
        box.Stamp();
        box.RotateYLeft();
        box.RotateYLeft();
        box.Stamp();

        PuzzleSolver solver = new PuzzleSolver();
        List<Box.Command> commands = solver.Solve(box);

        List<Box.Command> expectedCommands = new List<Box.Command> {
            Box.Command.Left90Y,
            Box.Command.Left90Z,
            Box.Command.Stamp,
            Box.Command.Left90Z,
            Box.Command.Left90Y,
            Box.Command.Stamp,
            Box.Command.Left90Y,
            Box.Command.Left90Y,
            Box.Command.Stamp };

        Assert.True(ListsAreEqual(expectedCommands, commands));
    }

    private bool ListsAreEqual(List<Box.Command> list1, List<Box.Command> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
            {
                return false;
            }
        }

        return true;
    }
}
