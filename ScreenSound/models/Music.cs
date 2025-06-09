using System;

namespace screensound.models
{
    public class Music
    {
        public Music(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return 
$@"                Id: {Id}
                Name: {Name}";
        }
    }
}