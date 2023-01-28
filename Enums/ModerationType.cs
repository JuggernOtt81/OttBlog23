using System.ComponentModel;

namespace OttBlog23.Enums
{
    public enum ModerationType
    {
        [Description("not relevant to the content of the post")]
        off_topic,
        [Description("aggressive pressure or intimidation")]
        harassment,
        [Description("showing a desire to cause physical or emotional harm - malicious")]
        hateful,
        [Description("fraudulent or deceptive")]
        scamming
    }
}
