using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace UnnamedColonySurvivalGame
{
    class Game : GameWindow
    {
        int screenWidth;
        int screenHeight;

        static bool instanceExists = false;

        ShaderProgram shaderProgram;
        GameObject playerObject;
        GameObject cameraObject;
        Camera camera;

        public static event Action UpdateEvent;
        public static event Action<ShaderProgram> MeshRenderEvent;

        Game(int screenWidth, int screenHeight) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            CenterWindow(new Vector2i(screenWidth, screenHeight));
            playerObject = new GameObject(new Vector3(0.5f, 65f, 1.5f));
            cameraObject = new GameObject(new Vector3(0f, 0.5f, 0f), playerObject);
            camera = new Camera(new NotNullable<GameObject>(cameraObject), new Vector2i(screenWidth, screenHeight), 90f, 0.125f, 128f);
            Player.CreatePlayerObject(new NotNullable<GameObject>(playerObject), new NotNullable<GameObject>(cameraObject));
            CursorState = CursorState.Grabbed;
        }

        public static Game CreateInstance(int screenWidth, int screenHeight)
        {
            if (!instanceExists)
            {
                instanceExists = true;
                return new Game(screenWidth, screenHeight);
            }
            else
            {
                throw new Exception("Instance of type \"Game\" already exists! Cannot have multiple instances of type \"Game\"");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            instanceExists = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            instanceExists = false;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            screenWidth = e.Width;
            screenHeight = e.Height;
            if (camera)
            {
                camera.resolution = new Vector2i(screenWidth, screenHeight);
            }
            GL.Viewport(0, 0, screenWidth, screenHeight);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            shaderProgram = new ShaderProgram("Default.vert", "Default.frag");
            new Chunk(new NotNullable<GameObject>(new GameObject()));
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            Time.UpdateDeltaTime((float)args.Time);
            Input.UpdateKeyDetection(KeyboardState, MouseState);
            UpdateEvent?.Invoke();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0f, 0.5f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.ViewMatrix;
            Matrix4 projection = camera.ProjectionMatrix;
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "model"), true, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "view"), true, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "projection"), true, ref projection);
            MeshRenderEvent?.Invoke(shaderProgram);
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GraphicsData.DeleteAllGraphicsDataInstances();
        }
    }

    enum Inputs
    {
        Forward,
        Backward,
        Right,
        Left,
        Down,
        Up,
        MouseLeft,
        MouseRight,
        MouseMiddle,
    }

    static class Input
    {
        public static float ScrollDelta { get; private set; }

        static readonly Inputs[] inputs = Enum.GetValues<Inputs>();

        public static Vector2 MouseDelta { get; private set; }

        static readonly ReadOnlyEnumKeysDictionary<Inputs, bool> keys = new ReadOnlyEnumKeysDictionary<Inputs, bool>();
        static readonly ReadOnlyEnumKeysDictionary<Inputs, bool> keyDowns = new ReadOnlyEnumKeysDictionary<Inputs, bool>();
        static readonly ReadOnlyEnumKeysDictionary<Inputs, bool> keyUps = new ReadOnlyEnumKeysDictionary<Inputs, bool>();

        static Input()
        {
            ScrollDelta = 0f;
            MouseDelta = new Vector2();
        }

        internal static void UpdateKeyDetection(KeyboardState keyboardState, MouseState mouseState)
        {
            foreach (Inputs input in inputs)
            {
                Enum key = PlayerPrefs.keyboardInputs[input];
                switch (key)
                {
                    case Keys k:
                        UpdateButtonPressData(
                            keyboardState.IsKeyDown(k),
                            keyboardState.IsKeyPressed(k),
                            keyboardState.IsKeyReleased(k)
                        );
                        break;
                    case MouseButton mb:
                        UpdateButtonPressData(
                            mouseState.IsButtonDown(mb),
                            mouseState.IsButtonPressed(mb),
                            mouseState.IsButtonReleased(mb)
                        );
                        break;
                }

                void UpdateButtonPressData(bool keyData, bool keyDownData, bool keyUpData)
                {
                    keys[input] = keyData;
                    keyDowns[input] = keyDownData;
                    keyUps[input] = keyUpData;
                }
            }
            MouseDelta = mouseState.Delta;
            ScrollDelta = mouseState.ScrollDelta.Y;
        }

        public static bool GetKeyDown(Inputs input) => keyDowns[input];

        public static bool GetKey(Inputs input) => keys[input];

        public static bool GetKeyUp(Inputs input) => keyUps[input];
    }

    static class Time
    {
        public static float DeltaTime { get; private set; }

        internal static void UpdateDeltaTime(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}
