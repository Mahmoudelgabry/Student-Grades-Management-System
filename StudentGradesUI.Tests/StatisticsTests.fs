module StatisticsTests

open Xunit
open Models
open Statistics

[<Fact>]
let ``Highest average is calculated correctly`` () =
    let students =
        [
            {
                Id = "1"
                FirstName = "Mahmoud"
                LastName = "Gabry"
                Grades = [ { Subject = "Math"; Score = 80.0 } ]
            }
            {
                Id = "2"
                FirstName = "Sara"
                LastName = "Mohamed"
                Grades = [ { Subject = "Math"; Score = 60.0 } ]
            }
        ]

    let stats = computeClassStats students

    Assert.Equal(Some 80.0, stats.HighestAverage)


[<Fact>]
let ``Pass rate is calculated correctly`` () =
    let students =
        [
            {
                Id = "1"
                FirstName = "Mahmoud"
                LastName = "Gabry"
                Grades = [ { Subject = "Math"; Score = 80.0 } ]
            }
            {
                Id = "2"
                FirstName = "Sara"
                LastName = "Mohamed"
                Grades = [ { Subject = "Math"; Score = 40.0 } ]
            }
        ]

    let stats = computeClassStats students

    Assert.Equal(Some 50.0, stats.PassRate)
