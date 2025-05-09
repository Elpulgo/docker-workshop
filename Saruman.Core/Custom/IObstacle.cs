namespace Saruman.Core.Custom;

public interface IObstacle
{
    Task Execute();

    int Priority();
}