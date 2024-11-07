namespace LinkyAppBackend.Api.Models.Enums;

public enum GroupRole
{
    Owner, // Can manage group settings 
    ContentManager, // Can manage all links
    Editor, // Can manage own links
    Viewer // Can view links
}