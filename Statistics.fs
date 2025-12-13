module Statistics

open Models
open GradeCalculator

type ClassStats = {
    HighestStudent : Student option  // option was used because the class might be empty or the students might not have grades
    HighestAverage : float option
    LowestStudent : Student option
    LowestAverage : float option
    PassRate : float option  // fraction 0.0 .. 1.0
}

let private studentAveragePair (students: Student list) =
    students |> List.choose (fun student ->
    match average student with
    | Some a -> Some (student,a)   // return list of students and their average
    | None -> None)


let computeClassStats (students: Student list) : ClassStats =
    match students with
    | [] ->
        { HighestStudent=None; HighestAverage=None; LowestStudent=None; LowestAverage=None; PassRate=None }
    | _ ->
        let pairs = studentAveragePair students
        if pairs = [] then
            // no student had grades
            { HighestStudent=None; HighestAverage=None; LowestStudent=None; LowestAverage=None; PassRate=None }
        else
            let sorted = pairs |> List.sortBy snd   // sort list based on the second (snd) element (average)
            let lowest, lowestAvg = List.head sorted    
            let highest, highestAvg = List.last sorted
            let totalStudentsWithGrades = float (List.length pairs)
            let passCount =
                pairs
                |> List.filter (fun (_,a) -> a >= 50.0)
                |> List.length
                |> float
            { HighestStudent = Some highest
              HighestAverage = Some highestAvg
              LowestStudent = Some lowest
              LowestAverage = Some lowestAvg
              PassRate = Some ((passCount / totalStudentsWithGrades)* 100.0) }