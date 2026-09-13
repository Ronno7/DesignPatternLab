namespace DesignPatternLab.Characters.Bike
{
    // David Baron, Chapter 5: the contract shared by every bike state.
    public interface IBikeState
    {
        void Handle(BikeController controller);
    }
}
