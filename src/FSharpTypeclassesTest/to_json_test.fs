namespace FsharpTypeclassesTest

open FsharpTypeclasses
open NUnit.Framework
open To_json.Ops

[<TestFixture>]
module To_json_test =
  let option_int = To_json.option To_json.int

  [<Test>]
  let int () =
    Assert.AreEqual("1", to_json To_json.int 1)

  [<Test>]
  let string () =
    Assert.AreEqual("\"1\"", to_json To_json.string "1")

  [<Test>]
  let float () =
    Assert.AreEqual("1.0", to_json To_json.float 1.0)

  [<Test>]
  let double () =
    Assert.AreEqual("1.0", to_json To_json.double 1.0)

  [<Test>]
  let char () =
    Assert.AreEqual("\"1\"", to_json To_json.char '1')

  [<Test>]
  let bool () =
    Assert.AreEqual("false", to_json To_json.bool false)
    Assert.AreEqual("true", to_json To_json.bool true)

  [<Test>]
  let option () =
    Assert.AreEqual("null", to_json option_int None)
    Assert.AreEqual("1", to_json option_int <| Some 1)

  [<Test>]
  let array () =
    let array_int = To_json.array To_json.int

    Assert.AreEqual("[]", to_json array_int Array.empty)
    Assert.AreEqual("[1]", to_json array_int [| 1 |])
    Assert.AreEqual("[1,2]", to_json array_int [| 1; 2 |])

  type person = { name : string; age : option<int> }

  let name person = person.name
  let age person = person.age

  (** A person to JSON converter. *)
  let to_json_person =
    To_json.object2
      (key "name" << name, To_json.string)
      (key "age" << age, option_int)

  [<Test>]
  let object2 () =
    let person = { name = "bob"; age = Some 31 }
    let json = """{"name":"bob","age":31}"""

    Assert.AreEqual(json, to_json to_json_person person)
