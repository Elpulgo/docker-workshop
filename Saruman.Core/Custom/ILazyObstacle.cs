namespace Saruman.Core.Custom;

public interface ILazyObstacle
{
    Task<T> Execute<T, T2, T3>(T2 t2, T3 t3) where T : class;
}