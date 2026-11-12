namespace LearnApp.Api.Services
{
    public interface ITransientProbe { Guid Id { get; } }
    public interface IScopedProbe { Guid Id { get; } }
    public interface ISingletonProbe { Guid Id { get; } }

    public class Probe : ITransientProbe, IScopedProbe, ISingletonProbe
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class ProbeReporter(ITransientProbe transient, IScopedProbe scoped, ISingletonProbe singleton)
    {
        public object Snapshot() => new { transient = transient.Id, scoped = scoped.Id, singleton = singleton.Id };
    }
}
