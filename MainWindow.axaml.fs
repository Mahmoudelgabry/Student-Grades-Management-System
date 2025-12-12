namespace StudentGradesUI

open System

open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Interactivity
open Avalonia.Threading

open Models
open CRUD
open GradeCalculator
open Statistics
open Roles
open Persistence


type MainWindow() as this =
    inherit Window()

    do
        this.InitializeComponent()

        let mutable state = { Students = getStudents() ; CurrentUserRole = Admin }

        let ViewerButton = this.FindControl<Button>("ViewerButton")
        let AdminButton = this.FindControl<Button>("AdminButton")
        let LoggedPanal = this.FindControl<StackPanel>("LoggedPanal")

        // viewer validation
        let ViewerPanal = this.FindControl<StackPanel>("ViewerPanal")
        let ViewerEmail = this.FindControl<TextBox>("ViewerEmail")
        let ViewerPassword = this.FindControl<TextBox>("ViewerPassword")
        let ViewerLogin = this.FindControl<Button>("ViewerLogin")
        let ViewerLoginMsg = this.FindControl<TextBlock>("ViewerLoginMsg")

        // admin validation
        let AdminPanal = this.FindControl<StackPanel>("AdminPanal")
        let AdminEmail = this.FindControl<TextBox>("AdminEmail")
        let AdminPassword = this.FindControl<TextBox>("AdminPassword")
        let AdminLogin = this.FindControl<Button>("AdminLogin")
        let AdminLoginMsg = this.FindControl<TextBlock>("AdminLoginMsg")


        //-------------------------------------------------------------------
        let ShowStudentsButton = this.FindControl<Button>("ShowStudentsButton")  
        let ShowClassButton = this.FindControl<Button>("ShowClassButton")
        let AddStudentButton = this.FindControl<Button>("AddStudentButton")
        let UpdateStudentButton = this.FindControl<Button>("UpdateStudentButton")
        let DeleteStudentButton = this.FindControl<Button>("DeleteStudentButton")

        let StudentPanal = this.FindControl<StackPanel>("StudentPanal")
        let ClassPanal = this.FindControl<StackPanel>("ClassPanal")
        let AddStudentPanal = this.FindControl<StackPanel>("AddStudentPanal")
        let UpdateStudentPanal = this.FindControl<StackPanel>("UpdateStudentPanal")
        let DeleteStudentPanal = this.FindControl<StackPanel>("DeleteStudentPanal")

        let StudentCards = this.FindControl<WrapPanel>("StudentCards")
        let ClassDetails = this.FindControl<TextBlock>("ClassDetails")

        // add student page
        let AddStudentId = this.FindControl<TextBox>("AddStudentId")
        let AddStudentFirstName = this.FindControl<TextBox>("AddStudentFirstName")
        let AddStudentLastName = this.FindControl<TextBox>("AddStudentLastName")
        let AddSubmit = this.FindControl<Button>("AddSubmit")
        let AddStudentMsg = this.FindControl<TextBlock>("AddStudentMsg")

        // update student page
        let UpdateStudentId = this.FindControl<TextBox>("UpdateStudentId")
        let UpdateStudentFirstName = this.FindControl<TextBox>("UpdateStudentFirstName")
        let UpdateStudentLastName = this.FindControl<TextBox>("UpdateStudentLastName")
        let UpdateGradesText = this.FindControl<TextBlock>("UpdateGradesText")
        let Updatebtn = this.FindControl<Button>("Updatebtn")
        let SubmitUpdatebtn = this.FindControl<Button>("SubmitUpdatebtn")
        let CancleUpdatebtn = this.FindControl<Button>("CancleUpdatebtn")
        let UpdateStudentMsg = this.FindControl<TextBlock>("UpdateStudentMsg")
        let UpdateGrades = this.FindControl<StackPanel>("UpdateGrades")
        let SubjectName = this.FindControl<TextBox>("SubjectName")
        let SubjectGrade = this.FindControl<TextBox>("SubjectGrade")
        let AddGrade = this.FindControl<Button>("AddGrade")
        let mutable UpdatedGradesList : Grade list = []

        // delete student page
        let DeleteStudentId = this.FindControl<TextBox>("DeleteStudentId")
        let DeleteSubmit = this.FindControl<Button>("DeleteSubmit")
        let DeleteStudentMsg = this.FindControl<TextBlock>("DeleteStudentMsg")



        let clean (s: string) =
            if isNull s then "" else s.Trim()

        let showMessageTemporary (textBlock: TextBlock) (msg: string) (milliseconds: int) =
            async {
                do!
                    (Dispatcher.UIThread.InvokeAsync(fun () ->
                        textBlock.Text <- msg
                    ).GetTask())
                    |> Async.AwaitTask

                do! Async.Sleep milliseconds

                do!
                    (Dispatcher.UIThread.InvokeAsync(fun () ->
                        textBlock.Text <- ""
                    ).GetTask())
                    |> Async.AwaitTask
            }
            |> Async.Start

        let generateNextID (students: Student list) =
            let numbers =
                students |> List.choose (fun s ->
                    match System.Int32.TryParse(s.Id) with
                    | true, n -> Some n
                    | _ -> None
                )
            let nextNum =
                match numbers with
                | [] -> 1
                | _ -> (List.max numbers) + 1
            nextNum.ToString()



//-----------------------------validation----------------------------//
        ViewerButton.Click.Add(fun _ ->
            ViewerPanal.IsVisible <- true
            AdminPanal.IsVisible <- false
            LoggedPanal.IsVisible <- false
        )

        ViewerLogin.Click.Add(fun _ ->
            let email = clean ViewerEmail.Text
            let pass = clean ViewerPassword.Text

            if email = "" || pass = "" then
                showMessageTemporary ViewerLoginMsg "Please fill all fields." 3000
            else
                match validateLogin email pass "Viewer" with
                | Ok role ->
                    state <- {state with CurrentUserRole = Viewer}
                    ViewerPanal.IsVisible <- false
                    LoggedPanal.IsVisible <- true
                    AddStudentButton.IsVisible <- false
                    UpdateStudentButton.IsVisible <- false
                    DeleteStudentButton.IsVisible <- false
                    ViewerEmail.Text <-""
                    ViewerPassword.Text <-""
                | Error e ->
                    showMessageTemporary ViewerLoginMsg e 3000
        )



        AdminButton.Click.Add(fun _ ->
            AdminPanal.IsVisible <- true
            ViewerPanal.IsVisible <- false
            LoggedPanal.IsVisible <- false
        )

        AdminLogin.Click.Add(fun _ ->
            let email = clean AdminEmail.Text
            let pass = clean AdminPassword.Text

            if email = "" || pass = "" then
                showMessageTemporary AdminLoginMsg "Please fill all fields." 3000
            else
                match validateLogin email pass "Admin" with
                | Ok role ->
                    state <- {state with CurrentUserRole = Admin}
                    AdminPanal.IsVisible <- false
                    LoggedPanal.IsVisible <- true
                    AddStudentButton.IsVisible <- true
                    UpdateStudentButton.IsVisible <- true
                    DeleteStudentButton.IsVisible <- true
                    AdminEmail.Text <-""
                    AdminPassword.Text <-""
                | Error e ->
                    showMessageTemporary AdminLoginMsg e 3000
        )
//-------------------------------------------------------------------//
        ShowStudentsButton.Click.Add(fun _ ->
            StudentPanal.IsVisible <- true
            ClassPanal.IsVisible <- false
            AddStudentPanal.IsVisible <- false
            UpdateStudentPanal.IsVisible <- false
            DeleteStudentPanal.IsVisible <- false

            StudentCards.Children.Clear()
            state.Students
            |> List.iter (fun st ->
                let card = StudentCard()
                card.SetStudent(st , this)
                StudentCards.Children.Add(card) |> ignore
            )
        )

        ShowClassButton.Click.Add(fun _ ->
            StudentPanal.IsVisible <- false
            ClassPanal.IsVisible <- true
            AddStudentPanal.IsVisible <- false
            UpdateStudentPanal.IsVisible <- false
            DeleteStudentPanal.IsVisible <- false

            let stats = Statistics.computeClassStats state.Students

            let highestMsg =
                match stats.HighestAverage with
                | Some ha -> sprintf "Highest average : %.2f ( %s %s )" ha stats.HighestStudent.Value.FirstName stats.HighestStudent.Value.LastName
                | None -> "No highest average available."

            let lowestMsg =
                match stats.LowestAverage with
                | Some la -> sprintf "Lowest average : %.2f ( %s %s )" la stats.LowestStudent.Value.FirstName stats.LowestStudent.Value.LastName
                | None -> "No lowest average available."

            let passRateMsg =
                match stats.PassRate with
                | Some p -> sprintf "Pass rate : %.2f%%" p
                | None -> "Pass rate unavailable."
            
            ClassDetails.Text <- highestMsg+"\n\n"+lowestMsg+"\n\n"+passRateMsg
        )

        AddStudentButton.Click.Add(fun _ ->
            StudentPanal.IsVisible <- false
            ClassPanal.IsVisible <- false
            AddStudentPanal.IsVisible <- true
            UpdateStudentPanal.IsVisible <- false
            DeleteStudentPanal.IsVisible <- false
            AddStudentId.Text <- generateNextID state.Students
            AddStudentId.IsReadOnly <- true
        )

        AddSubmit.Click.Add(fun _ ->
            let id = clean AddStudentId.Text
            let fn = clean AddStudentFirstName.Text
            let ln = clean AddStudentLastName.Text

            if id = "" || fn = "" || ln = "" then
                showMessageTemporary AddStudentMsg "Please fill all fields." 3000
            else
                let student = { Id = id; FirstName = fn; LastName = ln; Grades = [] }
                match addStudent state student with
                | Ok newState ->
                    state <- newState
                    showMessageTemporary AddStudentMsg "student added sucessfully" 3000
                    AddStudentId.Text <- generateNextID state.Students
                    AddStudentId.IsReadOnly <- true
                    AddStudentFirstName.Text <- ""
                    AddStudentLastName.Text <- ""
                | Error e ->
                    showMessageTemporary AddStudentMsg e 3000
        )

        DeleteStudentButton.Click.Add(fun _ ->
            StudentPanal.IsVisible <- false
            ClassPanal.IsVisible <- false
            AddStudentPanal.IsVisible <- false
            UpdateStudentPanal.IsVisible <- false
            DeleteStudentPanal.IsVisible <- true
        )

        DeleteSubmit.Click.Add(fun _ ->
            let id = clean DeleteStudentId.Text

            if id = "" then
                showMessageTemporary DeleteStudentMsg "Please fill the id field." 3000
            else
                match deleteStudent state id with
                | Ok newState ->
                    state <- newState
                    showMessageTemporary DeleteStudentMsg "student deleted sucessfully" 3000
                    DeleteStudentId.Text <- ""
                | Error e ->
                    showMessageTemporary DeleteStudentMsg e 3000
        )

        UpdateStudentButton.Click.Add(fun _ ->
            StudentPanal.IsVisible <- false
            ClassPanal.IsVisible <- false
            AddStudentPanal.IsVisible <- false
            UpdateStudentPanal.IsVisible <- true
            DeleteStudentPanal.IsVisible <- false
        )

        Updatebtn.Click.Add(fun _ ->
            let id = clean UpdateStudentId.Text

            if id = "" then
                showMessageTemporary UpdateStudentMsg "Please fill the id field." 3000
            else
                match getStudent state id with
                | Error e ->
                    showMessageTemporary UpdateStudentMsg e 3000
                | Ok student ->
                    UpdateStudentFirstName.IsVisible <- true
                    UpdateStudentLastName.IsVisible <- true
                    UpdateGrades.IsVisible <- true
                    Updatebtn.IsVisible <- false
                    SubmitUpdatebtn.IsVisible <- true
                    CancleUpdatebtn.IsVisible <- true
                    UpdateStudentId.IsReadOnly <- true
                    UpdateStudentFirstName.Text <- student.FirstName
                    UpdateStudentLastName.Text <- student.LastName
                    UpdateGradesText.Text <-
                        if student.Grades.IsEmpty then "- No grades"
                        else
                            student.Grades
                            |> List.map (fun g -> $"- {g.Subject} : {g.Score}")
                            |> String.concat "\n"
        )

        CancleUpdatebtn.Click.Add(fun _ ->
            UpdateStudentFirstName.IsVisible <- false
            UpdateStudentLastName.IsVisible <- false
            UpdateGrades.IsVisible <- false
            Updatebtn.IsVisible <- true
            SubmitUpdatebtn.IsVisible <- false
            CancleUpdatebtn.IsVisible <- false
            UpdateStudentId.IsReadOnly <- false
            UpdateStudentId.Text <- ""
            UpdatedGradesList <- []
        )

        AddGrade.Click.Add(fun _ ->
            let SN = clean SubjectName.Text
            let SGTxt = clean SubjectGrade.Text

            if SN <> "" || SGTxt <> "" then
                match System.Double.TryParse(SGTxt) with
                | true, value when value >= 0 && value <= 100 ->
                    let Grade = { Subject = SN; Score = value }
                    UpdatedGradesList <- UpdatedGradesList @ [Grade]
                    UpdateGradesText.Text <- UpdateGradesText.Text + "\n" + "- " + SN + " : " + $"{value}"
                    SubjectName.Text <-""
                    SubjectGrade.Text <-""
                | true, _ ->
                    showMessageTemporary UpdateStudentMsg "Number must be between 0 and 100" 3000 
                | false, _ ->
                    showMessageTemporary UpdateStudentMsg "Invalid format" 3000 
            else
                showMessageTemporary UpdateStudentMsg "Please fill all fields." 3000
        )

        SubmitUpdatebtn.Click.Add(fun _ ->
            let id = clean UpdateStudentId.Text
            let fn = clean UpdateStudentFirstName.Text
            let ln = clean UpdateStudentLastName.Text

            if id = "" || fn = "" || ln = "" then
                showMessageTemporary UpdateStudentMsg "Please fill all fields." 3000
            else
                
                let NewGrades =
                    match getStudent state id with
                    | Error _ -> []
                    | Ok s -> if List.isEmpty UpdatedGradesList then s.Grades else s.Grades @ UpdatedGradesList
                
                let NewStudent = { Id = id; FirstName = fn; LastName = ln; Grades = NewGrades }
                
                match updateStudent state NewStudent with
                | Ok newState ->
                    state <- newState
                    showMessageTemporary UpdateStudentMsg "student updated sucessfully" 3000
                    UpdateStudentFirstName.IsVisible <- false
                    UpdateStudentLastName.IsVisible <- false
                    UpdateGrades.IsVisible <- false
                    Updatebtn.IsVisible <- true
                    SubmitUpdatebtn.IsVisible <- false
                    CancleUpdatebtn.IsVisible <- false
                    UpdateStudentId.IsReadOnly <- false
                    UpdateStudentId.Text <- ""
                    UpdatedGradesList <- []
                | Error e ->
                    showMessageTemporary UpdateStudentMsg e 3000
        )

    member this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)