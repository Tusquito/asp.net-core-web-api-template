namespace Backend.Libs.Database.Account;

public enum RoleType : short
{
    User = 0,
    Tester = 1,
    Root = short.MaxValue
}