namespace API.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Permalink { get; set; }
        public string Date { get; set; }
        public string Image_link_small { get; set; }
        public string Image_link_medium { get; set; }
        public string Desc { get; set; }
        public string Body { get; set; }
    }
}
