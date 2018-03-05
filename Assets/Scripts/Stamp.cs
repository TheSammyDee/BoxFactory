
public class Stamp
{
    private float _rotation = 0;

    public float rotation
    {
        get
        {
            return _rotation;
        }
        set
        {
            _rotation += value;
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
}
