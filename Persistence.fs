module Persistence

open Models
open System.Text.Json
open System.Text.Json.Serialization
open System.IO

let private options =
    JsonSerializerOptions(PropertyNameCaseInsensitive = true, WriteIndented = true)

let save (path: string) (state: AppState) =
    let json = JsonSerializer.Serialize(state.Students, options)
    File.WriteAllText(path, json)

let load (path: string) : Result<Student list,string> =
    try
        if not (File.Exists path) then Error $"File '{path}' not found."
        else
            let txt = File.ReadAllText(path)
            let arr = JsonSerializer.Deserialize<Student list>(txt, options)
            match Option.ofObj arr with
            | None -> Error "Failed to deserialize JSON (null)."
            | Some l -> Ok l
    with ex ->
        Error $"Failed to load JSON: {ex.Message}"