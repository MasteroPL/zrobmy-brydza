namespace Models
{
    public class Player
    {
        public PlayerTag Tag { get; set; }
        public string Name { get; set; }

        public Player(PlayerTag Tag, string Name)
        {
            this.Tag = Tag;
            this.Name = Name;
        }
        
        override
        public string ToString()
        {
            return Tag.ToString() +" "+ Name;
        }
    }
    public enum PlayerTag
    {
        NOBODY = -1,
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }
}
