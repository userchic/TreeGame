
namespace WpfApp1.FieldElements
{
    public class Tree : Cell
    {
        public override bool IsEmpty() => false;

        public override bool IsTent() => false;

        public override bool IsTree() => true;
    }
}
