using UnityEngine;
using UnityEngine.UIElements;

namespace IRG
{
    public class DragAndDropManipulator : PointerManipulator
    {
        private bool _active;
        private Vector3 _start;
        private int _pointerId;

        public DragAndDropManipulator()
        {
            _pointerId = -1;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            _active = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected void OnPointerDown(PointerDownEvent e)
        {
            if (_active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                _active = true;
                _pointerId = e.pointerId;
                target.CapturePointer(_pointerId);
                _start = e.localPosition;
                e.StopPropagation();
            }
        }

        protected void OnPointerMove(PointerMoveEvent e)
        {
            if (!_active || !target.HasPointerCapture(_pointerId))
                return;

            Vector2 diff = e.localPosition - _start;

            target.style.top = target.layout.y + diff.y;
            target.style.left = target.layout.x + diff.x;

            e.StopPropagation();
        }

        protected void OnPointerUp(PointerUpEvent e)
        {
            if (!_active || !target.HasPointerCapture(_pointerId) || !CanStopManipulation(e))
                return;

            _active = false;
            target.ReleasePointer(_pointerId);
            _pointerId = -1;
            e.StopPropagation();
        }
    }
}