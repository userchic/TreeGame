

using System;

namespace WpfApp1.FieldElements
{
    [Serializable]
    public class Tent : Cell
    {
        public override bool IsEmpty() => false;

        public override bool IsTent() => true;

        public override bool IsTree() => false;
    }
}
