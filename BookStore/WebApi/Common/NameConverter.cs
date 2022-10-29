namespace WebApi.Common
{
    public static class NameConverter
    {
        public static string ConvertToFullName(string firstName, string? middleName, string lastName) => 
            (firstName + " " + (middleName != null ? (middleName + " ") : string.Empty) + lastName);
    }
}