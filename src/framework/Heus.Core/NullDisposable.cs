﻿
namespace Heus.Core
{
    public sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Instance { get; } = new ();

        private NullDisposable()
        {

        }

        public void Dispose()
        {

        }
    }
}
