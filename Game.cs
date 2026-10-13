using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Egc;

public class Game : GameWindow
{
    private readonly Camera _camera = new();
    private Shader _shader = null!;
    private Triangle _triangle = null!;

    private Vector3 _position = new Vector3(0.3f, 0.2f, 0.0f);
    private Vector3 _offset = new Vector3(0.3f, 0.2f, 0.0f);
    private Color4 _clearColor = new(0.05f, 0.08f, 0.20f, 1.0f);
    private Color4 _tint = new Color4(1.0f, 0.3f, 0.3f, 1.0f);

    private bool _wireframe = false;

    private readonly string _baseTitle;
    private double _elapsed;
    private double _secondTimer;
    private int _updates;
    private int _frames;

    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default,
               new NativeWindowSettings
               {
                   ClientSize = new Vector2i(width, height),
                   Title = title,
                   APIVersion = new Version(3, 3),
                   Profile = ContextProfile.Core,
                   Flags = ContextFlags.ForwardCompatible
               })
    {
        _baseTitle = title;
        VSync = VSyncMode.Off;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        _shader = Shader.FromFiles("Shaders/shader.vert", "Shaders/shader.frag");
        _triangle = new Triangle();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        _camera.Resize(e.Width, e.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        float dt = (float)args.Time;
        _elapsed += args.Time;
        _updates++;

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();

        if (KeyboardState.IsKeyPressed(Keys.D1)) _tint = new Color4(1.0f, 0.0f, 0.0f, 1.0f);
        if (KeyboardState.IsKeyPressed(Keys.D2)) _tint = new Color4(0.0f, 1.0f, 0.0f, 1.0f);
        if (KeyboardState.IsKeyPressed(Keys.D3)) _tint = new Color4(0.0f, 0.0f, 1.0f, 1.0f);
        if (KeyboardState.IsKeyPressed(Keys.D0)) _tint = Color4.White;
        if (KeyboardState.IsKeyPressed(Keys.F)) _wireframe = !_wireframe;

        float t = (float)_elapsed;

        _triangle.SetPosition(0, -0.5f + 0.2f * MathF.Sin(t * 1.0f), -0.5f);
        _triangle.SetPosition(1, 0.5f, -0.5f + 0.25f * MathF.Sin(t * 1.5f));
        _triangle.SetPosition(2, 0.0f, 0.5f + 0.3f * MathF.Sin(t * 2.2f));
        _triangle.UpdateVertices();

        if (KeyboardState.IsKeyPressed(Keys.Space))
        {
            const float cx = 0.0f, cy = 0.0f;
            _triangle.SetPosition(0, cx, cy);
            _triangle.SetPosition(1, cx, cy);
            _triangle.SetPosition(2, cx, cy);
            _triangle.UpdateVertices();
        }

        _secondTimer += args.Time;
        if (_secondTimer >= 1.0)
        {
            Title = $"{_baseTitle} | update/s: {_updates} | render/s: {_frames}";
            _updates = 0;
            _frames = 0;
            _secondTimer -= 1.0;
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _frames++;
        GL.ClearColor(_clearColor.R, _clearColor.G, _clearColor.B, _clearColor.A);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();
        _shader.SetMatrix4("uView", _camera.ViewMatrix);
        _shader.SetMatrix4("uProjection", _camera.ProjectionMatrix);
        _shader.SetVector3("uOffset", _offset);
        _shader.SetColor("uTint", _tint);

        GL.PolygonMode(MaterialFace.FrontAndBack, _wireframe ? PolygonMode.Line : PolygonMode.Fill);
        _triangle.Draw();
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        _triangle.Dispose();
        _shader.Dispose();
        base.OnUnload();
    }
}
