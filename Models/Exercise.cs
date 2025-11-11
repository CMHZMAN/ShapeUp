namespace ShapeUp.Models
{
    public class Exercise
    {
        public string? Name { get; set; }
        public string? Difficulty { get; set; }
        public string? Musclegroup { get; set; }

        public Exercise(string name, string difficulty, string musclegroup)
        {
            Name = name;
            Difficulty = difficulty;
            Musclegroup = musclegroup;
        }

        public override string ToString()
        {
            return $"Exercise: {Name}, Difficulty: {Difficulty}, Muscle Group: {Musclegroup}";
        }
    }
}

