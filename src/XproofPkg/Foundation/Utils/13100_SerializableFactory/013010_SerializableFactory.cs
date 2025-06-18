using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xproof.Abstractions.Utils;

namespace Xproof.Utils.SerializableFactory
{
    /// <summary>
    /// A generic, serializable factory that creates instances of TTarget using constructor injection via INamedArgumentResolver.
    /// </summary>
    [Serializable]
    public class SerializableFactory<TTarget>
    {
        /// <summary>
        /// The resolver used to inject parameters for object creation.
        /// </summary>
        private INamedArgumentResolver? resolver;

        /// <summary>
        /// Flag indicating whether parameters have been set for the factory.
        /// </summary>
        private bool isParameterSet = false;

        /// <summary>
        /// A cache for resolved parameter values, allowing serialization and deserialization of the factory state.
        /// </summary>
        private Dictionary<string, object>? cachedValues;

        private static bool IsSupported(Type type)
        {
            return type == typeof(string)
                || type == typeof(bool)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(double)
                || type == typeof(float)
                || type == typeof(Type);
        }

        /// <summary>
        /// Initializes a new instance of the SerializableFactory class.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public SerializableFactory() {
            var ctor = GetTargetConstructor();

            foreach (var param in ctor.GetParameters())
            {
                var type = param.ParameterType;
                if (!IsSupported(type))
                {
                    throw new NotSupportedException(
                        $"Parameter '{param.Name}' of type '{type.FullName}' is not supported for serialization."
                    );
                }
            }
        }

        /// <summary>
        /// Injects parameters for object creation via resolver. Can only be called once.
        /// </summary>
        public void SetParameter(INamedArgumentResolver resolver)
        {
            if (isParameterSet)
                throw new InvalidOperationException("Parameters already set.");

            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            cachedValues = new();
            var ctor = GetTargetConstructor();

            foreach (var param in ctor.GetParameters())
            {
                var value = typeof(INamedArgumentResolver)
                    .GetMethod("Resolve")!
                    .MakeGenericMethod(param.ParameterType)
                    .Invoke(resolver, new object[] { param.Name! })!;

                cachedValues[param.Name!] = value;
            }

            isParameterSet = true;
        }

        /// <summary>
        /// Creates an instance of TTarget using the resolved constructor parameters.
        /// </summary>
        public TTarget Create()
        {
            if (!isParameterSet || resolver == null || cachedValues == null)
                throw new InvalidOperationException("SetParameter must be called first.");

            var ctor = GetTargetConstructor();
            var args = ctor.GetParameters()
                .Select(p => cachedValues[p.Name!])
                .ToArray();

            return (TTarget)ctor.Invoke(args);
        }

        /// <summary>
        /// Serializes the factory state to a JSON string.
        /// </summary>
        public string SerializeToString()
        {
            if (!isParameterSet || cachedValues == null)
                throw new InvalidOperationException("Cannot serialize unset parameters.");

            // 変換専用の辞書を作る
            var serializableDict = cachedValues.ToDictionary(
                kv => kv.Key,
                kv => kv.Value switch
                {
                    Type t => "t" + (object)t.AssemblyQualifiedName!,
                    string s => "s" + s,
                    _ => kv.Value
                }

            );

            return System.Text.Json.JsonSerializer.Serialize(serializableDict);
        }

        /// <summary>
        /// Deserializes the factory state from a JSON string.
        /// </summary>
        public void DeserializeFromString(string json)
        {
            // the JSON string must not be null or empty
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Invalid JSON string", nameof(json));

            // Deserialize the JSON string into a dictionary
            var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(json)
                ?? throw new InvalidOperationException("Deserialization failed.");

            // pass the dictionary to SetParameter after converting the values
            var converted = dict.ToDictionary(
                kv => kv.Key,
                kv => ExtractPrimitive(kv.Value)
            );
            SetParameter(new DictionaryBasedResolver(converted));
        }

        /// <summary>
        /// Gets the target constructor for TTarget, which is the one with the least parameters.
        /// </summary>

        private ConstructorInfo GetTargetConstructor()
        {
            return typeof(TTarget)
                .GetConstructors()
                .OrderBy(c => c.GetParameters().Length)
                .FirstOrDefault()
                ?? throw new InvalidOperationException($"No accessible constructor for {typeof(TTarget)}");
        }

        /// <summary>
        /// Extracts a primitive value from a JSON element.
        /// </summary>
        private static object ExtractPrimitive(System.Text.Json.JsonElement element)
        {
            string strValue;
            switch (element.ValueKind)
            {
                case System.Text.Json.JsonValueKind.String:
                    strValue = element.GetString()!;

                    if (strValue.StartsWith("t"))
                    {
                        var type = Type.GetType(strValue.Substring(1), throwOnError: false);
                        if (type != null)
                            return type;
                    }
                    return strValue.Substring(1);

                case System.Text.Json.JsonValueKind.Number:
                    return element.TryGetInt64(out var l) ? l : element.GetDouble();

                case System.Text.Json.JsonValueKind.True:
                    return true;

                case System.Text.Json.JsonValueKind.False:
                    return false;

                default:
                    throw new NotSupportedException($"Unsupported JSON value: {element}");
            }
        }
    }


    /// <summary>
    /// Simple resolver that returns values from a dictionary.
    /// </summary>
    internal class DictionaryBasedResolver : INamedArgumentResolver
    {
        /// <summary>
        /// The dictionary containing named arguments.
        /// </summary>
        private readonly Dictionary<string, object> _dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBasedResolver"/> class with the specified dictionary.
        /// </summary>
        public DictionaryBasedResolver(Dictionary<string, object> dict)
        {
            _dict = dict;
        }

        /// <summary>
        /// Resolves a named argument of type T from the dictionary.
        /// </summary>
        public T Resolve<T>(string name)
            where T : notnull
        {
            if (_dict.TryGetValue(name, out var value))
            {
                if (value is T typed) return typed;
                if (typeof(T) == typeof(Type) && value is string typeName)
                    return (T)(object)Type.GetType(typeName, throwOnError: true)!;
            }

            throw new KeyNotFoundException($"Key '{name}' not found in resolver.");
        }
    }
}
