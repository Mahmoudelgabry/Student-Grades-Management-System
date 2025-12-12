module Roles

open Models    // open Models.fs
open System.IO
open System.Text.Json


let private path = "users.json"

// get users
let getUsers () =
    if File.Exists(path) then
        let json = File.ReadAllText(path)
        JsonSerializer.Deserialize<User list>(json)
    else
        []


let validateLogin (email: string) (password: string) (role: string) =
    let users = getUsers()

    let foundUser = users |> List.tryFind (fun u -> u.Email = email)

    match foundUser with
    | None ->
        Error "Email not found"

    | Some user ->
        if user.Password <> password then
            Error "Wrong password"
        else if user.Role <> role then
            Error "Email not found"
        else
            Ok user.Role