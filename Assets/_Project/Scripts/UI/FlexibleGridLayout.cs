using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI {
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        public FitType fitType;
        
        public int rows;
        public int columns;
        
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType is FitType.Width or FitType.Width or FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                
                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType is FitType.Width or FitType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }

            if (fitType is FitType.Height or FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            Rect rect = rectTransform.rect;
            float parentWidth = rect.width;
            float parentHeight = rect.height;

            float cellWidth = parentWidth / (float)columns - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
            float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float) rows) - (padding.bottom / (float)rows);

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = i / columns;
                var columCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columCount) + (spacing.x * columCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 0, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            throw new System.NotImplementedException();
        }

        public override void SetLayoutHorizontal()
        {
            throw new System.NotImplementedException();
        }

        public override void SetLayoutVertical()
        {
            throw new System.NotImplementedException();
        }
    }
}
