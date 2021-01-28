public static class GetDescription
    {
        public static string GetEnum<T>(this T t)
        {
            var field = GetCorrectType<T>().GetFields().FirstOrDefault(f => f.Name == t.ToString());
            if (field == null)
                return string.Empty;
            return ((DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute))).Description;
        }

        public static string GetClassField<T>(this T t, string name)
        {
            var prop = GetCorrectType<T>().GetProperties().FirstOrDefault(f => f.Name == name);
            if (prop == null)
                return string.Empty;
            return ((DescriptionAttribute)prop.GetCustomAttribute(typeof(DescriptionAttribute))).Description;
        }

        private static Type GetCorrectType<T>()
        {
            var type = Nullable.GetUnderlyingType(typeof(T));
            if (type == null)
                type = typeof(T);
            return type;
        }
    }