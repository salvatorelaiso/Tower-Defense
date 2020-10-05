using System;
using UnityEngine;

namespace Utils.CustomAttributes.ShowIf
{
    public enum ConditionOperator
    {
        // A field is visible/enabled only if all conditions are true.
        And,

        // A field is visible/enabled if at least ONE condition is true.
        Or
    }

    public enum ActionOnConditionFail
    {
        // If condition(s) are false, don't draw the field at all.
        DontDraw,

        // If condition(s) are false, just set the field as disabled.
        JustDisable
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator,
            params string[] conditions)
        {
            Action = action;
            Operator = conditionOperator;
            Conditions = conditions;
        }

        public ActionOnConditionFail Action { get; }
        public ConditionOperator Operator { get; }
        public string[] Conditions { get; }
    }
}