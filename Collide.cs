using curses0;

namespace cetest
{
    internal class Collide
    {

        static public bool CollidesWith(IRenderable objOne, IRenderable objTwo)
        {
            return objOne.XPosition + objOne.Width >= objTwo.XPosition &&
                    objTwo.XPosition + objTwo.Width >= objOne.XPosition &&
                           objOne.YPosition + objOne.Height >= objTwo.YPosition &&
                   objTwo.YPosition + objTwo.Height >= objOne.YPosition;
        }

        static public bool CollidesWithRight(IRenderable objOne, IRenderable objTwo)
        {
            return CollidesWith(objOne, objTwo) & objOne.XPosition + objOne.Width <= objTwo.XPosition;
        }

        static public bool CollidesWithLeft(IRenderable objOne, IRenderable objTwo)
        {
            return CollidesWith(objOne, objTwo) & objTwo.XPosition + objTwo.Width <= objOne.XPosition;
        }

        static public bool CollidesWithTop(IRenderable objOne, IRenderable objTwo)
        {
            return CollidesWith(objOne, objTwo) & objOne.YPosition + objOne.Height <= objTwo.YPosition;
        }

        static public bool CollidesWithBottom(IRenderable objOne, IRenderable objTwo)
        {
            return CollidesWith(objOne, objTwo) & objTwo.YPosition + objTwo.Height <= objOne.YPosition;
        }

    }
}
