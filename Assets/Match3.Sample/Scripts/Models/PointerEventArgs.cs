using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3
{
    public class PointerEventArgs : System.EventArgs
    {
        public PointerEventArgs(PointerEventData.InputButton button, Vector3 worldPosition)
        {
            Button = button;
            WorldPosition = worldPosition;
        }

        public Vector3 WorldPosition { get; }
        public PointerEventData.InputButton Button { get; }
    }
}