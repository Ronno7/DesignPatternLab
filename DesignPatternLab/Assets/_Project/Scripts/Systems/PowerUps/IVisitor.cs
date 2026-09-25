using DesignPatternLab.Characters.Bike;

namespace DesignPatternLab.Systems.PowerUps
{
    // Chapter 10: one overload for each kind of visitable bike element.
    public interface IVisitor
    {
        void Visit(BikeShield shield);
        void Visit(BikeEngine engine);
        void Visit(BikeWeapon weapon);
    }
}
