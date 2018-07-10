using System.Collections.Generic;

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

    public void Stamp()
    {
        Stamp stamp = new Stamp();
        stamp.rotation = -front.rotation;
        front.stamps.Add(stamp);
    }
}
