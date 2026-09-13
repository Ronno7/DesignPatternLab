namespace DesignPatternLab.Characters.Bike
{
    // Adapted from David Baron, Chapter 5.
    public class BikeStateContext
    {
        public IBikeState CurrentState { get; set; }

        private readonly BikeController _bikeController;

        public BikeStateContext(BikeController bikeController)
        {
            _bikeController = bikeController;
        }

        public void Transition()
        {
            CurrentState.Handle(_bikeController);
        }

        public void Transition(IBikeState state)
        {
            CurrentState = state;
            Transition();
        }
    }
}
