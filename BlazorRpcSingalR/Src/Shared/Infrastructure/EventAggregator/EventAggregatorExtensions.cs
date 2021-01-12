using System.Threading;
using System.Threading.Tasks;

namespace BlazorRpcSingalR.Shared.Infrastructure.EventAggregator
{
    /// <summary>
    /// Extensions for <see cref="IEventAggregator"/>.
    /// </summary>
    public static class EventAggregatorExtensions
    {
        /// <summary>
        /// Subscribes an instance to all events declared through implementations of <see cref = "IHandle{T}" />.
        /// </summary>
        /// <remarks>The subscription is invoked on the thread chosen by the publisher.</remarks>
        /// <param name="eventAggregator"></param>
        /// <param name = "subscriber">The instance to subscribe for event publication.</param>
        public static void SubscribeOnPublishedThread(this IEventAggregator eventAggregator, object subscriber)
        {
            eventAggregator.Subscribe(subscriber, f => f());
        }
        
        /// <summary>
        /// Publishes a message on the current thread (synchrone).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task PublishOnCurrentThreadAsync(this IEventAggregator eventAggregator, object message, CancellationToken cancellationToken)
        {
            return eventAggregator.PublishAsync(message, f => f(), cancellationToken);
        }

        /// <summary>
        /// Publishes a message on the current thread (synchrone).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task PublishOnCurrentThreadAsync(this IEventAggregator eventAggregator, object message)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(message, default);
        }

    }
}