using System;
using System.Collections.Generic;

namespace Utilities.Collections {
    public readonly struct list_clear_token<T> : IDisposable {
        readonly List<T> list;
        public list_clear_token(List<T> list) => this.list = list;
        public void Dispose() => list.Clear();
    }

    public static class list_clear_token {
        public static list_clear_token<T> clear_token<T>(this List<T> list) => new list_clear_token<T>(list);
    }
}