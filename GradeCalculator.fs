module GradeCalculator
open Models   

 
let totalScore (student: Student) =
    student.Grades |> List.sumBy (fun g -> g.Score)

 
let average (student: Student) =
    match student.Grades with
    | [] -> None
    | g ->
        let sum = totalScore student
        let avg = sum / float (List.length g)
        Some avg


 
let isPass (student: Student) (threshold: float) =
    match average student with
    | Some average -> average >= threshold
    | None -> false