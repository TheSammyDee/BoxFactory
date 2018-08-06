using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver
{
    private Dictionary<string, Dictionary<string, SolutionNode>> solutionTree;
    private List<Box.Command> shortestCommandChain;
    private int shortestCount;

    // Use dictionaries because of the saved cost of lookup compared to ToString()
    private Dictionary<Face.Side, string> sideNames = new Dictionary<Face.Side, string>
    {
        { Face.Side.Front, "Front" },
        { Face.Side.Back, "Back" },
        { Face.Side.Left, "Left" },
        { Face.Side.Right, "Right" },
        { Face.Side.Top, "Top" },
        { Face.Side.Bottom, "Botton" }
    };

    private Dictionary<int, string> rotationNames = new Dictionary<int, string>
    {
        { 0, "0" },
        { 90, "90" },
        { 180, "180" },
        { 270, "270" }
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
        List<StampPosition> stampPositions = GetStampPositions(box);
        StampPosition startingPosition = new StampPosition(Face.Side.Front, 0);
        string startingKey = GenerateStampPositionKey(startingPosition);
        shortestCommandChain = new List<Box.Command>();
        shortestCount = 99999999;
        FindShortestCommandChain(startingKey, stampPositions, new List<Box.Command>());

        return shortestCommandChain;
    }

    /// <summary>
    /// Calculates the shortest list of <see cref="Box.Command"/>s needed to reach each <see cref="Face"/>
    /// of a <see cref="Box"/> in each orientation, from each Face in each orientation
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, Dictionary<string, SolutionNode>> BuildSolutionTree()
    {
        Dictionary<string, Dictionary<string, SolutionNode>> tree = new Dictionary<string, Dictionary<string, SolutionNode>>();
        Box firstBox = new Box();
        Dictionary<string, SolutionNode> firstBranch = BuildSolutionBranch(firstBox);
        
        foreach (KeyValuePair<string, SolutionNode> solutionPair in firstBranch)
        {
            string startingKey = solutionPair.Key;

            if (startingKey == GenerateBoxKey(firstBox))
            {
                tree.Add(startingKey, firstBranch);
            }
            else
            {
                Box nextBox = solutionPair.Value.box;
                Dictionary<string, SolutionNode> nextBranch = BuildSolutionBranch(nextBox);
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
    private Dictionary<string, SolutionNode> BuildSolutionBranch(Box box)
    {
        // Create the first node for 0 moves, the starting position
        Dictionary<string, SolutionNode> solutionBranch = new Dictionary<string, SolutionNode>();
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
            string yKey = GenerateBoxKey(yBox);
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
            string zKey = GenerateBoxKey(zBox);
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

    private List<StampPosition> GetStampPositions(Box box)
    {
        List<StampPosition> positions = new List<StampPosition>();

        foreach (Face face in box.faces)
        {
            foreach (Stamp stamp in face.stamps)
            {
                StampPosition sp = new StampPosition(face.side, -stamp.rotation);
                positions.Add(sp);
            }
        }

        return positions;
    }

    private void FindShortestCommandChain(string startingKey, List<StampPosition> stampPositions, List<Box.Command> commands)
    {
        for (int i = 0; i < stampPositions.Count; i++)
        {
            string stampKey = GenerateStampPositionKey(stampPositions[i]);
            
            List<Box.Command> commandsToAdd = solutionTree[startingKey][stampKey].commands;
            int newCount = commands.Count + commandsToAdd.Count + 1;

            // If this path is already longer than the current shortest path, abandon this path
            if (newCount >= shortestCount)
            {
                return;
            }

            List<Box.Command> newCommands = CopyCommandList(commands);
            newCommands.AddRange(commandsToAdd);
            newCommands.Add(Box.Command.Stamp);
            
            // If we have reached the end of this path without abandoning it, it is the new current shortest path
            if (stampPositions.Count == 1)
            {
                shortestCommandChain = newCommands;
                shortestCount = newCount;

                return;
            }
            else
            {
                List<StampPosition> newPositions = CopyStampPositionsList(stampPositions);
                newPositions.Remove(newPositions[i]);

                FindShortestCommandChain(stampKey, newPositions, newCommands);
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

    protected static List<StampPosition> CopyStampPositionsList(List<StampPosition> list)
    {
        List<StampPosition> newList = new List<StampPosition>();

        foreach (StampPosition sp in list)
        {
            newList.Add(new StampPosition(sp));
        }

        return newList;
    }

    private string GenerateBoxKey(Box box)
    {
        return sideNames[box.Front().side] + "_" + rotationNames[box.Front().rotation];
    }

    private string GenerateStampPositionKey(StampPosition sp)
    {
        return sideNames[sp.side] + "_" + rotationNames[sp.orientation];
    }
}
