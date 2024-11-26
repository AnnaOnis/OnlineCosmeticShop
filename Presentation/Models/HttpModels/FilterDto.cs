namespace HttpModels
{
    public class FilterDto
    {
        public string? Filter { get; set; } // Общий параметр поиска
        public string? SortField { get; set; } // Общий параметр сортировки
        public bool SortOrder { get; set; } = false; // Общий параметр для указания порядка сортировки
        public int PageNumber { get; set; } = 1; // Номер страницы
        public int PageSize { get; set; } = 10; // Количество элементов на странице

        // Параметр, специфичный для продуктов
        public Guid? CategoryId { get; set; }
    }
}
