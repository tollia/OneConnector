using System;

namespace OneConnector.Services.Utils
{
    public class DisposableWrapper<T> : IDisposable
    {
        public T Base { get { return _baseObject; } }

        private readonly T _baseObject;
        private readonly Action<T> _disposeAction;

        public DisposableWrapper(T obj, Action<T> disposeAction)
        {
            _baseObject = obj;
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction(_baseObject);
        }
    }

}
