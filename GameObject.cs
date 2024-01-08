using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace UnnamedColonySurvivalGame
{
    class GameObject
    {
        public Vector3 localPosition;
        public Vector3 Position
        {
            get => _parent ? _parent.Position + Vector3.Transform(localPosition, _parent.Rotation) : localPosition;
        }
        public Vector3 Forward
        {
            get => CalculateGlobalDirection(Vector3.UnitZ);
        }
        public Vector3 Left
        {
            get => CalculateGlobalDirection(Vector3.UnitX);
        }
        public Vector3 Up
        {
            get => CalculateGlobalDirection(Vector3.UnitY);
        }
        public Quaternion localRotation;
        public Quaternion Rotation
        {
            get => _parent ? _parent.Rotation * localRotation : localRotation;
        }

        GameObject _parent;
        public GameObject parent
        {
            get => _parent;
            set
            {
                if (value != _parent)
                {
                    if (_parent)
                    {
                        _parent.children.Remove(this);
                    }
                    _parent = value;
                    if (value)
                    {
                        _parent.children.Add(this);
                    }
                }
            }
        }
        List<GameObject> children { get; }
        public List<Component> componentList { get; }

        public GameObject(GameObject parent = null) : this(Vector3.Zero, parent)
        {
            
        }

        public GameObject(Vector3 localPosition, GameObject parent = null) : this(localPosition, Quaternion.Identity, parent)
        {

        }

        public GameObject(Quaternion localRotation, GameObject parent = null) : this(Vector3.Zero, localRotation, parent)
        {

        }

        public GameObject(Vector3 localPosition, Quaternion localRotation, GameObject parent = null)
        {
            _parent = parent;
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            componentList = new List<Component>();
        }

        Vector3 CalculateGlobalDirection(Vector3 direction) => Vector3.Transform(direction, Rotation).Normalized();

        public bool TryGetChild(int index, out GameObject child)
        {
            bool childExists = index >= 0 && index < children.Count;
            child = childExists ? children[index] : null;
            return childExists;
        }

        public static implicit operator bool(GameObject gameObject) => gameObject != null;
    }
}
