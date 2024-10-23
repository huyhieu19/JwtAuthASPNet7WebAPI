using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using System.ComponentModel;
using System.Reflection;

namespace JwtAuthASPNet7WebAPI.Core.Utils
{
    public static class StaticExtentions
    {

        public static string GetEnumDescription<T>(this T enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()
                .Description;
        }

        public static string JoinExtention<T>(this T sourse, string separator = ", ")
        {
            return string.Join(separator, sourse);
        }

        public static string JoinRoles(this List<string> source)
        {
            if (source == null || !source.Any())
                return string.Empty;
            if (source.Count == 1)
            {
                return source[0];
            }
            return string.Join(" - ", source);
        }

        public static string JoinRoles(this List<RoleType> source)
        {
            if (source == null || !source.Any())
                return string.Empty;

            if (source.Count == 1)
            {
                return source[0].DescriptionAttribute();
            }

            // Project the list to their DescriptionAttribute and join them with "- "
            return string.Join(" - ", source.Select(role => role.DescriptionAttribute()));
        }

        public static List<RoleType> SplitRoles(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return new List<RoleType>();

            var rolesString = source.Split(new string[] { "-" }, StringSplitOptions.None);
            List<RoleType> roles = new List<RoleType>();
            foreach (var item in rolesString)
            {
                roles.Add((RoleType)Enum.Parse(typeof(RoleType), item.Trim()));
            }
            return roles;
        }


        public static string DescriptionAttribute<T>(this T source)
        {
            FieldInfo fieldInfo = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else return source.ToString();
        }
    }
}
