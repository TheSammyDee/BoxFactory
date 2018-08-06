
public class Stamp
{
    private int _rotation = 0;

    public int rotation
    {
        get
        {
            return _rotation;
        }
        set
        {
            _rotation = value;
            while (_rotation >= 360)
            {
                _rotation -= 360;
            }
            while (_rotation < 0)
            {
                _rotation += 360;
            }
        }
    }

    public Stamp()
    {
    }

    public Stamp DeepClone()
    {
        Stamp stamp = new Stamp();
        stamp.rotation = rotation;

        return stamp;
    }
}
