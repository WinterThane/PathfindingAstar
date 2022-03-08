using System.Collections.Generic;

namespace PathfindingAstar
{
    public partial class Actor
    {
        public static List<Actor> Selection = new List<Actor>();
        public static Actor LastSelected
        {
            get
            {
                return Selection.Count == 0 ? null : Selection[^1];
            }
        }

        public bool IsSelected { get { return Selection.Contains(this); } }

        public void Select()
        {
            if (!IsSelected)
            {
                Selection.Add(this);
            }
        }

        public void Deselect()
        {
            Selection.Remove(this);
        }

        public void ToggleSelect()
        {
            if (IsSelected)
            {
                Selection.Remove(this);
            }
            else
            {
                Selection.Add(this);
            }
        }
    }
}
