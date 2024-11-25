namespace Jam.Core
{

    public interface ITickable
    {
        public void Tick(float deltaTime);
    }

    public interface IFixedTickable
    {
        public void FixedTick();
    }

    public interface ILateTickable
    {
        public void LateTick();
    }

}