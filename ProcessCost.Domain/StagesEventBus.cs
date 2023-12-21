using System.Threading.Channels;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;

public interface IStageEvent;

public record StageUpdatedMoneyEvent(Money OldMoney, Stage UpdatedStage) : IStageEvent;

public interface IStagesEventBus
{
    ValueTask Publish(IStageEvent stageEvent);
    IAsyncEnumerable<IStageEvent> Subscribe(CancellationToken cancellationToken);
}

public class StagesEventBus : IStagesEventBus
{
    private readonly Channel<IStageEvent> _channel = Channel.CreateUnbounded<IStageEvent>();

    public ValueTask Publish(IStageEvent stageEvent)
    {
        return this._channel.Writer.WriteAsync(stageEvent);
    }

    public IAsyncEnumerable<IStageEvent> Subscribe(CancellationToken cancellationToken)
    {
        return this._channel.Reader.ReadAllAsync(cancellationToken);
    }
}