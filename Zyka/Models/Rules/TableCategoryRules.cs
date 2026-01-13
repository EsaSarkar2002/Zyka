using Zyka.Models.Enums;

namespace Zyka.Models.Rules
{
    public static class TableCategoryRules
    {
        public static (int Min, int Max) GetSeatCapacity(TableCategory category)
        {
            return category switch
            {
                TableCategory.Date => (2, 2),
                TableCategory.Family => (4, 6),
                TableCategory.Meeting => (8, 12),
                TableCategory.Celebration => (15, 20),
                _ => throw new ArgumentOutOfRangeException(nameof(category))
            };
        }
    }
}
