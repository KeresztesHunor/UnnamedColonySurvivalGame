using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace UnnamedColonySurvivalGame
{
    class Camera : Component
    {
        float _nearClippingPlaneThreshold;
        public float nearClippingPlaneThreshold
        {
            get => _nearClippingPlaneThreshold;
            set
            {
                float clampedValue = ClampClippingPlaneThreshold(value);
                if (clampedValue != _nearClippingPlaneThreshold)
                {
                    _nearClippingPlaneThreshold = clampedValue;
                    ResetProjectionMatrixToBeRecalculated();
                }
            }
        }
        float _farClippingPlaneThreshold;
        public float farClippingPlaneThreshold
        {
            get => _farClippingPlaneThreshold;
            set
            {
                float clampedValue = ClampClippingPlaneThreshold(value);
                if (clampedValue != _farClippingPlaneThreshold)
                {
                    _farClippingPlaneThreshold = clampedValue;
                    ResetProjectionMatrixToBeRecalculated();
                }
            }
        }
        float _fov;
        public float fov
        {
            get => _fov;
            set
            {
                float clampedValue = Math.Clamp(value, 30f, 150f);
                if (clampedValue != _fov)
                {
                    _fov = clampedValue;
                    ResetProjectionMatrixToBeRecalculated();
                }
            }
        }
        public float AspectRatio { get; private set; }

        Vector2i _resolution;
        public Vector2i resolution
        {
            get => _resolution;
            set
            {
                Vector2i clampedValue = new Vector2i(
                    GeneralFunctions.ClampWithMinValue(value.X, 1),
                    GeneralFunctions.ClampWithMinValue(value.Y, 1)
                );
                if (clampedValue != _resolution)
                {
                    _resolution = clampedValue;
                    AspectRatio = _resolution.X / (float)_resolution.Y;
                    ResetProjectionMatrixToBeRecalculated();
                }
            }
        }
        public Matrix4 ViewMatrix
        {
            get => Matrix4.LookAt(HostGameObject.Position, HostGameObject.Position + HostGameObject.Forward, HostGameObject.Up);
        }
        public Matrix4 ProjectionMatrix
        {
            get => getProjectionMatrix();
        }

        Func<Matrix4> getProjectionMatrix;
        Func<Matrix4> recalculateAndGetProjectionMatrix { get; }

        public Camera(NotNullable<GameObject> hostGameObject, Vector2i resolution, float fov, float nearClippingPlaneThreshold, float farClippingPlaneThreshold) : base(hostGameObject)
        {
            this.resolution = resolution;
            _fov = fov;
            _nearClippingPlaneThreshold = nearClippingPlaneThreshold;
            _farClippingPlaneThreshold = farClippingPlaneThreshold;
            recalculateAndGetProjectionMatrix = () => {
                Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(this.fov), AspectRatio, this.nearClippingPlaneThreshold, this.farClippingPlaneThreshold);
                getProjectionMatrix = () => projectionMatrix;
                return projectionMatrix;
            };
            getProjectionMatrix = recalculateAndGetProjectionMatrix;
        }

        void ResetProjectionMatrixToBeRecalculated()
        {
            if (getProjectionMatrix != recalculateAndGetProjectionMatrix)
            {
                getProjectionMatrix = recalculateAndGetProjectionMatrix;
            }
        }

        float ClampClippingPlaneThreshold(float value) => Math.Clamp(value, 0.001f, 1024f);

        public static implicit operator bool(Camera camera) => camera != null;
    }
}
