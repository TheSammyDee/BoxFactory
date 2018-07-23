using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    public Box box { get; private set; }
    public List<Box.Command> commands { get; private set; }
    public int score
    {
        get
        {
            return commands.Count;
        }
    }

    public Solution(Box box, List<Box.Command> commands)
    {
        this.box = box;
        this.commands = commands;
    }
}
