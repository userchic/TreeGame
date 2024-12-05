using System;

namespace WpfApp1.FieldElements
{
    [Serializable]
    public abstract class Cell
    {
        public abstract bool IsEmpty();
        public abstract bool IsTent();
        public abstract bool IsTree();
    }
}