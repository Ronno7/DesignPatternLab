namespace DesignPatternLab.Characters.Drone
{
    // Chapter 11: the context only needs this shared strategy interface.
    public interface IManeuverBehaviour
    {
        void Maneuver(Drone drone);
    }
}
