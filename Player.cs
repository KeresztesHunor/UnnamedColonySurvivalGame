using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace UnnamedColonySurvivalGame
{
    class Player : MonoBehavior
    {
        const float speed = 8f;
        const float piOver2 = (float)(Math.PI / 2);

        public static Player Instance { get; private set; }
        GameObject cameraObject { get; }

        Player(NotNullable<GameObject> hostGameObject, NotNullable<GameObject> cameraObject) : base(hostGameObject)
        {
            this.cameraObject = cameraObject.Value;
        }

        public static void CreatePlayerObject(NotNullable<GameObject> hostGameObject, NotNullable<GameObject> cameraObject)
        {
            Instance = new Player(hostGameObject, cameraObject);
        }

        public override void Update()
        {
            if (Input.GetKey(Inputs.Forward))
            {
                HostGameObject.localPosition += MoveHorizontal(HostGameObject.Forward);
            }
            if (Input.GetKey(Inputs.Backward))
            {
                HostGameObject.localPosition -= MoveHorizontal(HostGameObject.Forward);
            }
            if (Input.GetKey(Inputs.Right))
            {
                HostGameObject.localPosition -= MoveHorizontal(HostGameObject.Left);
            }
            if (Input.GetKey(Inputs.Left))
            {
                HostGameObject.localPosition += MoveHorizontal(HostGameObject.Left);
            }
            if (Input.GetKey(Inputs.Up))
            {
                HostGameObject.localPosition.Y += ScaledSpeed();
            }
            if (Input.GetKey(Inputs.Down))
            {
                HostGameObject.localPosition.Y -= ScaledSpeed();
            }
            HostGameObject.localRotation *= new Quaternion(0f, -Input.MouseDelta.X * ScaledMouseSpeed(), 0f);
            cameraObject.localRotation = new Quaternion(Math.Clamp(cameraObject.localRotation.ToEulerAngles().X + Input.MouseDelta.Y * ScaledMouseSpeed(), -piOver2, piOver2), 0f, 0f);

            Vector3 MoveHorizontal(Vector3 direction) => direction * ScaledSpeed();

            float ScaledSpeed() => speed * Time.DeltaTime;
        }

        float ScaledMouseSpeed() => PlayerPrefs.mouseSensitivity * Time.DeltaTime;
    }
}
