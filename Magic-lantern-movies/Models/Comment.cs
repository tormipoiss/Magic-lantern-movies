namespace True_Comment
{
    public class Comment
    {
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        public Comment() { }

        public Comment(Guid movieId, string userName, string text)
        {
            MovieId = movieId;
            UserName = userName;
            Text = text;
            DatePosted = DateTime.Now;
        }

        public void DisplayComment()
        {
            Console.WriteLine($"{UserName} (Posted on {DatePosted}): {Text}");
        }
    }
}
