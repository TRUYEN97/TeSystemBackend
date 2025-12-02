namespace TeSystemBackend.Application.Constants;

public static class Permissions
{
    public const string ComputerView = "Computer.View";
    public const string ComputerCreate = "Computer.Create";
    public const string ComputerUpdate = "Computer.Update";
    public const string ComputerDelete = "Computer.Delete";
    
    public const string TeamView = "Team.View";
    public const string TeamCreate = "Team.Create";
    public const string TeamUpdate = "Team.Update";
    public const string TeamDelete = "Team.Delete";
    
    public const string DepartmentView = "Department.View";
    public const string DepartmentCreate = "Department.Create";
    public const string DepartmentUpdate = "Department.Update";
    public const string DepartmentDelete = "Department.Delete";
    
    public const string LocationView = "Location.View";
    public const string LocationCreate = "Location.Create";
    public const string LocationUpdate = "Location.Update";
    public const string LocationDelete = "Location.Delete";
}

public static class Roles
{
    public const string Admin = "ADMIN";
    public const string User = "USER";
    
    public const string PcManager = "PC_Manager";
    public const string PcViewer = "PC_Viewer";
    public const string TeamLead = "Team_Lead";
}
