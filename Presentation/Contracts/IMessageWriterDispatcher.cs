namespace Cozyupk.HelloShadowDI.Presentation.Contracts
{
    /// <summary>
    /// Defines a contract for dispatching messages.
    /// </summary>
    public interface IMessageWriterDispatcher
    {
        /// <summary>
        /// Dispatches a message to the appropriate handler or destination.
        /// </summary>
        void DispatchMessage();
    }
}
