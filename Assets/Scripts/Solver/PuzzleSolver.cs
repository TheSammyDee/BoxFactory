using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver
{
    private Dictionary<string, Dictionary<string, SolutionNode>> solutionTree;
    private List<SolutionNode> nodesToCheck;

    private class SolutionNode
    {
        public List<Box.Command> inversedCommands;
        public List<Box.Command> commandChain
        {
            get
            {
                List<Box.Command> newList = CopyCommandList(inversedCommands);
                newList.Reverse();
                return newList;
            }
        }
        public Box box;

        public SolutionNode(List<Box.Command> commands, Box box)
        {
            inversedCommands = commands;
            this.box = box;
        }
    }

    protected class StampPosition
    {
        public Face.Side side;
        private float _orientation;
        public float orientation
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
        public bool done;

        public StampPosition(Face.Side side, float orientation)
        {
            this.side = side;
            this.orientation = orientation;
            done = false;
        }

        public StampPosition(StampPosition sp)
        {
            this.side = sp.side;
            this.orientation = sp.orientation;
            this.done = sp.done;
        }
    }

    public PuzzleSolver()
    {
        nodesToCheck = new List<SolutionNode>();
        solutionTree = new Dictionary<string, Dictionary<string, SolutionNode>>();
    }

    public List<Box.Command> Solve(Box box)
    {
        if (solutionTree.Count == 0)
        {
            solutionTree = BuildSolutionTree();
        }

        List<StampPosition> stampPositions = GetStampPositions(box);
        List<Box.Command> startingCommand = new List<Box.Command> { Box.Command.Stamp };
        List<List<Box.Command>> commandChains = GenerateCommandChains(box, stampPositions, startingCommand);

        return new List<Box.Command> { Box.Command.Stamp };
    }

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

    private Dictionary<string, SolutionNode> BuildSolutionBranch(Box box)
    {
        Dictionary<string, SolutionNode> solutionBranch = new Dictionary<string, SolutionNode>();
        SolutionNode firstNode = new SolutionNode(new List<Box.Command>(), box);
        nodesToCheck.Add(firstNode);
        solutionBranch.Add(GenerateBoxKey(firstNode.box), firstNode);
        for (int i = 0; i < solutionBranch.Count; i++)
        {
            SolutionNode currentNode = nodesToCheck[i];

            Box yBox = new Box(currentNode.box);
            yBox.RotateYLeftInverse();
            string yKey = GenerateBoxKey(yBox);
            if (!solutionBranch.ContainsKey(yKey))
            {
                List<Box.Command> yCommands = CopyCommandList(currentNode.inversedCommands);
                yCommands.Add(Box.Command.Left90Y);
                SolutionNode yNode = new SolutionNode(yCommands, yBox);
                nodesToCheck.Add(yNode);
                solutionBranch.Add(yKey, yNode);
            }

            Box zBox = new Box(currentNode.box);
            zBox.RotateZLeftInverse();
            string zKey = GenerateBoxKey(zBox);
            if (!solutionBranch.ContainsKey(zKey))
            {
                List<Box.Command> zCommands = CopyCommandList(currentNode.inversedCommands);
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

    private List<List<Box.Command>> GenerateCommandChains(StampPosition currentPosition, List<StampPosition> stampPositions, List<Box.Command> commands)
    {
        string startingKey = GenerateBoxKey(box);
        List<List<Box.Command>> commandChains;

        for (int i = 0; i < stampPositions.Count; i++)
        {
            string stampKey = GenerateStampPositionKey(stampPositions[i]);
            List<Box.Command> newCommands = CopyCommandList(commands);
            List<Box.Command> commandsToAdd = solutionTree[startingKey][stampKey].inversedCommands;
            newCommands.AddRange(commandsToAdd);
            newCommands.Add(Box.Command.Stamp);
            List<StampPosition> newPositions = CopyStampPositionsList(stampPositions);
            newPositions.Remove(newPositions[i]);

            Box newBox = new Box(box);

            foreach (Box.Command command in commandsToAdd)
            {
                switch (command)
                {
                    case Box.Command.Left90Y:
                        newBox.RotateYLeftInverse();
                }
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
        return box.Front().side.ToString() + "_" + box.Front().rotation.ToString();
    }

    private string GenerateStampPositionKey(StampPosition sp)
    {
        return sp.side.ToString() + "_" + sp.orientation.ToString();
    }
}
