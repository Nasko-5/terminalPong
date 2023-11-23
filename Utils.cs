namespace curses0;

using Mindmagma.Curses;
using System.Runtime.InteropServices;
internal class Utils
{
    public static void PutString(IntPtr Window, byte mode, int y, int x, string str)
    {
        int maxY, maxX;
        NCurses.GetMaxYX(Window, out maxY, out maxX);
        switch (mode)
        {
            case 0:
                NCurses.MoveWindowAddString(Window, y, x, str);
                break;
            case 1:
                NCurses.MoveWindowAddString(Window, y % maxY, x % maxX, str);
                break;
            case 2:
                if (x + str.Length <= maxX && y + str.Count(s => s == '\n') <= maxY)
                {
                    NCurses.MoveWindowAddString(Window, y, x, str);
                }
                break;
        }
    }

    public static void PutChar(IntPtr Window, byte mode, int y, int x, char c)
    {
        int maxY, maxX;
        NCurses.GetMaxYX(Window, out maxY, out maxX);

        try
        {
            switch (mode)
            {
                case 0:
                    NCurses.MoveWindowAddChar(Window, y, x, c);
                    break;
                case 1:
                    NCurses.MoveWindowAddChar(Window, y % maxY, x % maxX, c);
                    break;
                case 2:
                    if (x <= maxX && y <= maxY)
                    {
                        NCurses.MoveWindowAddChar(Window, y, x, c);
                    }
                    break;
            }
        }
        catch { }
    }

    // funny dll import, also probably what's causing the virus warning :(
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int key);
    public static bool IsKeyPressed(int key)
    {
        short state = GetKeyState(key);
        return (state & 128) != 0;
    }

    //  GetKeyState returns an 8 bit number, with the last bit
    //  indicating if the key of the keycode given is pressed or not
    //  10101101 - pressed  00101101 - not pressed
    //  
    //  the isKeyPressed method is a helper method, that calls
    //  GetKeyState with a certain key code, and masks the returned
    //  value with 10000000 by ANDing them together
    //
    //       A:10101101 &       A:00101101 &  
    //       B:10000000         B:10000000
    //  Result:10000000    Result:00000000
    //
    // it then checks if the resulting value is not equal to zero,
    // and returns true if it's pressed, and false if it's not

}
