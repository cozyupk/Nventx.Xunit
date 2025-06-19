using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/// This file is a MEMO for future reference.

/*

namespace Xproof.Abstractions.TestProofForTestMethods
{
    /// <summary>
    /// IAwaitableForOutcome is an interface that defines a type that can be awaited for a specific value or exception outcome.
    /// </summary>
    /// <remarks>
    /// Note: This interface is not covariant in T due to CLR restrictions on TaskAwaiter<T>,
    /// which is a value type and invariant. Consumers should not assume assignability variance.
    /// </remarks>
    public interface IAwaitableForOutcome<T>
    {
        /// <summary>
        /// Gets a TaskAwaiter for the outcome of the operation.
        /// </summary>
        public TaskAwaiter<T> GetAwaiter();
    }

    /// <summary>
    /// IAwaitableForValue is an interface that extends IAwaitableForOutcome and is specifically for types that return a value.
    /// </summary>
    public interface IAwaitableForValue<T> : IAwaitableForOutcome<T>
    {
    }

    /// <summary>
    /// IAwaitableForException is an interface that extends IAwaitableForOutcome and is specifically for types that throw an exception.
    /// </summary>
    public interface IAwaitableForException<T> : IAwaitableForOutcome<T>
    {
        /// <summary>
        /// Gets the exception that will be thrown when awaited.
        /// </summary>
        public Exception Exception { get; }
    }

    /// <summary>
    /// AwaitableForValue is a class that implements IAwaitableForValue and provides a way to return a value asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AwaitableForValue<T> : IAwaitableForValue<T>
    {
        /// <summary>
        /// TaskToReturnValue is a Task that will return the specified value when awaited.
        /// </summary>
        private Task<T> TaskToReturnValue { get; }

        /// <summary>
        /// AwaitableForValue constructor initializes the TaskToReturnValue with the provided value.
        /// </summary>
        public AwaitableForValue(T value)
        {
            TaskToReturnValue = Task.FromResult(value);
        }

        /// <summary>
        /// Gets a TaskAwaiter for the value outcome of the operation.
        /// </summary>
        public TaskAwaiter<T> GetAwaiter()
        {
            return TaskToReturnValue.GetAwaiter();
        }
    }

    /// <summary>
    /// AwaitableForException is a class that implements IAwaitableForException and provides a way to throw an exception asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AwaitableForException<T> : IAwaitableForException<T>
    {
        /// <summary>
        /// Exception is the exception that will be thrown when awaited.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// TaskToReturnException is a Task that will throw the specified exception when awaited.
        /// </summary>
        private Task<T> TaskToReturnException { get; }

        /// <summary>
        /// AwaitableForException constructor initializes the TaskToReturnException with the provided exception.
        /// </summary>
        public AwaitableForException(Exception exception)
        {
            Exception = exception;
            TaskToReturnException = Task.FromException<T>(exception);
        }

        /// <summary>
        /// Gets a TaskAwaiter for the exception outcome of the operation.
        /// </summary>
        public TaskAwaiter<T> GetAwaiter()
        {
            return TaskToReturnException.GetAwaiter();
        }



        public void hoge(object obj)
        {

        }

        public void hoge(Exception ex)
        {

        }
    }
}

*/
