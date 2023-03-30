using ErrorOr;

namespace BuberBreakfast.ServiceErrors
{
    public static class Errors
    {
        public static class Breakfast
        {
            public static Error InvalidName => Error.Validation(
                code: "Breakfast.InvalidName",
                description: $"Breakfast name must bet between {Models.Breakfast.MinNameLength} and {Models.Breakfast.MaxNameLength}"
                );

            public static Error InvalidDescription => Error.Validation(
                code: "Breakfast.InvalidDescription",
                description: $"Breakfast name must bet between {Models.Breakfast.MinDescLength} and {Models.Breakfast.MaxDescLength}"
                );
            public static Error NotFound => Error.NotFound(
                code: "Breakfast.NotFound",
                description: "Breakfast not found"
                );
        }
    }
}
