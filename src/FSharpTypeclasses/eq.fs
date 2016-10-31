namespace FsharpTypeclasses

[<RequireQualifiedAccess>]
module Eq =
  type t<'a> = { eq : 'a -> 'a -> bool }

  let equality = { eq = (=) }

  module Ops = let eq t = t.eq
  module Laws = let reflexivity t a = Ops.eq t a a
