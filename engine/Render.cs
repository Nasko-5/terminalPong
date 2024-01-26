using Mindmagma.Curses;

namespace cetest.engine;

public class Render
{
    // should be able to hold any type of object that has a Draw method
    public List<IRenderable> objectList = new List<IRenderable>();
    private List<IRenderable> toRender = new List<IRenderable>();

    public IntPtr Window { get; set; }

    /// <summary>
    /// Constructor for the <c>Render</c> class
    /// </summary>
    /// <param name="window"></param>
    public Render(IntPtr window)
    {
        Window = window;
        NCurses.GetMaxYX(Window, out maxY, out maxX);
    }

    /// <summary>
    /// Adds specified object to the object list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public T AddObject<T>(T obj) where T : IRenderable
    {
        objectList.Add(obj);
        return obj;
    }

    /// <summary>
    /// Moves object to the foremost layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public void MoveToTop<T>(T obj) where T : IRenderable
    {
        objectList.Remove(obj);
        objectList.Add(obj);
    }

    /// <summary>
    /// Moves object up one layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public void MoveUp<T>(T obj) where T : IRenderable
    {
        int i = objectList.IndexOf(obj);

        if (i + 1 < objectList.Count)
        {
            objectList.Remove(obj);
            objectList.Insert(i + 1, obj);
        }
    }

    /// <summary>
    /// Moves object down one layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public void MoveDown<T>(T obj) where T : IRenderable
    {
        int i = objectList.IndexOf(obj);
        if (i - 1 >= 0)
        {
            objectList.Remove(obj);
            objectList.Insert(i - 1, obj);
        }
    }

    /// <summary>
    /// Moves object to the last layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public void MoveToBottom<T>(T obj) where T : IRenderable
    {
        objectList.Remove(obj);
        objectList.Insert(0, obj);
    }

    int maxY;
    int maxX;

    /// <summary>
    /// Renders all visible objects
    /// </summary>
    public void RenderObjects()
    {
        getRenderable();

        NCurses.Clear();

        foreach (var o in toRender)
        {
            o.XPosition = (o.XPosition % maxX + maxX) % maxX;
            o.YPosition = (o.YPosition % maxY + maxY) % maxY;

            o.Draw(Window);
        }

        NCurses.Refresh();
    }

    /// <summary>
    /// Returns all objects that have <c>Show</c> set to true
    /// </summary>
    private void getRenderable()
    {
        toRender = objectList
            .Where(obj => obj.Show)
            .ToList();
    }

    /// <summary>
    /// Gets an object from the object list given an ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>
    /// Returns object with requested ID, otherwise
    /// returns null
    /// </returns>
    public IRenderable GetRenderableObject(string ID)
    {
        return objectList.Find(o => o.ID == ID);
    }

    public override string ToString()
    {
        return $"Rendering currently:\n{string.Join('\n', toRender.Select(a => $"  {toRender.IndexOf(a)}.{a.GetType()} {a.ID} - X:{a.XPosition} Y:{a.YPosition}"))}";
    }


}