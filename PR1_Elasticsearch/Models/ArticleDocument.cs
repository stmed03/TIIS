namespace PR1_Elasticsearch.Models
{
    public class ArticleDocument
    {
        public int Id { get; set; }

        // Основное поле для полнотекстового поиска
        public string Content { get; set; } = string.Empty;
    }
}
