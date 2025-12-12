module CRUD

open Models    // open Models.fs
open Persistence


let private path = "students.json"

// get students
let getStudents () =
    match Persistence.load path with
    | Ok list -> list
    | Error e -> []

// get student by id
let getStudent (state: AppState) (id: string) : Result<Student,string> =
    match state.Students |> List.tryFind (fun s -> s.Id = id) with
    | Some stu -> Ok stu
    | None -> Error $"Student with ID '{id}' not found."


//add student to the student list
let addStudent (state: AppState) (student: Student) =
    if state.Students |> List.exists (fun s -> s.Id = student.Id) then  // check if the ID already exists or not
        Error $"Student with ID '{student.Id}' already exists."
    else
        let newState = {state with Students = student :: state.Students}
        Persistence.save path newState
        Ok newState  // return new state with the new student


// update student
let updateStudent (state: AppState) ( student: Student) =
    let exists = state.Students |> List.exists (fun s -> s.Id = student.Id)   // check if the ID already exists or not
    if not exists then 
        Error $"Student with ID '{id}' not found."
    else
        let newState = {state with Students = state.Students |> List.map (fun s -> if s.Id = student.Id then student else s)}
        Persistence.save path newState
        Ok newState // return new state with updated student


// delete student from student list
let deleteStudent (state: AppState) (id: string) =
    let exists = state.Students |> List.exists (fun s -> s.Id = id)  // check if the ID already exists or not
    if not exists then 
        Error $"Student with ID '{id}' not found."
    else
        //let remaining = state.Students |> List.filter (fun s -> s.Id <> id)
        //Ok { state with Students = remaining }   
        let newState = {state with Students = state.Students |> List.filter (fun s -> s.Id <> id) }
        Persistence.save path newState
        Ok newState // return new state without deleted student