namespace cetest.engine
{
    public unsafe interface IRenderable
    {
        public void Draw(IntPtr Window);

        //public bool CollidesWith(IRenderable obj);
        public void ShowToggle();

        public bool Show { get; set; }
        public string ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

    }
}