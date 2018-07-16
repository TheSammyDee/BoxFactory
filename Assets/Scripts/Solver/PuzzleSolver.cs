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

    protected static List<Box.Command> CopyCommandList(List<Box.Command> list)
    {
        List<Box.Command> newList = new List<Box.Command>();

        foreach (Box.Command command in list)
        {
            newList.Add(command);
        }

        return newList;
    }

    private string GenerateBoxKey(Box box)
    {
        return box.Front().side.ToString() + "_" + box.Front().rotation.ToString();
    }
}
