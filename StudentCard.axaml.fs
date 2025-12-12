namespace StudentGradesUI

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Media
open Models
open Avalonia.Interactivity

type StudentCard() as this =
    inherit UserControl()

    let nameText = lazy (this.FindControl<TextBlock>("NameText"))
    let idText = lazy (this.FindControl<TextBlock>("IdText"))
    let gradesText = lazy (this.FindControl<TextBlock>("GradesText"))
    let detailsButton = lazy (this.FindControl<Button>("DetailsButton"))

    let mutable currentStudent : Student option = None
    let mutable parentWindow : Window option = None

    do
        AvaloniaXamlLoader.Load(this)

        detailsButton.Force().Click.Add(fun _ ->
            match currentStudent, parentWindow with
            | Some s, Some wnd ->
                let popup = Window()
                popup.Title <- "Student Details"
                popup.Width <- 300.0
                popup.Height <- 200.0

                let stack = StackPanel(Margin = Avalonia.Thickness(10.0))
                let nameBlock = TextBlock(Text = $"Name : {s.FirstName} {s.LastName}", FontWeight = FontWeight.Bold , Margin = Avalonia.Thickness(0.0,0.0,0.0,10.0))
                let idBlock = TextBlock(Text = $"ID : {s.Id}" , Margin = Avalonia.Thickness(0.0,0.0,0.0,10.0))
                let gradesBlock =
                    if s.Grades.IsEmpty then TextBlock(Text = "No grades")
                    else TextBlock(Text = String.concat "\n" (s.Grades |> List.map (fun g -> $"{g.Subject} : {g.Score}")),LineHeight = 20.0)

                stack.Children.Add(nameBlock) |> ignore
                stack.Children.Add(idBlock) |> ignore
                stack.Children.Add(gradesBlock) |> ignore

                popup.Content <- stack
                popup.ShowDialog(wnd) |> ignore
            | _ -> ()
        )

    /// Set the student and the parent window
    member this.SetStudent(student: Student, parent: Window) =
        currentStudent <- Some student
        parentWindow <- Some parent
        nameText.Force().Text <- $"{student.FirstName} {student.LastName}"
        idText.Force().Text <- $"ID : {student.Id}"
        gradesText.Force().Text <-
            if student.Grades.IsEmpty then "No grades"
            else
                student.Grades
                |> List.map (fun g -> $"{g.Subject} : {g.Score}")
                |> String.concat "\n"
