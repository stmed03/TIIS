namespace PR1_Elasticsearch.Models
{
    public class ArticleDocument
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Основное поле для полнотекстового поиска
        public string Content { get; set; } = string.Empty;
    }
}
