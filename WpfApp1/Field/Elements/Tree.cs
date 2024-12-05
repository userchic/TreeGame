
using System;

namespace WpfApp1.FieldElements
{
    [Serializable]
    public class Tree : Cell
    {
        public override bool IsEmpty() => false;

        public override bool IsTent() => false;

        public override bool IsTree() => true;
    }
}
