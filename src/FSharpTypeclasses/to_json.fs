namespace FsharpTypeclasses

open System.Collections.Generic

[<RequireQualifiedAccess>]
module To_json =
  type t<'a> = { apply : 'a -> string }
  type key_value<'a> = KeyValuePair<string, 'a>

  let commalist = String.concat ","
  let enbrace s = "{" + s + "}"

  let int = { apply = sprintf "%d" }
  let string = { apply = sprintf "\"%s\"" }
  let float : t<float> = { apply = sprintf "%A" }
  let double : t<double> = { apply = sprintf "%A" }
  let char = { apply = sprintf "\"%c\"" }
  let bool = { apply = sprintf "%b" }
  let option t = { apply = Option.fold (fun _ -> t.apply) "null" }
  let array t =
    { apply = fun a ->
        "[" + (a |> Array.map t.apply |> commalist) + "]" }

  (**
  Returns a To_json instance that can convert a single key-value pair
  into a JSON string. Note that by itself this conversion isn't a
  well-formed JSON string; you'll need to encapsulate it in one of the
  `objectX` instances.

  @param t a JSON converter instance for the value in the key-value
  pair.
  *)
  let key_value t : t<key_value<'a>> =
    { apply = fun a ->
        (string.apply a.Key) + ":" + (t.apply a.Value) }

  let object1 (f1, t1) =
    { apply = enbrace << (key_value t1).apply << f1 }

  let object2 (f1, t1) (f2, t2) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply |]
          |> commalist |> enbrace }

  let object3 (f1, t1) (f2, t2) (f3, t3) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply
           a |> f3 |> (key_value t3).apply |]
          |> commalist |> enbrace }

  let object4 (f1, t1) (f2, t2) (f3, t3) (f4, t4) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply
           a |> f3 |> (key_value t3).apply
           a |> f4 |> (key_value t4).apply |]
          |> commalist |> enbrace }

  let object5 (f1, t1) (f2, t2) (f3, t3) (f4, t4) (f5, t5) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply
           a |> f3 |> (key_value t3).apply
           a |> f4 |> (key_value t4).apply
           a |> f5 |> (key_value t5).apply |]
          |> commalist |> enbrace }

  let object6 (f1, t1) (f2, t2) (f3, t3) (f4, t4) (f5, t5) (f6, t6) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply
           a |> f3 |> (key_value t3).apply
           a |> f4 |> (key_value t4).apply
           a |> f5 |> (key_value t5).apply
           a |> f6 |> (key_value t6).apply |]
          |> commalist |> enbrace }

  let object7
    (f1, t1) (f2, t2) (f3, t3) (f4, t4) (f5, t5) (f6, t6) (f7, t7) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply
           a |> f3 |> (key_value t3).apply
           a |> f4 |> (key_value t4).apply
           a |> f5 |> (key_value t5).apply
           a |> f6 |> (key_value t6).apply
           a |> f7 |> (key_value t7).apply |]
          |> commalist |> enbrace }

  let object8
    (f1, t1)
    (f2, t2)
    (f3, t3)
    (f4, t4)
    (f5, t5)
    (f6, t6)
    (f7, t7)
    (f8, t8) =
    { apply = fun a ->
        [| a |> f1 |> (key_value t1).apply
           a |> f2 |> (key_value t2).apply
           a |> f3 |> (key_value t3).apply
           a |> f4 |> (key_value t4).apply
           a |> f5 |> (key_value t5).apply
           a |> f6 |> (key_value t6).apply
           a |> f7 |> (key_value t7).apply
           a |> f8 |> (key_value t8).apply |]
          |> commalist |> enbrace }

  module Ops =
    let key (name : string) value = KeyValuePair(name, value)
    let to_json t = t.apply
    let make_to_json f = { apply = f }
