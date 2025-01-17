﻿// This file is part of YamlDotNet - A .NET library for YAML.
// Copyright (c) Antoine Aubry and contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YamlDeserializer.Serialization
{
    internal sealed class LazyComponentRegistrationList<TArgument, TComponent> : IEnumerable<Func<TArgument, TComponent>>
    {
        private readonly List<LazyComponentRegistration> entries = new List<LazyComponentRegistration>();

        public sealed class LazyComponentRegistration
        {
            public readonly Type ComponentType;
            public readonly Func<TArgument, TComponent> Factory;

            public LazyComponentRegistration(Type componentType, Func<TArgument, TComponent> factory)
            {
                ComponentType = componentType;
                Factory = factory;
            }
        }

        public sealed class TrackingLazyComponentRegistration
        {
            public readonly Type ComponentType;
            public readonly Func<TComponent, TArgument, TComponent> Factory;

            public TrackingLazyComponentRegistration(Type componentType, Func<TComponent, TArgument, TComponent> factory)
            {
                ComponentType = componentType;
                Factory = factory;
            }
        }

        public void Add(Type componentType, Func<TArgument, TComponent> factory) => entries.Add(item: new LazyComponentRegistration(componentType, factory));

        public int Count => entries.Count;

        public IEnumerable<Func<TArgument, TComponent>> InReverseOrder
        {
            get
            {
                for (var i = entries.Count - 1; i >= 0; --i)
                {
                    yield return entries[i].Factory;
                }
            }
        }

        public IEnumerator<Func<TArgument, TComponent>> GetEnumerator() => entries.Select(e => e.Factory).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
