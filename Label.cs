
using Mindmagma.Curses;

namespace curses0;


/// <summary>
/// Class <c>Label</c> creates a render able Label object
/// </summary>

public unsafe class Label : IRenderable
{
    /// <summary>
    /// Constructor for the <c>Label</c> class
    /// </summary>
    /// <param name="iD"></param>
    /// <param name="text"></param>
    /// <param name="xPosition"></param>
    /// <param name="yPosition"></param>
    public Label(string id, int xPosition, int yPosition, int width, int height, string text, byte alignment = 0)
    {
        Text = text;
        Alignment = alignment;
        ID = id;
        XPosition = xPosition;
        YPosition = yPosition;
        Width = width;
        Height = height;
        Show = true;
    }



    public void Draw(IntPtr Window)
    {
        if (Text != previousText)
        {
            Parse();
        }

        switch (Alignment)
        {
            case 0:
                writeLeftAligned(Window, XPosition, YPosition, parsed);
                break;
            case 1:
                writeCenterAligned(Window, XPosition, YPosition, parsed);
                break;
            case 2:
                writeRightAligned(Window, XPosition, YPosition, parsed);
                break;
        }

        previousText = Text;
    }

    public void writeLeftAligned(IntPtr Window, int x, int y, string[] text)
    {
        for (int index = 0; index < Height - (Height - parsed.Length); index++)
        {
            NCurses.MoveWindowAddString(Window, YPosition + index, XPosition, text[index]);
        }
    }

    public void writeCenterAligned(IntPtr Window, int x, int y, string[] text)
    {
        for (int index = 0; index < Height - (Height - parsed.Length); index++)
        {
            NCurses.MoveWindowAddString(Window, YPosition + index, XPosition + (Width - text[index].Length) / 2, text[index]);
        }
    }

    public void writeRightAligned(IntPtr Window, int x, int y, string[] text)
    {
        for (int index = 0; index < Height - (Height - parsed.Length); index++)
        {
            NCurses.MoveWindowAddString(Window, YPosition + index, XPosition + (Width - text[index].Length), text[index]);
        }
    }

    private void Parse()
    {
        string[] substrings = Text.Split('\n');
        List<string> outList = new List<string>();
        for (int i = 0; i < substrings.Length; i++)
        {
            string line = substrings[i];
            for (int j = 0; j < line.Length; j += Width)
            {
                /* if i am reading this correctly, the splitting works like so;
                // 
                // lets take an example string "abcdefghijklmnopq"
                // and split it every 4 characters
                //
                // let the length of the string be L, 18, and the split S, and J, the starting index
                //
                // Iteration one:
                //      We have our starting position already, simple enough, but wee have to calculate
                //      how many chars from that position to read
                //
                //      We do that by simply comparing S with L minus J, because the string is technically
                //      getting "smaller" now
                //
                //      abcdefghiijklmnopq
                //      ^--^
                //      J=0
                //      S < L - J
                //      4 < 18 - 0
                //      4 < 18
                //      4
                //
                // Iteration two:
                //
                //      Now that we have split it once, we go onto the next substring to split, where J is 4 now
                //      (incrementing by S)
                //      
                //      The process is much the same here, let's skip to the end
                //
                // Iteration four:
                //      J = 16
                //      L - J = 18 - 16 = 2
                //      
                //      S < 2
                //      4 < 2  finally! we're at the end, L - J is finally smaller, and gets picked over S
                //
                //      this means we split off the remaining two characters
                */      
                outList.Add(line.Substring(j, Math.Min(Width, line.Length - j)));
            }
        }

        parsed = outList.ToArray();
    }

    /// <summary>
    /// Toggles visibility
    /// </summary>
    public void ShowToggle()
    {
        Show ^= true;
    }

    /// <summary>
    /// Specifies the alignment of the text inside the label
    /// <list type="table">
    ///     <item>
    ///         <term>0</term>
    ///         <description>Left-aligned</description>
    ///     </item>
    ///     <item>
    ///         <term>1</term>
    ///         <description>Center-aligned</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>Right-aligned</description>
    ///     </item>
    /// </list>
    /// </summary>
    public byte Alignment { get; set; }

    private string[] parsed = new string[]{};
    private string previousText = "";
    /// <summary>
    /// The text inside the label
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Determines whether or not to display this label
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// Identifier and name for this label
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// Width of Label
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Height of Label
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// The X position of this label
    /// </summary>
    public int XPosition { get; set; }


    /// <summary>
    /// The Y position of this label
    /// </summary>
    public int YPosition { get; set; }

}