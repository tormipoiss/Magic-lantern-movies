namespace True_Comment
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        public Comment() { }

        public Comment(string userName, string text)
        {
            UserName = userName;
            Text = text;
            DatePosted = DateTime.Now;
        }

        public Comment(int id, string userName, string text, DateTime datePosted)
        {
            Id = id;
            UserName = userName;
            Text = text;
            DatePosted = datePosted;
        }

        public void DisplayComment()
        {
            Console.WriteLine($"{UserName} (Posted on {DatePosted}): {Text}");
        }
    }
}
