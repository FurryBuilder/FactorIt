//////////////////////////////////////////////////////////////////////////////////
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Furry Builder
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////////

using System;
using FactorIt.Patterns;
using JetBrains.Annotations;

namespace FactorIt
{
    /// <summary>
    ///     Represent a registration in a dictionary based on a type of contract
    ///     and a sub-set key.
    /// </summary>
    public class RegistrationKey : KeyBase<RegistrationKey>
    {
        private RegistrationKey([CanBeNull] string key, [NotNull] Type type)
        {
            Key = key;
            Type = type;
        }

        public string Key { get; private set; }
        public Type Type { get; private set; }

        public static RegistrationKey From<T>([CanBeNull] string key)
        {
            return new RegistrationKey(key, typeof (T));
        }

        protected override string GetStringValues()
        {
            return
                "Key=\"" + Key + "\", " +
                "Type=\"" + Type + "\"";
        }

        public class Comparer : KeyComparerBase<Comparer>
        {
            public override bool Equals([CanBeNull] RegistrationKey x, [CanBeNull] RegistrationKey y)
            {
                if (x == null && y != null)
                {
                    return false;
                }

                if (x != null && y == null)
                {
                    return false;
                }

                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return
                    string.Equals(x.Key, y.Key) &&
                    x.Type == y.Type;
            }

            public override int GetHashCode([NotNull] RegistrationKey obj)
            {
                unchecked
                {
                    return
                        ((obj.Key != null ? obj.Key.GetHashCode() : 0) * 397) ^
                        obj.Type.GetHashCode();
                }
            }
        }
    }
}