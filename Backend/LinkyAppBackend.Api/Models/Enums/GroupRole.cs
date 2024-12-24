namespace LinkyAppBackend.Api.Models.Enums;

public enum GroupRole
{
    Viewer, // Can view links
    Editor, // Can manage own links
    ContentManager, // Can manage all links
    Owner // Can manage group settings 
}