module CrudTests

open Xunit
open Models
open CRUD

[<Fact>]
let ``Add student adds student to state`` () =
    let state =
        {
            Students = []
            CurrentUserRole = Admin
        }

    let student =
        {
            Id = "1"
            FirstName = "Mahmoud"
            LastName = "Gabry"
            Grades = []
        }

    let result = addStudent state student

    match result with
    | Ok newState ->
        Assert.Equal(1, newState.Students.Length)
    | Error _ ->
        Assert.True(false)


[<Fact>]
let ``Delete student removes student from state`` () =
    let student =
        {
            Id = "1"
            FirstName = "Mahmoud"
            LastName = "Gabry"
            Grades = []
        }

    let state =
        {
            Students = [ student ]
            CurrentUserRole = Admin
        }

    let result = deleteStudent state "1"

    match result with
    | Ok newState ->
        Assert.Equal(0, newState.Students.Length)
    | Error _ ->
        Assert.True(false)

[<Fact>]
let ``Update student updates student data`` () =

    let Student =
        {
            Id = "1"
            FirstName = "Mahmoud"
            LastName = "Mohamed"
            Grades = []
        }

    let state =
        {
            Students = [ Student ]
            CurrentUserRole = Admin
        }

    let updatedStudent =
        {
            Id = "1"
            FirstName = "Mahmoud"
            LastName = "Gabry"
            Grades = []
        }


    let result = updateStudent state updatedStudent

    match result with
    | Ok newState ->
        let studentAfterUpdate = newState.Students.Head
        Assert.Equal("Gabry", studentAfterUpdate.LastName)
    | Error _ ->
        Assert.True(false)
