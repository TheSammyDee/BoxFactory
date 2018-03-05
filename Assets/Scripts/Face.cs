﻿using System.Collections.Generic;

public class Face
{
    public float rotation { get; private set; }

    public enum Side { Front, Back, Top, Bottom, Left, Right };

    public Side side { get; private set; }
    public List<Stamp> stamps;

    public Face(Side side)
    {
        this.side = side;
        rotation = 0;
        stamps = new List<Stamp>();
    }

    public void Rotate(float rot)
    {
        rotation += rot;
        while (rotation >= 360)
        {
            rotation -= 360;
        }
        while (rotation < 0)
        {
            rotation += 360;
        }
    }
}
