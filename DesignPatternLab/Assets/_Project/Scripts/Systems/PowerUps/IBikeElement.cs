namespace DesignPatternLab.Systems.PowerUps
{
    public interface IBikeElement
    {
        void Accept(IVisitor visitor);
    }
}
