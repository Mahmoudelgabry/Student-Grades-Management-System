module GradeCalculatorTests

open Xunit
open Models
open GradeCalculator

[<Fact>]
let ``Average is calculated correctly`` () =
    let student =
        {
            Id = "1"
            FirstName = "Mahmoud"
            LastName = "Gabry"
            Grades = [
                { Subject = "Math"; Score = 80.0 }
                { Subject = "CS"; Score = 90.0 }
            ]
        }

    let avg = average student
    Assert.Equal(Some 85.0, avg)

[<Fact>]
let ``Student passes when average is 50 or more`` () =
    let student =
        {
            Id = "1"
            FirstName = "Sara"
            LastName = "Mohamed"
            Grades = [
                { Subject = "Math"; Score = 60.0 }
            ]
        }

    Assert.True(isPass student 50.0)

