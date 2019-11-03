using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityIoC
{
    public partial class Context
    {
        public static IDictionary<Type, ICollection<ValidState>> ValidatorCollection
        {
            get
            {
                if (validatorlist == null)
                {
                    validatorlist = new Dictionary<Type, ICollection<ValidState>>();
                }

                return validatorlist;
            }
        }

        public static void AddConstraint(
            Type dataType,
            ValidState.Predicate validator,
            string msg,
            When action = When.All
        )
        {
            if (dataType != null)
            {
                ValidState vs = new ValidState();
                vs.predicate = validator;
                vs.message = msg;
                vs.when = action;

                AddToValidatorCollection(dataType, vs);
            }
        }

        private static void AddToValidatorCollection(Type dataType, ValidState vs)
        {
            if (!ValidatorCollection.ContainsKey(dataType))
            {
                ValidatorCollection[dataType] = new HashSet<ValidState>();
            }

            ValidatorCollection[dataType].Add(vs);
        }

        public static bool RemoveConstraint<T>(string msg = null, When when = When.All)
        {
            return RemoveConstraint(typeof(T), msg, when);
        }

        public static bool RemoveConstraint<T>(When when = When.All)
        {
            return RemoveConstraint(typeof(T), string.Empty, when);
        }

        public delegate bool RefPredicate<T>(ref T obj);

        public static void AddConstraint<T>(
            RefPredicate<T> validator,
            string msg,
            When action = When.All
        )
        {
            var dataType = typeof(T);
            ValidState vs = new ValidState();
            vs.predicate = (ref object obj) =>
            {
                var t = (T) obj;
                return validator(ref t);
            };
            vs.message = msg;
            vs.when = action;

            AddToValidatorCollection(dataType, vs);
        }

        public static void AddConstraint<T>(
            Func<T, bool> validator,
            string msg,
            When action = When.All
        )
        {
            var dataType = typeof(T);
            ValidState vs = new ValidState();
            vs.predicate = (ref object obj) =>
            {
                var t = (T) obj;
                return validator(t);
            };
            vs.message = msg;
            vs.when = action;

            AddToValidatorCollection(dataType, vs);
        }

        public static void ClearConstraints()
        {
            validatorlist?.Clear();
        }

        public static ICollection<ValidState> GetValidators(Type type)
        {
            if (!ValidatorCollection.ContainsKey(type))
            {
                ValidatorCollection[type] = new HashSet<ValidState>();
            }

            return ValidatorCollection[type];
        }

        public static bool RemoveConstraint(Type dataType, When when = When.All)
        {
            return RemoveConstraint(dataType, string.Empty, when);
        }

        public static bool RemoveConstraint(Type dataType, string msg = null, When when = When.All)
        {
            if (ValidatorCollection.ContainsKey(dataType))
            {
                var validStates = ValidatorCollection[dataType];
                foreach (var validState in validStates.ToList())
                {
                    if (when.IsEqual(validState.when))
                    {
                        var legalToRemove = string.IsNullOrEmpty(msg) || validState.message.Equals(msg);
                        if (legalToRemove)
                        {
                            return validStates.Remove(validState);
                        }
                    }
                }

                return false;
            }

            return false;
        }
    }
}