using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Utils.CustomAttributes.ShowIf
{
    /// <summary>
    ///     Defines the <see cref="ShowIfAttributeDrawer" />.
    /// </summary>
    [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        ///     The GetMethod.
        /// </summary>
        /// <param name="target">The target<see cref="object" />.</param>
        /// <param name="methodName">The methodName<see cref="string" />.</param>
        /// <returns>The <see cref="MethodInfo" />.</returns>
        private static MethodInfo GetMethod(object target, string methodName)
        {
            return GetAllMethods(target, m => m.Name.Equals(methodName,
                StringComparison.InvariantCulture)).FirstOrDefault();
        }

        /// <summary>
        ///     The GetField.
        /// </summary>
        /// <param name="target">The target<see cref="object" />.</param>
        /// <param name="fieldName">The fieldName<see cref="string" />.</param>
        /// <returns>The <see cref="FieldInfo" />.</returns>
        private static FieldInfo GetField(object target, string fieldName)
        {
            return GetAllFields(target, f => f.Name.Equals(fieldName,
                StringComparison.InvariantCulture)).FirstOrDefault();
        }

        /// <summary>
        ///     The GetAllFields.
        /// </summary>
        /// <param name="target">The target<see cref="object" />.</param>
        /// <param name="predicate">The predicate<see cref="Func{FieldInfo, bool}" />.</param>
        /// <returns>The <see cref="IEnumerable{FieldInfo}" />.</returns>
        private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
        {
            var types = new List<Type>
            {
                target.GetType()
            };

            while (types.Last().BaseType != null) types.Add(types.Last().BaseType);

            for (var i = types.Count - 1; i >= 0; i--)
            {
                var fieldInfos = types[i]
                    .GetFields(
                        BindingFlags.Instance |
                        BindingFlags.Static |
                        BindingFlags.NonPublic |
                        BindingFlags.Public |
                        BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var fieldInfo in fieldInfos) yield return fieldInfo;
            }
        }

        /// <summary>
        ///     The GetAllMethods.
        /// </summary>
        /// <param name="target">The target<see cref="object" />.</param>
        /// <param name="predicate">The predicate<see cref="Func{MethodInfo, bool}" />.</param>
        /// <returns>The <see cref="IEnumerable{MethodInfo}" />.</returns>
        private static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            var methodInfos = target.GetType()
                .GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.NonPublic |
                    BindingFlags.Public)
                .Where(predicate);

            return methodInfos;
        }

        /// <summary>
        ///     The MeetsConditions.
        /// </summary>
        /// <param name="property">The property<see cref="SerializedProperty" />.</param>
        /// <returns>The <see cref="bool" />.</returns>
        private bool MeetsConditions(SerializedProperty property)
        {
            var showIfAttribute = attribute as ShowIfAttribute;
            var target = property.serializedObject.targetObject;
            var conditionValues = new List<bool>();

            foreach (var condition in showIfAttribute.Conditions)
            {
                var conditionField = GetField(target, condition);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                    conditionValues.Add((bool) conditionField.GetValue(target));

                var conditionMethod = GetMethod(target, condition);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                    conditionValues.Add((bool) conditionMethod.Invoke(target, null));
            }

            if (conditionValues.Count > 0)
            {
                bool met;
                if (showIfAttribute.Operator == ConditionOperator.And)
                {
                    met = true;
                    foreach (var value in conditionValues) met = met && value;
                }
                else
                {
                    met = false;
                    foreach (var value in conditionValues) met = met || value;
                }

                return met;
            }

            Debug.LogError("Invalid boolean condition fields or methods used!");
            return true;
        }

        /// <summary>
        ///     The GetPropertyHeight.
        /// </summary>
        /// <param name="property">The property<see cref="SerializedProperty" />.</param>
        /// <param name="label">The label<see cref="GUIContent" />.</param>
        /// <returns>The <see cref="float" />.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Calcluate the property height, if we don't meet the condition and the draw mode is DontDraw, then height will be 0.
            var meetsCondition = MeetsConditions(property);
            var showIfAttribute = attribute as ShowIfAttribute;

            if (!meetsCondition && showIfAttribute.Action == ActionOnConditionFail.DontDraw) return 0;

            return base.GetPropertyHeight(property, label);
        }

        /// <summary>
        ///     The OnGUI.
        /// </summary>
        /// <param name="position">The position<see cref="Rect" />.</param>
        /// <param name="property">The property<see cref="SerializedProperty" />.</param>
        /// <param name="label">The label<see cref="GUIContent" />.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var meetsCondition = MeetsConditions(property);
            // Early out, if conditions met, draw and go.
            if (meetsCondition)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var showIfAttribute = attribute as ShowIfAttribute;
            if (showIfAttribute.Action == ActionOnConditionFail.DontDraw)
            {
            }
            else if (showIfAttribute.Action == ActionOnConditionFail.JustDisable)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}