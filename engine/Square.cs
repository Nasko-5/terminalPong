namespace cetest.engine;
public unsafe class Square : IRenderable
{
    /// <summary>
    /// Constructor for the <c>Square</c> class
    /// </summary>
    /// <param name="id"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="filled"></param>
    /// <param name="outlinePattern"></param>
    /// <param name="fillChar"></param>
    public Square(string id, int x, int y, int width, int height, bool filled, string outlinePattern = "@", char fillChar = ' ')
    {
        ID = id;
        XPosition = x;
        YPosition = y;
        Width = width;
        Height = height;
        OutlinePattern = outlinePattern;
        FillChar = fillChar;
        Filled = filled;
        Show = true;
    }


    /// <summary>
    /// Draws the square on the specified Window
    /// </summary>
    /// <param name="Window"></param>
    public void Draw(IntPtr Window)
    {

        if (Filled)
        {
            for (int i = 0; i < Height; i++)
            {
                Drawing.HorizontalLine(Window, XPosition + 1, YPosition + i, Width - 1, FillChar);
            }
        }

        Drawing.VerticalLine(Window, XPosition, YPosition + 1, Height - 1, OutlinePattern);
        Drawing.VerticalLine(Window, XPosition + Width, YPosition, Height, OutlinePattern);
        Drawing.HorizontalLine(Window, XPosition, YPosition, Width, OutlinePattern);
        Drawing.HorizontalLine(Window, XPosition, YPosition + Height, Width, OutlinePattern);

    }

    //public bool CollidesWith(IRenderable obj)
    //{
    //    return          XPosition + Width >= obj.XPosition &&
    //            obj.XPosition + obj.Width >= XPosition     &&
    //                   YPosition + Height >= obj.YPosition &&
    //           obj.YPosition + obj.Height >= YPosition     ;
    //}

    //public bool CollidesWithRight(IRenderable obj)
    //{
    //    return CollidesWith(obj) & XPosition + Width <= obj.XPosition;
    //}

    //public bool CollidesWithLeft(IRenderable obj)
    //{
    //    return CollidesWith(obj) & obj.XPosition + obj.Width <= XPosition;
    //}

    //public bool CollidesWithTop(IRenderable obj)
    //{
    //    return CollidesWith(obj) & YPosition + Height <= obj.YPosition;
    //}

    //public bool CollidesWithBottom(IRenderable obj)
    //{
    //    return CollidesWith(obj) & obj.YPosition + obj.Height <= YPosition;
    //}

    /// <summary>
    /// Toggles visibility
    /// </summary>
    public void ShowToggle()
    {
        Show ^= true;
    }

    /// <summary>
    /// Identifier and name for this square
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// Determines whether or not to display this square
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// The width of the square
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of the square
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// The outline pattern to surround the square with
    /// </summary>
    public string OutlinePattern { get; set; }
    /// <summary>
    /// Character to fill the square with
    /// </summary>
    public char FillChar { get; set; }

    /// <summary>
    /// Dictates whether or not to fill the square
    /// </summary>
    public bool Filled { get; set; }

    /// <summary>
    /// The X position of this square
    /// </summary>
    public int XPosition { get; set; }

    /// <summary>
    /// The Y position of this square
    /// </summary>
    public int YPosition { get; set; }
}

