using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver
{
    private Dictionary<int, Dictionary<int, SolutionNode>> solutionTree;
    private Dictionary<int, int> commandsCounts;
    private List<int> shortestPath;
    private int shortestCount;

    // Use dictionaries because of the saved cost of lookup compared to ToString()
    private Dictionary<Face.Side, int> sideNames = new Dictionary<Face.Side, int>
    {
        { Face.Side.Front, 10 },
        { Face.Side.Back, 20 },
        { Face.Side.Left, 30 },
        { Face.Side.Right, 40 },
        { Face.Side.Top, 50 },
        { Face.Side.Bottom, 60 }
    };

    private Dictionary<int, int> rotationNames = new Dictionary<int, int>
    {
        { 0, 1 },
        { 90, 2 },
        { 180, 3 },
        { 270, 4 }
    };

    /// <summary>
    /// Represents a solution end point of the given <see cref="Box"/> and the <see cref="Box.Command"/>s 
    /// required to reach that Box configuration
    /// </summary>
    private class SolutionNode
    {
        public List<Box.Command> commands;
        public Box box;

        public SolutionNode(List<Box.Command> commands, Box box)
        {
            this.commands = commands;
            this.box = box;
        }
    }

    /// <summary>
    /// Represents the positioning of a <see cref="Stamp"/> by the <see cref="Face.Side"/>
    /// it is on, and the rotation of the <see cref="Face"/> needs to be in to apply the Stamp
    /// </summary>
    protected class StampPosition
    {
        public Face.Side side;
        private int _orientation;
        public int orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
                while (_orientation >= 360)
                {
                    _orientation -= 360;
                }
                while (_orientation < 0)
                {
                    _orientation += 360;
                }
            }
        }

        public StampPosition(Face.Side side, int orientation)
        {
            this.side = side;
            this.orientation = orientation;
        }

        public StampPosition(StampPosition sp)
        {
            this.side = sp.side;
            this.orientation = sp.orientation;
        }
    }

    public PuzzleSolver()
    {
        solutionTree = BuildSolutionTree();
    }

    /// <summary>
    /// Finds the shortest possible list of <see cref="Box.Command"/>s to recreate
    /// the given <see cref="Box"/>
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    public List<Box.Command> Solve(Box box)
    {
        List<int> stampPositions = GetStampPositions(box);
        int startingKey = GenerateStampPositionKey(Face.Side.Front, 0);
        shortestPath = new List<int>();
        shortestCount = 99999999;
        commandsCounts = new Dictionary<int, int>();
        FindShortestPath(startingKey, stampPositions, new List<int> { { startingKey } }, 0);

        List<Box.Command> commandList = new List<Box.Command>();
        for (int i = 0; i <= shortestPath.Count - 2; i++)
        {
            int fromKey = shortestPath[i];
            int toKey = shortestPath[i + 1];

            commandList.AddRange(solutionTree[fromKey][toKey].commands);
            commandList.Add(Box.Command.Stamp);
        }

        return commandList;
    }

    /// <summary>
    /// Calculates the shortest list of <see cref="Box.Command"/>s needed to reach each <see cref="Face"/>
    /// of a <see cref="Box"/> in each orientation, from each Face in each orientation
    /// </summary>
    /// <returns></returns>
    private Dictionary<int, Dictionary<int, SolutionNode>> BuildSolutionTree()
    {
        Dictionary<int, Dictionary<int, SolutionNode>> tree = new Dictionary<int, Dictionary<int, SolutionNode>>();
        Box firstBox = new Box();
        Dictionary<int, SolutionNode> firstBranch = BuildSolutionBranch(firstBox);
        
        foreach (KeyValuePair<int, SolutionNode> solutionPair in firstBranch)
        {
            int startingKey = solutionPair.Key;

            if (startingKey == GenerateBoxKey(firstBox))
            {
                tree.Add(startingKey, firstBranch);
            }
            else
            {
                Box nextBox = solutionPair.Value.box;
                Dictionary<int, SolutionNode> nextBranch = BuildSolutionBranch(nextBox);
                tree.Add(startingKey, nextBranch);
            }
        }

        return tree;
    }

    /// <summary>
    /// Calculates the shortest list of <see cref="Box.Command"/>s needed to reach each <see cref="Face"/>
    /// of a <see cref="Box"/> in each orientation starting from the configuration of the given Box
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    private Dictionary<int, SolutionNode> BuildSolutionBranch(Box box)
    {
        // Create the first node for 0 moves, the starting position
        Dictionary<int, SolutionNode> solutionBranch = new Dictionary<int, SolutionNode>();
        SolutionNode firstNode = new SolutionNode(new List<Box.Command>(), box);
        List<SolutionNode> nodesToCheck = new List<SolutionNode>();
        nodesToCheck.Add(firstNode);
        solutionBranch.Add(GenerateBoxKey(firstNode.box), firstNode);

        // As each Box configuration is first reached, it is added to the solutionBranch.
        // Use each reached configuration as a new starting point in turn.
        // Once all configurations have been reached once, the branch is complete
        for (int i = 0; i < solutionBranch.Count; i++)
        {
            SolutionNode currentNode = nodesToCheck[i];

            Box yBox = new Box(currentNode.box);
            yBox.RotateYLeft();
            int yKey = GenerateBoxKey(yBox);
            if (!solutionBranch.ContainsKey(yKey))
            {
                List<Box.Command> yCommands = CopyCommandList(currentNode.commands);
                yCommands.Add(Box.Command.Left90Y);
                SolutionNode yNode = new SolutionNode(yCommands, yBox);
                nodesToCheck.Add(yNode);
                solutionBranch.Add(yKey, yNode);
            }

            Box zBox = new Box(currentNode.box);
            zBox.RotateZLeft();
            int zKey = GenerateBoxKey(zBox);
            if (!solutionBranch.ContainsKey(zKey))
            {
                List<Box.Command> zCommands = CopyCommandList(currentNode.commands);
                zCommands.Add(Box.Command.Left90Z);
                SolutionNode zNode = new SolutionNode(zCommands, zBox);
                nodesToCheck.Add(zNode);
                solutionBranch.Add(zKey, zNode);
            }
        }

        return solutionBranch;
    }

    private List<int> GetStampPositions(Box box)
    {
        List<int> positions = new List<int>();

        foreach (Face face in box.faces)
        {
            foreach (Stamp stamp in face.stamps)
            {
                int orientation = -stamp.rotation;
                while (orientation >= 360)
                {
                    orientation -= 360;
                }
                while (orientation < 0)
                {
                    orientation += 360;
                }
                int sp = GenerateStampPositionKey(face.side, orientation);
                positions.Add(sp);
            }
        }

        return positions;
    }

    // temp for testing
    public int solves = 0;

    List<int> newPath;
    List<int> newRemaining;

    private void FindShortestPath(int startingKey, List<int> remainingStampPositions, List<int> stampPath, int currentCount)
    {
        for (int i = 0; i < remainingStampPositions.Count; i++)
        {
            solves++;
            int stampKey = remainingStampPositions[i];
            int combinedKey = GenerateCombinedKey(startingKey, stampKey);
            int countToAdd;

            if (commandsCounts.ContainsKey(combinedKey))
            {
                countToAdd = commandsCounts[combinedKey];
            }
            else
            {
                countToAdd = solutionTree[startingKey][stampKey].commands.Count;
                commandsCounts.Add(combinedKey, countToAdd);
            }

            // The count of the commands from the previous step, the commands to get to this 
            // stamp, plus the stamp command
            int newCount = currentCount + countToAdd + 1;

            // If this path is already longer than the current shortest path, abandon this path
            if (newCount >= shortestCount)
            {
                return;
            }

            newPath = new List<int>(stampPath);
            newPath.Add(stampKey);
            
            // If we have reached the end of this path without abandoning it, it is the new current shortest path
            if (remainingStampPositions.Count == 1)
            {
                shortestPath = newPath;
                shortestCount = newCount;

                return;
            }
            else
            {
                newRemaining = new List<int>(remainingStampPositions);
                newRemaining.Remove(newRemaining[i]);

                FindShortestPath(stampKey, newRemaining, newPath, newCount);
            }
        }
    }

    protected static List<Box.Command> CopyCommandList(List<Box.Command> list)
    {
        List<Box.Command> newList = new List<Box.Command>();

        foreach (Box.Command command in list)
        {
            newList.Add(command);
        }

        return newList;
    }

    private int GenerateBoxKey(Box box)
    {
        return sideNames[box.Front().side] + rotationNames[box.Front().rotation];
    }

    private int GenerateStampPositionKey(Face.Side side, int orientation)
    {
        return sideNames[side] + rotationNames[orientation];
    }

    private int GenerateCombinedKey(int startingKey, int stampKey)
    {
        return startingKey * 100 + stampKey;
    }
}
