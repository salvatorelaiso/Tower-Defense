using UnityEngine;
using Utils.CustomAttributes.ShowIf;

namespace Utils
{
    public class BoxGizmoDrawer2D : MonoBehaviour
    {
        [SerializeField] private readonly Color color = Color.white;

        [SerializeField] private readonly Draw draw = Draw.onSelect;

        [SerializeField] private readonly bool drawAntiDiagonal = false;

        [SerializeField] private readonly bool drawMainDiagonal = false;

        [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(useDifferentColorWhenSelected))]
        public Color onSelectColor = Color.white;

        [SerializeField] private bool useDifferentColorWhenSelected = false;

        private void DrawBox()
        {
            DrawBox(color);
        }

        private void DrawBox(Color color)
        {
            Gizmos.color = color;
            
            var bottomLeftCorner = transform.position - transform.localScale / 2;
            var upperLeftCorner = bottomLeftCorner + Vector3.up * transform.localScale.y;
            var upperRightCorner = transform.position + transform.localScale / 2;
            var bottomRightCorner = upperRightCorner - Vector3.up * transform.localScale.y;

            Gizmos.DrawLine(upperLeftCorner, upperRightCorner);
            Gizmos.DrawLine(bottomLeftCorner, bottomRightCorner);
            Gizmos.DrawLine(upperLeftCorner, bottomLeftCorner);
            Gizmos.DrawLine(upperRightCorner, bottomRightCorner);

            if (drawMainDiagonal) Gizmos.DrawLine(upperLeftCorner, bottomRightCorner);
            if (drawAntiDiagonal) Gizmos.DrawLine(bottomLeftCorner, upperRightCorner);
        }

        private void OnDrawGizmos()
        {
            if (draw == Draw.always) DrawBox();
        }

        private void OnDrawGizmosSelected()
        {
            if (draw != Draw.Never)
            {
                if (useDifferentColorWhenSelected)
                    DrawBox(onSelectColor);
                else
                    DrawBox();
            }
        }

        private enum Draw
        {
            Never,
            always,
            onSelect
        }
    }
}