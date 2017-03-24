namespace SmithsonianSearch.Models
{
    using SmithsonianSearch.Models.Enums;

    public class SortingOption
    {
        public string Label { get; set; }

        public SortOptionsEnum Value { get; set; }
    }
}