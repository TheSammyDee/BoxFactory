using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFactory
{
    private const float STAMP_INCREMENT = 1f / 6f;

    private float stampChance;
    private int stampCount;

    public PuzzleFactory() { }

    public Box CreatePuzzle()
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

        return box;
    }

    private void ResetVariables()
    {
        stampChance = STAMP_INCREMENT;
        stampCount = 0;
    }
}
