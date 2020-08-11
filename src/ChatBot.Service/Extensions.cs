using System;
using System.ComponentModel;

namespace ChatBot.Business.Contracts.MasterData.Attributes
{
    public static class Extensions
    {
        /// <summary>
        ///     Returns the value of the Description attribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            var field = type.GetField(name);
            if (field == null) return null;
            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attr?.Description;
        }
    }
}