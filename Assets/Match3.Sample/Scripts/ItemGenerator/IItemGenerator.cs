using System;

namespace Match3
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(int capacity);
    }
}