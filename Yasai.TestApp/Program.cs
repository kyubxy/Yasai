using OpenTK.Mathematics;
using Yasai;
using Yasai.Graphics;

using (Game g = new TestGame())
    g.Run();

public class TestGame : Game
{
    public TestGame()
    {
        Renderer.Add(new Sprite(@"test.png")
        {
            Position = new Vector2(400),
            Size = new Vector2(400)
        });
    }
}
    