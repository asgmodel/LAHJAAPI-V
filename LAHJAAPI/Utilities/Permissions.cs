using System.Reflection;

namespace LAHJAAPI.Utilities
{
    public static class Permissions
    {
        public const string ViewPlan = "View Plan";
        public const string DeletePlan = "Delete Plan";


        public static async Task<List<string>> GetFields()
        {
            List<FieldInfo> fields = typeof(Permissions).GetFields(BindingFlags.Static | BindingFlags.Public).ToList();

            List<string> permitions = new List<string>();
            foreach (var field in fields)
            {
                var value = field.GetValue(null).ToString();
                permitions.Add(value);
            }
            return permitions;
        }
    }


}