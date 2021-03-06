﻿namespace Mermaider.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Simplified case insensitive list of strings
    /// </summary>
    public class OicList
    {
        private readonly List<string> _internalList = new List<string>();

        public List<string> CopyOfAll()
        {
            return new List<string>(_internalList);
        }

        public bool Any()
        {
            return _internalList.Any();
        }

        public void Add(string value)
        {
            if (_internalList.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }
            _internalList.Add(value);
        }

        public bool Has(string value)
        {
            if (_internalList.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return false;
        }

        public void TryRemove(string value)
        {
            var items = _internalList.Where(s => s.Equals(value, StringComparison.OrdinalIgnoreCase));
            var indicies = new List<int>();
            foreach (var item in items)
            {
                indicies.Add(_internalList.IndexOf(item));
            }

            indicies = indicies.OrderByDescending(i => i).ToList();

            for (var i = indicies.Count - 1; i >= 0; i--)
            {
                _internalList.RemoveAt(indicies[i]);
            }
        }
    }
}