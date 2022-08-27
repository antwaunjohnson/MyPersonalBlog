using System.ComponentModel;

namespace MyPersonalBlog.Enums;

public enum ModerationType
{
    [Description("Violent speech")]
    Violence = 0,
    [Description("Offensive language")]
    Language = 1,
    [Description("Drug references")]
    Drugs = 2,
    [Description("Sexual references")]
    Sexual = 3,
    [Description("Threating speech")]
    Threatening = 4,
    [Description("Political references")]
    Political = 5
}
