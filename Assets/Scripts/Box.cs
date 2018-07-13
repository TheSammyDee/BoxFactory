﻿using System.Collections.Generic;

public class Box
{
    public List<Face> faces;

    // The right face is on the player's right
    private Face front, back, top, bottom, left, right;

    public Box()
    {
        // The Face name indicates the position in relation to the player
        // The Face.Side is the face's original position and identifier
        front = new Face(Face.Side.Front);
        back = new Face(Face.Side.Back);
        back.Rotate(180);
        top = new Face(Face.Side.Top);
        bottom = new Face(Face.Side.Bottom);
        left = new Face(Face.Side.Left);
        right = new Face(Face.Side.Right);

        faces = new List<Face> { front, back, top, bottom, left, right };
    }

    // Rotates the Box counterclockwise facing the up direction 
    public void RotateYLeft()
    {
        Face temp = front;
        front = right;
        right = back;
        back = left;
        left = temp;
        top.Rotate(-90);
        bottom.Rotate(90); 
    }

    // Rotates the Box counterclockwise facing the forward direction
    public void RotateZLeft()
    {
        Face temp = top;
        top = right;
        top.Rotate(90);
        right = bottom;
        right.Rotate(90);
        bottom = left;
        bottom.Rotate(90);
        left = temp;
        left.Rotate(90);
        front.Rotate(90);
        back.Rotate(-90);
    }

    /// <summary>
    /// Adds a <see cref="Stamp"/> to the current front <see cref="Face"/> unless
    /// a Stamp has already been placed with the same position and orientation.
    /// Returns true if a new Stamp is placed, otherwise false;
    /// </summary>
    /// <returns></returns>
    public bool Stamp()
    {
        Stamp stamp = new Stamp();
        stamp.rotation = -front.rotation;
        if (front.stamps.Count > 0)
        {
            for (int i = 0; i < front.stamps.Count; i++)
            {
                if (front.stamps[i].rotation == stamp.rotation)
                {
                    return false;
                }
            }
        }
        front.stamps.Add(stamp);
        return true;
    }
}
