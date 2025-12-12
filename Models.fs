module Models

type Grade = {
    Subject : string
    Score : float
}

type Student = {
    Id : string
    FirstName : string
    LastName : string
    Grades : Grade list
}

type Role =
    | Admin
    | Viewer

type AppState = {
    Students : Student list
    CurrentUserRole : Role
}

type User = {
    Email : string
    Password : string
    Role : string
}