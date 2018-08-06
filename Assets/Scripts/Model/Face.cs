using System.Collections.Generic;

public class Face
{
    public int rotation { get; private set; }

    public enum Side { Front, Back, Top, Bottom, Left, Right };

    public Side side { get; private set; }
    public List<Stamp> stamps;

    public Face(Side side)
    {
        this.side = side;
        rotation = 0;
        stamps = new List<Stamp>();
    }

    public void Rotate(int rot)
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

    public Face DeepClone()
    {
        Face face = new Face(side);
        face.Rotate(rotation);
        foreach (Stamp stamp in stamps)
        {
            face.stamps.Add(stamp.DeepClone());
        }

        return face;
    }
}
