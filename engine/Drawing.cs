namespace cetest.engine;

using Mindmagma.Curses;

public class Drawing
{

    public static void FreeLine(IntPtr Window, int x1, int y1, int x2, int y2, char c)
    {
        double x = x1;
        double y = y1;
        double dx = x2 - x1;
        double dy = y2 - y1;

        double steps = dx * Convert.ToInt16(dx > dy) + dy * Convert.ToInt16(dy > dx);

        double xIncrement = dx / steps;
        double yIncrement = dy / steps;

        for (int i = 0; i < steps; i++)
        {
            x += xIncrement;
            y += yIncrement;
            NCurses.MoveWindowAddChar(Window, (int)y, (int)x, c);
        }
    }

    public static void VerticalLine(IntPtr Window, int x, int y, int length, char c)
    {
        for (int i = 0; i < length + 1; i++)
        {
            Utils.PutChar(Window, 1, y + i * Math.Sign(i), x, c);
        }
    }

    public static void VerticalLine(IntPtr Window, int x, int y, int length, string pattern)
    {
        for (int i = 0; i < length + 1; i++)
        {
            Utils.PutChar(Window, 1, y + i * Math.Sign(i), x, pattern[i % pattern.Length]);
        }
    }

    public static void HorizontalLine(IntPtr Window, int x, int y, int length, char c)
    {
        for (int i = 0; i < length; i++)
        {
            Utils.PutChar(Window, 1, y, x + i * Math.Sign(i), c);
        }
    }

    public static void HorizontalLine(IntPtr Window, int x, int y, int length, string pattern)
    {
        for (int i = 0; i < length; i++)
        {
            Utils.PutChar(Window, 1, y, x + i * Math.Sign(i), pattern[i % pattern.Length]);
        }
    }
}