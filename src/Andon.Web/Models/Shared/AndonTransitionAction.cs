namespace Andon.Web.Models.Shared;

public enum AndonTransitionAction
{
    Create = 1,
    Acknowledge = 2,
    Assign = 3,
    Escalate = 4,
    Resolve = 5,
    Cancel = 6,
    Comment = 7,
    CreateRca = 8
}
