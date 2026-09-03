classDiagram
    %% Core Entities
    class User {
        +int Id
        +string UserId
        +string FullName
        +string Email
        +ICollection~WorkspaceMember~WorkspaceMembers
        +ICollection~Todo~ Todos
        +ICollection~Query~ Queries
        +ICollection~Notification~ Notifications
        +CreateWorkspace(string name) Workspace
    }

    class Workspace {
        +int Id
        +string Name
        +WorkspaceStatus Status
        +bool IsDeleted
        +ICollection~WorkspaceMember~ WorkspaceMembers
        +ICollection~Todo~ Todos
        +ICollection~Message~ Messages
        +AddMember(int userId, Role role) void
        +RemoveMember(int userId) void
        +UpdateStatus(WorkspaceStatus newStatus) void
    }

    class WorkspaceMember {
        +int Id
        +int WorkspaceId
        +int UserId
        +Role Role
        +ChangeRole(Role newRole) void
    }

    class Todo {
        +int Id
        +string Title
        +string Description
        +Status Status
        +int WorkspaceId
        +int AssignedUserId
        +DateTime CreatedAt
        +ICollection~Query~ Queries
        +UpdateStatus(Status newStatus) void
        +AssignUser(int userId) void
    }

    class Query {
        +int Id
        +string Body
        +string FileName
        +string FilePath
        +int UserId
        +int TodoId
        +UpdateBody(string newBody) void
        +AttachFile(string fileName, string filePath) void
    }

    class Notification {
        +int Id
        +string Title
        +string Message
        +int UserId
        +bool IsRead
        +RedirectLink RedirectLink
        +int RedirectId
        +DateTime CreatedTime
        +MarkAsRead() void
    }
    
    class Message {
        +int Id
        +string Body
        +int WorkspaceId
        +int UserId
        +DateTime CreatedTime
        +EditMessage(string newBody) void
        +DeleteMessage() void
    }

    %% Enumerations
    class WorkspaceStatus {
        <<enumeration>>
        Running
        Postponed
        Completed
    }

    class Role {
        <<enumeration>>
        Admin
        Coordinator
        Contributor
    }

    class Status {
        <<enumeration>>
        Pending
        Processing
        Completed
    }

    class RedirectLink {
        <<enumeration>>
        Todo
        Project
    }

    %% Defining Relationships (One-to-Many)
    User "1" --> "*" WorkspaceMember : Enrolls as
    User "1" --> "*" Notification : Receives
    User "1" --> "*" Todo : Assigned to
    User "1" --> "*" Query : Submits
    User "1" --> "*" Message : Sends

    Workspace "1" *-- "*" WorkspaceMember : Contains (Composition)
    Workspace "1" *-- "*" Todo : Owns (Composition)
    Workspace "1" *-- "*" Message : Hosts (Composition)

    Todo "1" *-- "*" Query : Contains
    
    %% Enum Associations
    Workspace ..> WorkspaceStatus
    WorkspaceMember ..> Role
    Todo ..> Status
    Notification ..> RedirectLink