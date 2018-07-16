using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    public Box box { get; private set; }
    public int score { get; private set; }

    public Solution(Box box, int score)
    {
        this.box = box;
        this.score = score;
    }
}
