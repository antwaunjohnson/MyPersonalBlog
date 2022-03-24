using System.ComponentModel;

namespace MyPersonalBlog.Enums;

public enum ModerationType
{
    [Description("Violent speech")]
    Violence,
    [Description("Offensive language")]
    Language,
    [Description("Drug references")]
    Drugs,
    [Description("Sexual references")]
    Sexual,
    [Description("Threating speech")]
    Threatening,
    [Description("Political references")]
    Political
}
