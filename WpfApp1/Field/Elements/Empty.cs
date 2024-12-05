

using System;

namespace WpfApp1.FieldElements
{
    [Serializable]
    public class Empty : Cell
    {
        public override bool IsEmpty() => true;

        public override bool IsTent() => false;

        public override bool IsTree()=> false;
    }
}
