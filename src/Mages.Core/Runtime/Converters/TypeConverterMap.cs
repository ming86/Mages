﻿namespace Mages.Core.Runtime.Converters
{
    using System;
    using System.Collections.Generic;

    sealed class TypeConverterMap
    {
        private readonly Dictionary<Type, Dictionary<Type, Func<Object, Object>>> _cache = new Dictionary<Type, Dictionary<Type, Func<Object, Object>>>();

        public Func<Object, Object> FindConverter(Type to)
        {
            if (to != typeof(Object))
            {
                var mapping = GetConverterMappingFromCache(to);

                return obj =>
                {
                    if (obj != null)
                    {
                        var converter = default(Func<Object, Object>);
                        var type = obj.GetType();

                        if (mapping.TryGetValue(type, out converter))
                        {
                            return converter.Invoke(obj);
                        }
                    }

                    return obj;
                };
            }

            return StandardConverters.Identity;
        }

        public Func<Object, Object> FindConverter(Type from, Type to)
        {
            if (from != to && to != typeof(Object))
            {
                var converters = StandardConverters.List;
                var length = converters.Count;

                for (var i = 0; i < length; ++i)
                {
                    var converter = converters[i];

                    if (converter.From == from && converter.To == to)
                    {
                        return converter.Converter;
                    }
                }

                return StandardConverters.Default;
            }

            return StandardConverters.Identity;
        }

        private Dictionary<Type, Func<Object, Object>> GetConverterMappingFromCache(Type to)
        {
            var mapping = default(Dictionary<Type, Func<Object, Object>>);

            if (!_cache.TryGetValue(to, out mapping))
            {
                var converters = StandardConverters.List;
                var length = converters.Count;
                mapping = new Dictionary<Type, Func<Object, Object>>();

                for (var i = 0; i < length; ++i)
                {
                    var converter = converters[i];

                    if (converter.To == to)
                    {
                        mapping.Add(converter.From, converter.Converter);
                    }
                }

                _cache.Add(to, mapping);
            }

            return mapping;
        }
    }
}
