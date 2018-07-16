using System.Collections.Generic;

public class Box
{
    public List<Face> faces;
    public enum Command { Left90Y, Left90Z, Stamp };

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

    public Box(Box box)
    {
        front = box.Front().DeepClone();
        back = box.Back().DeepClone();
        left = box.Left().DeepClone();
        right = box.Right().DeepClone();
        top = box.Top().DeepClone();
        bottom = box.Bottom().DeepClone();

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

    public void RotateYLeftInverse()
    {
        Face temp = left;
        left = back;
        back = right;
        right = front;
        front = temp;
        top.Rotate(90);
        bottom.Rotate(-90);
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

    public void RotateZLeftInverse()
    {
        back.Rotate(90);
        front.Rotate(-90);
        left.Rotate(-90);
        Face temp = left;
        bottom.Rotate(-90);
        left = bottom;
        right.Rotate(-90);
        bottom = right;
        top.Rotate(-90);
        right = top;
        top = temp;
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

    public Face Front()
    {
        return front;
    }

    public Face Back()
    {
        return back;
    }

    public Face Left()
    {
        return left;
    }

    public Face Right()
    {
        return right;
    }

    public Face Top()
    {
        return top;
    }

    public Face Bottom()
    {
        return bottom;
    }
}
